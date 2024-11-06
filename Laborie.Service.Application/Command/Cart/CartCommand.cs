using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Command.Cart;

public sealed record AddCart(string UserId, string ProductId, string VariantId, int Quantity) : ICommand<Response>;
public sealed record RemoveCart(List<string> Ids) : ICommand<Response>;
public sealed record UpdateCart(string Id, int Quantity) : ICommand<Response>;