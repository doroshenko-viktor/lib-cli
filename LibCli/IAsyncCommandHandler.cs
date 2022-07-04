namespace LibCli.Interfaces;

public interface IAsyncCommandHandler<TCommand>
    where TCommand : ICommandSpecification
{
    Task<int> Handle(TCommand command);
}