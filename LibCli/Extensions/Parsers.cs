using LibCli.Exceptions;
using Microsoft.Extensions.CommandLineUtils;

namespace LibCli.Extensions;

public static class CommandOptionExtensions
{
    public static int ParseInt(this CommandOption option)
    {
        try
        {
            return int.Parse(option.Value());
        }
        catch
        {
            throw new ParsingException(option.Template, "int");
        }
    }
}