using LibCli;
using LibCli.Attributes;
using Microsoft.Extensions.CommandLineUtils;

namespace Tests.Examples.Commands;

public record TestCommand : ICommandSpecification
{
    public string Name => "test";

    [FromOptions("-s|--string", "test string option", CommandOptionType.SingleValue)]
    public string? TestOption { get; set; }

    [FromOptions("-i|--int", "test int option", CommandOptionType.SingleValue)]
    public int TestIntOption { get; set; }
}