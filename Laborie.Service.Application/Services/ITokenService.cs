using Laborie.Service.Domain.Entities.Mongo.Laborie;

namespace Laborie.Service.Application.Services;

public interface ITokenService
{
    string GenerateToken(LaborieAgency agency, string deviceToken);
}
