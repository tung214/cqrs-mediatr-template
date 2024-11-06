using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Queries.Address;

public sealed record GetAddressQuery(string UserId) : IQuery<Response>;