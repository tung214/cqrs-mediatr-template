using System.Text;
using System.Text.Json;
using Laborie.Service.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ApplicationException = Laborie.Service.Application.Exceptions.ApplicationException;

namespace Laborie.Service.Application.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;

    public ExceptionHandlerMiddleware(RequestDelegate next
        , ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext httpContext)
    {
        try
        {
            // string method = httpContext.Request.Method;
            // string apiPath = httpContext.Request.Path.Value;

            var request = httpContext.Request;
            var connection = httpContext.Connection;
            // Lấy địa chỉ IP của client
            var clientIpAddress = httpContext.Connection.RemoteIpAddress?.ToString();
            // Lấy cổng client sử dụng
            var clientPort = httpContext.Connection.RemotePort;

            _logger.LogDebug("Request client info: {clientIpAddress}:{clientPort}", clientIpAddress, clientPort);

            var requestContent = "";
            if (request.ContentLength > 0)
            {
                request.EnableBuffering();
                var buffer = new byte[Convert.ToInt32(request.ContentLength)];
                await request.Body.ReadAsync(buffer, 0, buffer.Length);
                //get body string here...
                requestContent = Encoding.UTF8.GetString(buffer);
                request.Body.Position = 0;  //rewinding the stream to 0
            }

            _logger.LogDebug("Request body: {requestContent}", requestContent);

            await _next(httpContext);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Handler request error {message}", ex.Message);
            await HandleExceptionAsync(httpContext, ex);
        }
    }

    private static Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        var statusCode = GetStatusCode(exception);

        context.Response.ContentType = "application/json";

        context.Response.StatusCode = statusCode;

        // await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        return context.Response.WriteAsync(JsonSerializer.Serialize(new
        {
            context.Response.StatusCode,
            Message = GetTitle(exception),
            InnerMessage = GetErrors(exception)
        }));
    }

    private static int GetStatusCode(Exception exception) =>
           exception switch
           {
               BadRequestException => StatusCodes.Status400BadRequest,
               NotFoundException => StatusCodes.Status404NotFound,
               ValidationException => StatusCodes.Status422UnprocessableEntity,
               UnauthorizedAccessException => StatusCodes.Status401Unauthorized,
               _ => StatusCodes.Status500InternalServerError
           };

    private static string GetTitle(Exception exception) =>
        exception switch
        {
            ApplicationException applicationException => applicationException.Title,
            UnauthorizedAccessException => "Unauthorized",
            _ => "Server Error"
        };

    private static IReadOnlyDictionary<string, string[]> GetErrors(Exception exception)
    {
        IReadOnlyDictionary<string, string[]>? errors = null;

        if (exception is ValidationException validationException)
        {
            errors = validationException.ErrorsDictionary;
        }
        else
        {
            return GetInnerMessage(exception);
        }

        return errors;
    }

    private static Dictionary<string, string[]> GetInnerMessage(Exception ex)
    {
        var innerMessages = new Dictionary<string, string[]>();

        Exception? innerException = ex.InnerException;
        int count = 1;

        while (innerException != null)
        {
            innerMessages.Add($"Error{count}", [innerException.Message]);
            innerException = innerException.InnerException;
            count++;
        }

        return innerMessages;
    }
}
