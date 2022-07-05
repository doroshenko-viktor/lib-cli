# Easy CLI

This library is built on top of more low level `Microsoft.Extensions.CommandLineUtils` NuGet package
with intention to make build configuration of `CLI` application as easy and fast as possible.

This library allows to setup `CLI` configuration with several lines of code. Commands are configured
simply by definition of command and handler classes.
It supports automatic dependency injection. Currently only with `IServiceProvider`.
It supports nesting of commands.

To configure simple `CLI` app:

- Create `DI` container:

    ```charp
    var serviceProvider = new ServiceCollection()
        .AddSingleton<IService, Service>()
        .BuildServiceProvider();
    ```
- Create new instance of `CommandLineAppBuilder`:

    ```charp
    var app = new CommandLineAppBuilder(serviceProvider);
    ```

- Now using fluent api on `CommandLineAppBuilder` it is possible to configure application:

    ```csharp
    var app = new CommandLineAppBuilder(serviceProvider)
    .Configure((app) =>
    {
        app.Name = "Example.CLI";
        app.Description = "CLI description";
        app.HelpOption("-h|--help");
    })
    ```
  Method `Configure` simply exposes `Microsoft.Extensions.CommandLineUtils.CommandLineApplication`.
  Here it is possible to make any configuration as you could do it without `EasyCli`. This is made
  with intention to not limit possible usecases of this library and allow it to be more versatile.

- Now the main part - we need to create command, which must be an object, implementing `ICommandSpecification` interface:

    ```csharp
    public record TestCommand : ICommandSpecification
    {
        public string Name => "test";
    }
    ```

  `ICommandSpecification` forces implementor to define name of the command, which will be used as
  a command name on `CLI` invocation. And optionally to define option template for help menu for
  that command. By default it will be create with template `-h|--help`. But if you don't need help
  functionality for this command, you can set this to `null` and it will not be created.

  Command may contain any number of fields. `EasyCli` will populate those fields marked by
  `FromOptions` attribute. It receives `string` template, which maps this field to optional cli
  attribute, e.g. `-f|--file`, `string` description, which is used in help menu and `Microsoft.Extensions.CommandLineUtils.CommandOptionType`, which defines, how many values should be mapped.


  *Note: this is a user responsibility here to maintain consistency between `CommandOptionType` and
  actual field value. e.g. if you choose `CommandOptionType.NoValue` than field value should likely
  be `bool` and in case of `CommandOptionType.MultipleValue` field value should be a collection.
  At the moment there is no compile time check for this.*

- Now create a handler for the command. It must implement `IAsyncCommandHandler<TCommand>` interface:

    ```csharp
    public interface IAsyncCommandHandler<TCommand>
        where TCommand : ICommandSpecification
    {
        Task<int> Handle(TCommand command);
    }
    ```

  For example:

    ```csharp
    public class TestCommandHandler : IAsyncCommandHandler<TestCommand>
    {
        private readonly IService _service;

        public TestCommandHandler(IService service)
        {
            _service = service;
        }

        public Task<int> Handle(TestCommand command)
        {
            Console.WriteLine(command.TestIntOption);
            Console.WriteLine(_service.GetString());
            Console.WriteLine(command.TestOption ?? "default test value");
            return Task.FromResult(0);
        }
    }
    ```

  Here `IService` is a user defined service, which will be injected into handler by `DI` container.


## TODO:

- Proper mapping into `CommandOptionType.MultipleValue` and `CommandOptionType.NoValue`
