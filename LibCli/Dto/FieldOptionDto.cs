using System.Reflection;
using Microsoft.Extensions.CommandLineUtils;

namespace LibCli.Dto;

public record struct FieldOptionDto
{
    public FieldOptionDto(PropertyInfo field, CommandOption option)
    {
        Field = field;
        Option = option;
    }

    public PropertyInfo Field { get; }
    public CommandOption Option { get; }
}