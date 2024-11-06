using MediatR;

namespace Laborie.Service.Application.Interface;

public interface ICommand<out TResponse> : IRequest<TResponse>
{
}
