using MediatR;

namespace Laborie.Service.Application.Interface;

public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, TResponse>
     where TQuery : IQuery<TResponse>
{
}
