using Laborie.Service.Application.DTOs.Home;
using Laborie.Service.Application.Interface;
using Laborie.Service.Domain.Entities.Mongo.Laborie;
using Laborie.Service.Domain.Repositories.Mongo;
using Laborie.Service.Shared.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Laborie.Service.Application.Queries.Home.Handlers;
public class GetProductItemQueryHandler(ILogger<GetProductItemQueryHandler> logger
    , IMongoRepository<LaborieProduct> productRepository
    , IMongoRepository<LaborieProductVariants> productVariantsRepository)
: IQueryHandler<GetProductItemQuery, Response>
{
    public async Task<Response> Handle(GetProductItemQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var product = await productRepository.FindOneAsync(x => !x.IsDelete && x.IsActive && x.Id == request.Id);
            var productVariants = await productVariantsRepository.FindAsync(x => !x.IsDelete && x.IsActive
                    && x.Product == product.Id);
            var data = new ProductItemDto
            {
                Id = product.Id,
                Name = product.Name,
                Images = product.Images != null ? product.Images.Select(a => a.Url).ToList() : [],
                Versions = productVariants.Select(a => new ProductItemVersionDto
                {
                    Id = a.Id,
                    Name = a.Name,
                    Image = a.Image?.Url,
                    Price = a.Price,
                    Selected = a.Id == request.VariantId
                }).ToList()
            };
            return new Response<ProductItemDto>(StatusCodes.Status200OK, "Success", "") with
            {
                Data = data
            };
        }
        catch (System.Exception ex)
        {
            logger.LogError(ex, "{class} error {message}", GetType().Name, ex.Message);
            return new Response(StatusCodes.Status500InternalServerError, "Lỗi lấy thông tin sản phẩm", ex.Message);
        }
    }
}
