using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Command.Order;

public sealed record OrderAdd(string UserId
    , List<OrderItem> OrderItems
    , string AddressId // id địa chỉ khách    
) : ICommand<Response>;

public sealed record OrderItem(string ProductId, string VariantId, int Quantity);

public sealed record OrderUpdate(string UserId
    , string OrderId
    , List<OrderItem> OrderItems
    , string AddressId // id địa chỉ khách    
) : ICommand<Response>;

public sealed record OrderDelete(string UserId
    , string OrderId
) : ICommand<Response>;