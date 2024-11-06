using Laborie.Service.Application.Interface;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Laborie.Service.Shared.Extensions;
using Laborie.Service.Application.DTOs.Profile;
using Laborie.Service.Domain.Repositories.Mongo;
using Mapster;
using Laborie.Service.Application.Services;
using Laborie.Service.Domain.Entities.Mongo.Laborie;

namespace Laborie.Service.Application.Command.Login.Handlers
{
    public class RegisterHandler(ILogger<RegisterHandler> logger
        , IMongoRepository<LaborieAgency> agencyRepo
        , IEmailService emailService
    )
    : ICommandHandler<Register, Response>
    {
        public async Task<Response> Handle(Register request, CancellationToken cancellationToken)
        {
            try
            {
                // validate exist email
                var exist = await agencyRepo.FindOneAsync(x => x.Email == request.Email); // không kiểm tra isDelete
                if (exist != null)
                    return new Response(StatusCodes.Status400BadRequest, "Email đã được sử dụng");


                var salt = StringExtensions.NextRandomStrings(16, 1).First();
                if (string.IsNullOrEmpty(salt))
                    return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Lỗi sinh SALT!");

                var activeCode = PasswordGenerator.GeneratePassword(6);
                var hashActiveCode = StringExtensions.GenerateSaltedHash(activeCode, salt);

                // new record 
                var agency = new LaborieAgency
                {
                    Name = request.Name,
                    Email = request.Email,
                    CreatedDate = DateTime.Now,
                    ModifiedDate = DateTime.Now,
                    ReferCode = request.ReferCode,
                    Status = Shared.Constant.AgencyStatus.INACTIVE,
                    Salt = salt,
                    ResetPassword = hashActiveCode
                };

                // save
                await agencyRepo.InsertOneAsync(agency);

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
                </body>
                </html> ";

                // Send email with active code
                await emailService.SendMail(request.Email
                    , "Kích hoạt tài khoản Laborie"
                    , body.Replace("%ACTIVECODE%", activeCode)
                );

                var data = agency.Adapt<ProfileItemDto>();

                return new Response<ProfileItemDto>(StatusCodes.Status201Created, "Created", "") with
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
}