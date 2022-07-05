using Examples.Services;
using LibCli;
using Microsoft.Extensions.DependencyInjection;
using Tests.Examples.Commands;
using Tests.Examples.Handlers;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IService, Service>()
    .BuildServiceProvider();

var app = new CommandLineAppBuilder(serviceProvider)
    .Configure((app) =>
    {
        app.Name = "Example.CLI";
        app.Description = "CLI description";
        app.HelpOption("-h|--help");
    })
    .AddCommand<TestCommand, TestCommandHandler>(scb =>
    {
        scb.AddCommand<TestCommand, TestCommandHandler>();
    })
    .Build()
    .Execute(args);
