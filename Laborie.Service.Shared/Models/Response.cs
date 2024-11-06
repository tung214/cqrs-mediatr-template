namespace Laborie.Service.Shared.Models;
public record Response(int StatusCode, string Message, string InternalMessage = "");
public record Response<TData>(int StatusCode, string Message, string InternalMessage = "") : Response(StatusCode, Message, InternalMessage)
{
    public TData? Data { get; set; }
}

public record ResponsePaging<TData>(int StatusCode, string Message, string InternalMessage = "") : Response(StatusCode, Message, InternalMessage)
{
    public int TotalPage { get; set; }
    public int Total { get; set; }
    public List<TData>? Data { get; set; }
}
