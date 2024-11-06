using Laborie.Service.Application.DTOs.Home;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Constant;
using Laborie.Service.Shared.Models;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Laborie.Service.Application.Queries.Home.Handlers;

/// <summary>
/// 
/// </summary>
/// <typeparam name="GetBannerQueryHandler"></typeparam>
public class GetBannerQueryHandler(ILogger<GetBannerQueryHandler> logger
    , IConfiguration configuration
    , IMemoryCache memoryCache
    , IMongoRepository<LaborieBanner> bannerRepository)
: IQueryHandler<GetBannerQuery, Response>
{
    public async Task<Response> Handle(GetBannerQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!memoryCache.TryGetValue(CacheKeys.Banners, out List<BannerDto>? bannerResult))
            {
                logger.LogInformation("Reload all banner");

                int bannerCache = configuration.GetSection("Caching:Banner").Get<int>();
                var banners = await bannerRepository.FindAsync(x => x.IsDelete == false);
                var dataResult = banners.OrderBy(x => x.Order).Select(x => x.Adapt<BannerDto>()).ToList();

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(bannerCache));
                memoryCache.Set(CacheKeys.Banners, dataResult, cacheEntryOptions);

                bannerResult = dataResult;
            }

            return new Response<List<BannerDto>>(StatusCodes.Status200OK
                , bannerResult != null && bannerResult.Count != 0 ? "Success" : "Success no result", "") with
            {
                Data = bannerResult
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi lấy thông tin banner", ex.Message);
        }
    }
}
