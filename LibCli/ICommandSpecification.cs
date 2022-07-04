namespace LibCli;

public interface ICommandSpecification
{
    /// <summary>
    /// Defines name to invoke command from CLI interface
    /// e.g. `cli-app <command-name>`
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Defines help option flag. If set to null, help option will not be created.
    /// </summary>
    string? HelpOption { get => "-h|--help"; }
}