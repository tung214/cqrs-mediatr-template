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
public class LoginHandler(ILogger<LoginHandler> logger
    , IMongoRepository<LaborieAgency> agencyRepo
    , ITokenService tokenService)
: ICommandHandler<Login, Response>
{
    public async Task<Response> Handle(Login request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await agencyRepo.FindOneAsync(x => x.Email == request.Email && x.IsDelete == false);
            if (agency == null) return new Response(StatusCodes.Status406NotAcceptable, "Tài khoản hoặc mật khẩu không đúng", "");

            var hashPassword = StringExtensions.GenerateSaltedHash(request.Password, agency.Salt);
            if (!hashPassword.Equals(agency.Password))
                return new Response(StatusCodes.Status406NotAcceptable, "Tài khoản hoặc mật khẩu không đúng");

            var token = tokenService.GenerateToken(agency, request.DeviceToken);

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
