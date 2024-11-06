using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class ActiveHandler(ILogger<ActiveHandler> logger
        , IMongoRepository<LaborieAgency> agencyRepo)
    : ICommandHandler<Active, Response>
    {
        public async Task<Response> Handle(Active request, CancellationToken cancellationToken)
        {
            try
            {
                // get agency
                var exist = await agencyRepo.FindOneAsync(x => x.Email == request.Email && x.IsDelete == false);
                if (exist == null)
                    return new Response(StatusCodes.Status400BadRequest, "Tài khoản chưa được đăng ký");

                // check status is inactive
                if (exist.Status != Shared.Constant.AgencyStatus.INACTIVE)
                    return new Response(StatusCodes.Status400BadRequest, "Tài khoản đã được kích hoạt");

                // check agency expired active
                if (exist.CreatedDate.AddDays(1) < DateTime.Now)
                    return new Response(StatusCodes.Status400BadRequest, "Mã kích hoạt không hợp lệ");

                // check active code valid
                var hashActiveCode = StringExtensions.GenerateSaltedHash(request.ActiveCode, exist.Salt);
                if (!hashActiveCode.Equals(exist.ResetPassword))
                    return new Response(StatusCodes.Status400BadRequest, "Mã kích hoạt không hợp lệ");

                exist.Status = Shared.Constant.AgencyStatus.ACTIVE;
                exist.ModifiedDate = DateTime.Now;
                exist.ResetPassword = null;
                exist.ResetCount = 0;

                // save
                var updateResult = await agencyRepo.Update(new Dictionary<string, object?>
                    {
                        {nameof(exist.Status), exist.Status},
                        {nameof(exist.ModifiedDate), exist.ModifiedDate},
                        {nameof(exist.ResetPassword), exist.ResetPassword},
                        {nameof(exist.ResetCount), exist.ResetCount}
                    }, exist.Id);

                if (updateResult.ModifiedCount == 0)
                    return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Update Agency fail");

                return new Response<RequireSetPasswordDto>(StatusCodes.Status200OK, "Kích hoạt tài khoản thành công", "")
                {
                    Data = new RequireSetPasswordDto { UserId = exist.Id, RequireSetPassword = true }
                };
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
                return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
            }
        }
    }
}