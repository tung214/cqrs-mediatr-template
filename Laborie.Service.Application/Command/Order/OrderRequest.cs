namespace Laborie.Service.Application.Command.Order;

public sealed record OrderRequest(List<OrderItemRequest> OrderItems
    , string AddressId // id địa chỉ khách
);

public sealed record OrderItemRequest(string ProductId, string VariantId, int Quantity);