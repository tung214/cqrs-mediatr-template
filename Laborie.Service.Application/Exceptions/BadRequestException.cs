namespace Laborie.Service.Application.Exceptions;

public abstract class BadRequestException(string message) : ApplicationException("Bad Request", message)
{
}
