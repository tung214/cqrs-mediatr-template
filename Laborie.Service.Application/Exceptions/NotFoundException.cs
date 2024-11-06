namespace Laborie.Service.Application.Exceptions;
public abstract class NotFoundException(string message) : ApplicationException("Not found", message)
{
}
