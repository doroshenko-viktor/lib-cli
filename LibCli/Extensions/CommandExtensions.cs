using LibCli.Dto;

namespace LibCli.Extensions;

internal static class CommandExtensions
{
    public static void PopulateFields<TCommand>(
        this TCommand command,
        List<FieldOptionDto> options
    )
        where TCommand : ICommandSpecification
    {
        options.ForEach((x) => x.Field.SetValue(command, x.Option.Value()));
    }
}