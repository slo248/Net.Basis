// Copyright (c) Slo248.
// Licensed under the MIT License.

namespace Slo248.Net.Cqrs.Requests;

public interface IRequest : MediatR.IRequest
{
}

public interface IRequest<out TResult> : MediatR.IRequest<TResult>
{
}
