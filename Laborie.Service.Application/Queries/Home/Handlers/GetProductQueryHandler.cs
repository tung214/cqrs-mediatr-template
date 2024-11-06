using Laborie.Service.Application.DTOs.Home;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Constant;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Home.Handlers;
public class GetProductQueryHandler(ILogger<GetProductQueryHandler> logger
    , IConfiguration configuration
    , IMemoryCache memoryCache
    , IMongoRepository<LaborieProduct> productRepository
    , IMongoRepository<LaborieProductVariants> productVariantsRepository
)
: IQueryHandler<GetProductQuery, Response>
{
    public async Task<Response> Handle(GetProductQuery request, CancellationToken cancellationToken)
    {
        try
        {
            if (!memoryCache.TryGetValue(CacheKeys.Products, out ProductDto? productResult))
            {
                logger.LogInformation("Reload home product");

                int cacheExpire = configuration.GetSection("Caching:Product").Get<int>();

                var products = await productRepository.FindAsync(x => !x.IsDelete && x.IsActive);
                var productIds = products.Select(x => x.Id);
                var productVariants = await productVariantsRepository.FindAsync(x => !x.IsDelete && x.IsActive
                    && productIds.Contains(x.Product));

                var salonProduct = products.Where(x => x.IsPackage == true).ToList();
                var personalProduct = products.Where(x => x.IsPackage != true).ToList();

                var salon = salonProduct.Select(x => new ProductItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Images = x.Images != null ? x.Images.Select(a => a.Url).ToList() : [],
                    Versions = productVariants.Where(a => a.Product == x.Id)
                            .Select(a => new ProductItemVersionDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Image = a.Image?.Url,
                                Price = a.Price,
                                Selected = false
                            }).ToList()
                }).ToList();

                var personal = personalProduct.Select(x => new ProductItemDto
                {
                    Id = x.Id,
                    Name = x.Name,
                    Images = x.Images != null ? x.Images.Select(a => a.Url).ToList() : [],
                    Versions = productVariants.Where(a => a.Product == x.Id)
                            .Select(a => new ProductItemVersionDto
                            {
                                Id = a.Id,
                                Name = a.Name,
                                Image = a.Image?.Url,
                                Price = a.Price,
                                Selected = false
                            }).ToList()
                }).ToList();

                var data = new ProductDto
                {
                    Salon = salon,
                    Personal = personal
                };

                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    .SetSlidingExpiration(TimeSpan.FromMinutes(cacheExpire));
                memoryCache.Set(CacheKeys.Products, data, cacheEntryOptions);

                productResult = data;
            }


            return new Response<ProductDto>(StatusCodes.Status200OK, "Success", "") with
            {
                Data = productResult
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi lấy thông tin sản phẩm", ex.Message);
        }
    }
}
