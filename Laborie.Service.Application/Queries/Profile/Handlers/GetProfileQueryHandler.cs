using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Profile.Handlers;


public class GetProfileQueryHandler(ILogger<GetProfileQueryHandler> logger
    , IMongoRepository<LaborieAgency> agencyRepo
)
: IQueryHandler<GetProfileQuery, Response>
{
    public async Task<Response> Handle(GetProfileQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await agencyRepo.FindOneAsync(x => x.Id == request.UserId && x.IsDelete == false);
            if (agency == null || agency.Status != Shared.Constant.AgencyStatus.ACTIVE)
                return new Response(StatusCodes.Status204NoContent, "Tài khoản không tồn tại", "");

            var data = agency.Adapt<ProfileItemDto>();
            return new Response<ProfileItemDto>(StatusCodes.Status200OK, "Success", "") with
            {
                Data = data
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}

