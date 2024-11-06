using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;

namespace Laborie.Service.Application.Queries.Profile;

public sealed record GetProfileQuery(string UserId) : IQuery<Response>;