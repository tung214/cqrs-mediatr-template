using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Queries.Cart;

public sealed record GetCartQuery(string UserId) : IQuery<Response>;