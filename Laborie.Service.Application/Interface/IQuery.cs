using MediatR;

namespace Laborie.Service.Application.Interface;

public interface IQuery<out TResponse> : IRequest<TResponse>
{
}
