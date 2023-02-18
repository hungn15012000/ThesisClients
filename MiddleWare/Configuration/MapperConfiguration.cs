using AutoMapper;
using MiddleWare.Data;
using MiddleWare.Models.Customer;
using MiddleWare.Models.Product;
using MiddleWare.Models.User;

namespace MiddleWare.Configuration
{
    public class MapperConfiguration : Profile
    {
        public MapperConfiguration()
        {
            CreateMap<CustomerCreateDto, Customer>().ReverseMap();
            CreateMap<CustomerReadDto, Customer>().ReverseMap();
            CreateMap<CustomerUpdateDto, Customer>().ReverseMap();
            CreateMap<Product, ProductReadDto>()
                .ForMember(q => q.CustomerName, d => d.MapFrom(map => $"{map.Customer.Firstname} {map.Customer.Lastname}"))
                .ReverseMap();
            CreateMap<Product, ProductCreateDto>().ReverseMap();
            CreateMap<Product, ProductUpdateDto>().ReverseMap();
            CreateMap<Product, ProductDetailDto>()
                .ForMember(q => q.CustomerName, d => d.MapFrom(map => $"{map.Customer.Firstname} {map.Customer.Lastname}"))
                .ReverseMap();
            CreateMap<ApiUser, UserDto>().ReverseMap();
        }
    }
}
