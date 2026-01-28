namespace Vendas.Application.Abstractions.Commands;

public interface ICommandHandler<in TCommand, TResult>
{
    public Task<TResult> HandleAsync(TCommand command, CancellationToken cancellationToken);
}