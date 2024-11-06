using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Login.Handlers;
public class ChangePasswordHandler(ILogger<ChangePasswordHandler> logger
    , IMongoRepository<LaborieAgency> agencyRepo
) : ICommandHandler<ChangePassword, Response>
{
    public async Task<Response> Handle(ChangePassword request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await agencyRepo.FindOneAsync(x => x.Id == request.UserId && x.IsDelete == false);
            if (agency == null)
                return new Response(StatusCodes.Status404NotFound, "Tài khoản hoặc mật khẩu không đúng");

            // check old password/reset code valid
            var hashOldPassword = StringExtensions.GenerateSaltedHash(request.OldPassword, agency.Salt);
            if (!hashOldPassword.Equals(agency.Password) && hashOldPassword.Equals(agency.ResetPassword))
                return new Response(StatusCodes.Status404NotFound, "Tài khoản hoặc mật khẩu không đúng");

            var hashPassword = StringExtensions.GenerateSaltedHash(request.NewPassword, agency.Salt);
            agency.Password = hashPassword;
            agency.ModifiedDate = DateTime.Now;
            agency.ResetCount = 0;

            // save
            var updateResult = await agencyRepo.Update(new Dictionary<string, object?>
            {
                {nameof(agency.Password), agency.Password},
                {nameof(agency.ModifiedDate), agency.ModifiedDate},
                {nameof(agency.ResetCount), agency.ResetCount}
            }, agency.Id);

            if (updateResult.ModifiedCount == 0)
                return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Set password Agency fail");

            return new Response(StatusCodes.Status202Accepted, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}
