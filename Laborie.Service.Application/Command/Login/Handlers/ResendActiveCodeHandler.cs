using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Application.Services;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class ResendActiveCodeHandler(ILogger<ResendActiveCodeHandler> logger
        , IMongoRepository<LaborieAgency> agencyRepo
        , IEmailService emailService)
    : ICommandHandler<ResendActiveCode, Response>
    {
        public async Task<Response> Handle(ResendActiveCode request, CancellationToken cancellationToken)
        {
            try
            {
                // validate exist email
                var exist = await agencyRepo.FindOneAsync(x => x.Email == request.Email && x.IsDelete == false);
                if (exist == null)
                    return new Response(StatusCodes.Status404NotFound, "Tài khoản không đúng");

                if (exist.Status == Shared.Constant.AgencyStatus.ACTIVE && string.IsNullOrEmpty(exist.Password))
                    return new Response<RequireSetPasswordDto>(StatusCodes.Status205ResetContent, "Tài khoản đã được kích hoạt, nhập mật khẩu ban đầu!")
                    {
                        Data = new RequireSetPasswordDto { UserId = exist.Id, RequireSetPassword = true }
                    };

                var activeCode = PasswordGenerator.GeneratePassword(6);
                var hashActiveCode = StringExtensions.GenerateSaltedHash(activeCode, exist.Salt);

                exist.ResetPassword = hashActiveCode;
                exist.ResetDate = DateTime.Now;
                exist.ResetCount += 1;

                // save
                var updateResult = await agencyRepo.Update(new Dictionary<string, object?>
                    {
                        {nameof(exist.ResetPassword), exist.ResetPassword},
                        {nameof(exist.ResetDate), exist.ResetDate},
                        {nameof(exist.ResetCount), exist.ResetCount}
                    }, exist.Id);

                if (updateResult.ModifiedCount == 0)
                    return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Update Agency fail");

                var body =
                @"<!DOCTYPE html>
                <html lang=""en"">
                <head>
                    <meta charset=""UTF-8"">
                    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
                    <title>Mã Kích Hoạt Tài Khoản</title>
                    <style>
                        .container {
                            font-family: Arial, sans-serif;
                            text-align: center;
                            margin-top: 50px;
                        }
                        .activation-code {
                            display: inline-block;
                            background-color: #f0f0f0;
                            color: #333;
                            padding: 10px 20px;
                            margin-top: 10px;
                            font-size: 24px;
                            font-weight: bold;
                            border-radius: 5px;
                        }
                    </style>
                </head>
                <body>
                    <div class=""container"">
                        <p>Mã kích hoạt tài khoản của bạn là</p>
                        <div class=""activation-code"">%ACTIVECODE%</div>
                    </ div >
                </ body >
                </ html > ";

                // Send email with active code
                await emailService.SendMail(exist.Email
                    , "Kích hoạt tài khoản Laborie"
                    , body.Replace("%ACTIVECODE%", activeCode)
                );

                return new Response(StatusCodes.Status202Accepted, "Success", "");
            }
            catch (System.Exception ex)
            {
                logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
                return new Response(StatusCodes.Status500InternalServerError, "Lỗi gửi mail kích hoạt", ex.Message);
            }
        }
    }
}