using Microsoft.Extensions.DependencyInjection;
using Recycle.CLI.Interfaces;
using Recycle.CLI.Services;
using Tests.Examples.Commands;
using Tests.Examples.Handlers;

var serviceProvider = new ServiceCollection()
    .AddSingleton<IService, Service>()
    .BuildServiceProvider();

var app = new CommandLineAppBuilder(serviceProvider)
    .Configure((app) =>
    {
        app.Name = "Recycle.CLI";
        app.Description = "Helper tool for Recycle project";
        app.HelpOption("-h|--help");
    })
    .AddCommand<TestCommand, TestCommandHandler>(scb =>
    {
        scb.AddCommand<TestCommand, TestCommandHandler>();
    })
    .Build()
    .Execute(args);
