using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Command.Profile.Handlers;

public class ProfileUpdateHandler(ILogger<ProfileUpdateHandler> logger
    , IMongoRepository<LaborieAgency> agencyRepo
) : ICommandHandler<ProfileUpdate, Response>
{
    public async Task<Response> Handle(ProfileUpdate request, CancellationToken cancellationToken)
    {
        try
        {
            var agency = await agencyRepo.FindOneAsync(x => x.Id == request.UserId && x.IsDelete == false);
            if (agency == null || agency.Status != Shared.Constant.AgencyStatus.ACTIVE)
                return new Response(StatusCodes.Status404NotFound, "Tài khoản không tồn tại", "");
            var update = new Dictionary<string, object?>();

            if (!string.IsNullOrEmpty(request.Name)) update.Add(nameof(agency.Name), request.Name);
            if (!string.IsNullOrEmpty(request.Phone)) update.Add(nameof(agency.Phone), request.Phone);
            if (request.DateOfBirth != null) update.Add(nameof(agency.DateOfBirth), request.DateOfBirth);

            if (update.Count == 0) return new Response(StatusCodes.Status200OK, "Success", "");

            // save
            var updateResult = await agencyRepo.Update(update, agency.Id);

            if (updateResult.ModifiedCount == 0)
                return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", "Update Agency fail");

            return new Response(StatusCodes.Status200OK, "Success", "");
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi hệ thống", ex.Message);
        }
    }
}