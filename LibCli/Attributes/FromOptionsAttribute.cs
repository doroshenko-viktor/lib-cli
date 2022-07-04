using Microsoft.Extensions.CommandLineUtils;

namespace LibCli.Attributes;

[AttributeUsage(AttributeTargets.Property)]
public class FromOptionsAttribute : Attribute
{
    public string Template { get; }
    public string Description { get; }
    public CommandOptionType Type { get; }

    public FromOptionsAttribute(
        string template,
        string description,
        CommandOptionType type)
    {
        Template = template;
        Description = description;
        Type = type;
    }
}