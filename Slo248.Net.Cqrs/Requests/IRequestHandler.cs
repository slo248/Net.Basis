// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.Cqrs.Requests;

public interface IRequestHandler<in TRequest> : MediatR.IRequestHandler<TRequest> where TRequest : IRequest
{
}

public interface IRequestHandler<in TRequest, TResult> : MediatR.IRequestHandler<TRequest, TResult>
    where TRequest : IRequest<TResult>
{
}
