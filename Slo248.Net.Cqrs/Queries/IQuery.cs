using Slo248.Net.Cqrs.Requests;

namespace Slo248.Net.Cqrs.Queries;

public interface IQuery<out TResult> : IRequest<TResult>
{
}
