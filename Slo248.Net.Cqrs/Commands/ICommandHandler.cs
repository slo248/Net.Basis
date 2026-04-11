using Slo248.Net.Cqrs.Requests;

namespace Slo248.Net.Cqrs.Commands;

public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand> where TCommand : ICommand
{
}

public interface ICommandHandler<in TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : ICommand<TResult>
{
}
