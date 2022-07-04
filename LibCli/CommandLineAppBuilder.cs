using System.Reflection;
using LibCli.Extensions;
using LibCli.Interfaces;
using LibCli.Services;
using Microsoft.Extensions.CommandLineUtils;
using Microsoft.Extensions.DependencyInjection;

namespace LibCli;

public class CommandLineAppBuilder
{
    private readonly CommandLineApplication _app;
    private readonly ServiceProvider _serviceProvider;

    public CommandLineAppBuilder(
        ServiceProvider serviceProvider,
        CommandLineApplication? app = default)
    {
        _serviceProvider = serviceProvider;
        _app = app ?? new CommandLineApplication();
    }

    public CommandLineAppBuilder AddCommand<TCommand, THandler>(Action<CommandLineAppBuilder> registerSubcommands)
        where TCommand : ICommandSpecification
        where THandler : IAsyncCommandHandler<TCommand>
    {
        var command = Activator.CreateInstance<TCommand>();

        _app.Command(command.Name, (app) =>
        {
            var subcommandBuilder = new CommandLineAppBuilder(_serviceProvider, app);
            registerSubcommands(subcommandBuilder);
            app = subcommandBuilder.Build();

            if (command.HelpOption is not null)
            {
                app.HelpOption(command.HelpOption);
            }

            var options = ReflectionService.GetFieldToOptionMappings<TCommand>(app).ToList();

            app.OnExecute(async () =>
            {
                var commandHandler = CreateCommandHandler<TCommand, THandler>();
                command.PopulateFields(options);

                return await commandHandler.Handle(command);
            });
        });

        return this;
    }

    public CommandLineAppBuilder AddCommand<TCommand, THandler>()
        where TCommand : ICommandSpecification
        where THandler : IAsyncCommandHandler<TCommand>
    {
        return AddCommand<TCommand, THandler>((_) => { });
    }

    private THandler CreateCommandHandler<TCommand, THandler>()
        where TCommand : ICommandSpecification
        where THandler : IAsyncCommandHandler<TCommand>
    {
        var constructors = typeof(THandler).GetConstructors();
        ConstructorInfo constructor;
        if (constructors.Length == 1)
        {
            constructor = constructors[0];
        }
        else
        {
            constructor = constructors.First(c => c.GetParameters().Length > 0);
        }

        var constructorParams = constructor.GetParameters()
            .Select(param => _serviceProvider.GetRequiredService(param.ParameterType))
            .ToArray();

        return (THandler)constructor.Invoke(constructorParams);
    }

    public CommandLineAppBuilder Configure(Action<CommandLineApplication> configure)
    {
        configure(_app);
        return this;
    }

    public CommandLineApplication Build()
    {
        return _app;
    }
}