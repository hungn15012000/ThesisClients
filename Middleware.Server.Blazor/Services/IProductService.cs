using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json.Linq;

namespace Middleware.Server.Blazor.Services
{
    public interface IProductService
    {
        Task<Response<List<ProductReadDto>>> GetProducts();
        Task<Response<ProductDetailDto>> GetProduct(int id);
        Task<Response<int>> CreateProduct(ProductCreateDto productCreateDto);
        Task<Response<int>> EditProduct(int id, ProductUpdateDto productUpdateDto);
        Task<Response<ProductUpdateDto>> GetForUpdateProduct(int id);
        Task<Response<int>> Delete(int id);
        Task<Response<JObject>> Download();
    }
}