using Laborie.Service.Application.DTOs.Address;
using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.ValueObjects;
using Mapster;

namespace Laborie.Service.Infrastructure.Configurations;

public class MapsterConfig
{
    public static void Configure()
    {
        TypeAdapterConfig.GlobalSettings.Default.PreserveReference(true);

        TypeAdapterConfig<LaborieAgency, ProfileItemDto>
            .NewConfig()
            .Map(dest => dest.ReferCommission, src => src.Commission)
            .Map(dest => dest.ReferCode, src => src.InviteCode);      
    }
}
