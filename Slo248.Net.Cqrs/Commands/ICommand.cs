using Slo248.Net.Cqrs.Requests;

namespace Slo248.Net.Cqrs.Commands;

public interface ICommand : IRequest
{
}

public interface ICommand<out TResult> : IRequest<TResult>
{
}
