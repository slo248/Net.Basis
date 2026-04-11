using Slo248.Net.Cqrs.Requests;

namespace Slo248.Net.Cqrs.Queries;

public interface IQueryHandler<in TQuery, TResult> : IRequestHandler<TQuery, TResult> where TQuery : IQuery<TResult>
{
}
