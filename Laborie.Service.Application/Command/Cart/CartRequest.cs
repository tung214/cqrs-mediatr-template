namespace Laborie.Service.Application.Command.Cart;

public sealed record AddCartRequest(string ProductId, string VariantId, int Quantity);
public sealed record RemoveCartRequest(List<string> Ids);
public sealed record UpdateCartRequest(int Quantity);