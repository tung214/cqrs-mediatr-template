using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Queries.Home;


public sealed record GetBannerQuery() : IQuery<Response>;

public sealed record GetProductQuery() : IQuery<Response>;

public sealed record GetProductItemQuery(string Id, string VariantId) : IQuery<Response>;