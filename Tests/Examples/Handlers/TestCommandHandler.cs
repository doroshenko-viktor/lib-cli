using System;
using System.Threading.Tasks;
using LibCli.Interfaces;
using Recycle.CLI.Interfaces;
using Tests.Examples.Commands;

namespace Tests.Examples.Handlers;

public class TestCommandHandler : IAsyncCommandHandler<TestCommand>
{
    private readonly IService _service;

    public TestCommandHandler(IService service)
    {
        _service = service;
    }

    public Task<int> Handle(TestCommand command)
    {
        Console.WriteLine(_service.GetString());
        Console.WriteLine(command.TestOption ?? "default test value");
        return Task.FromResult(0);
    }
}