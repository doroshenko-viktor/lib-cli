using System.Reflection;
using LibCli.Attributes;
using LibCli.Dto;
using Microsoft.Extensions.CommandLineUtils;

namespace LibCli.Services;

internal static class ReflectionService
{
    /// <summary>
    /// This is a helper function, which maps field to it's corresponding CLI option.
    /// Why this needed: CommandOption is populated with actual value only at the moment
    /// of command execution and not at the moment of configuration. Until that happens
    /// we store this mapping.
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IEnumerable<FieldOptionDto> GetFieldToOptionMappings<TCommand>(CommandLineApplication app)
        where TCommand : ICommandSpecification
    {
        return typeof(TCommand).GetProperties().Select(field =>
        {
            var fromOptions = field.GetCustomAttribute<FromOptionsAttribute>();
            if (fromOptions is not null)
            {
                var option = app.Option(
                    template: fromOptions.Template,
                    description: fromOptions.Description,
                    optionType: fromOptions.Type);

                return (FieldOptionDto?)new FieldOptionDto(field, option);
            }

            return null;
        }).Where(x => x is not null)
        .Select(x => (FieldOptionDto)x!);
    }
}