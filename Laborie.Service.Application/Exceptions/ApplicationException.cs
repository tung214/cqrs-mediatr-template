namespace Laborie.Service.Application.Exceptions;


public abstract class ApplicationException(string title, string message) : Exception(message)
{
    public string Title { get; } = title;
}
