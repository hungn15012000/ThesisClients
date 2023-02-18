using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Services
{
    public interface ICustomerService
    {
        Task<Response<List<CustomerReadDto>>> GetCustomers();
        Task<Response<CustomerDetailDto>> GetCustomer(int id);
        Task<Response<int>> CreateCustomer(CustomerCreateDto customerCreateDto);
        Task<Response<int>> EditCustomer(int id, CustomerUpdateDto customerUpdateDto);
        Task<Response<CustomerUpdateDto>> GetForUpdateCustomer(int id);
        Task<Response<int>> Delete(int id);
    }
}