namespace LibCli;

public interface ICommandHandler<TCommand>
    where TCommand : ICommandSpecification
{
    int Handle(TCommand command);
}