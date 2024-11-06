using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Application.Services;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Shared.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Login.Handlers;

public class SetPasswordHandler(ILogger<ActiveHandler> logger
    , IMongoRepository<LaborieAgency> agencyRepo
    , ITokenService tokenService)
: ICommandHandler<SetPassword, Response>
{
    public async Task<Response> Handle(SetPassword request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await agencyRepo.FindOneAsync(x => x.Id == request.UserId && x.IsDelete == false);
            if (agency == null || agency.Status != Shared.Constant.AgencyStatus.ACTIVE)
                return new Response(StatusCodes.Status404NotFound, "Tài khoản không đúng", "");

            var hashPassword = StringExtensions.GenerateSaltedHash(request.NewPassword, agency.Salt);

            agency.Password = hashPassword;
            agency.ModifiedDate = DateTime.Now;

            var token = tokenService.GenerateToken(agency, request.DeviceToken);

            // save
            var updateResult = await agencyRepo.Update(new Dictionary<string, object?>
            {
                {nameof(agency.Password), agency.Password},
                {nameof(agency.ModifiedDate), agency.ModifiedDate}
            }, agency.Id);

            if (updateResult.ModifiedCount == 0)
                return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Set password Agency fail");

            var profile = agency.Adapt<ProfileItemDto>();

            return new Response<LoginItemDto>(StatusCodes.Status200OK, "Success", "") with
            {
                Data = new LoginItemDto
                {
                    AccessToken = token,
                    Profile = profile
                }
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}
