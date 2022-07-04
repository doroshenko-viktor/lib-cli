using LibCli;
using LibCli.Attributes;
using Microsoft.Extensions.CommandLineUtils;

namespace Tests.Examples.Commands;

public record TestCommand : ICommandSpecification
{
    public string Name => "test";

    [FromOptions("-t|--test", "test option", CommandOptionType.SingleValue)]
    public string? TestOption { get; set; }
}