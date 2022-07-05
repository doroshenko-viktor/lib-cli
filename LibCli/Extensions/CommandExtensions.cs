using LibCli.Dto;
using LibCli.Exceptions;

namespace LibCli.Extensions;

internal static class CommandExtensions
{
    public static void PopulateFields<TCommand>(
        this TCommand command,
        List<FieldOptionDto> options
    )
        where TCommand : ICommandSpecification
    {
        static object Parse(Type type, string value)
        {
            if (type == typeof(string)) return value;
            if (type == typeof(byte)) return byte.Parse(value);
            if (type == typeof(short)) return short.Parse(value);
            if (type == typeof(int)) return int.Parse(value);
            if (type == typeof(long)) return long.Parse(value);
            if (type == typeof(bool)) return bool.Parse(value);
            if (type == typeof(decimal)) return decimal.Parse(value);
            if (type == typeof(double)) return double.Parse(value);
            if (type == typeof(char)) return char.Parse(value);

            throw new Exception();
        }

        options.ForEach((x) =>
        {
            var value = x.Option.Value();
            if (value is null) return;

            try
            {
                var parsed = Parse(x.Field.PropertyType, value);
                x.Field.SetValue(command, parsed);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw new ParsingException(x.Option.Template, x.Field.PropertyType.ToString());
            }
        });
    }
}