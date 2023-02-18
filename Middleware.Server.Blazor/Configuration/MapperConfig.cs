using AutoMapper;
using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Configuration
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<CustomerDetailDto, CustomerUpdateDto>().ReverseMap();
            CreateMap<ProductDetailDto, ProductUpdateDto>().ReverseMap();
        }
    }
}
