namespace LibCli.Exceptions;

public class ParsingException : Exception
{
    public ParsingException(string name, string type) :
        base($"Impossible to parse {name} into type {type}")
    { }
}