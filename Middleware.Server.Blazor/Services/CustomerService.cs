using AutoMapper;
using Blazored.LocalStorage;
using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Services
{
    public class CustomerService : BaseHttpService, ICustomerService
    {
        private readonly IClient client;
        private readonly IMapper mapper;
        public CustomerService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            this.client = client;
            this.mapper = mapper;   
        }

        public async Task<Response<int>> CreateCustomer(CustomerCreateDto customerCreateDto)
        {
            Response<int> response = new Response<int> { Success = true};
            try
            {
                await GetBearerToken();
                await client.CustomersPOSTAsync(customerCreateDto);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<int>(apiException);


            }
            return response;
        }

        public async Task<Response<int>> Delete(int id)
        {
            Response<int> response = new Response<int> { Success = true };
            try
            {
                await GetBearerToken();
                await client.CustomersDELETEAsync(id);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<int>(apiException);


            }
            return response;
        }

        public async Task<Response<int>> EditCustomer(int id, CustomerUpdateDto customerUpdateDto)
        {
            Response<int> response = new Response<int>();
            try
            {
                await GetBearerToken();
                await client.CustomersPUTAsync(id, customerUpdateDto);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<int>(apiException);
            }
            return response;
        }

        public async Task<Response<CustomerDetailDto>> GetCustomer(int id)
        {
            Response<CustomerDetailDto> response;
            try
            {
                await GetBearerToken();
                var data = await client.CustomersGETAsync(id);
                response = new Response<CustomerDetailDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<CustomerDetailDto>(apiException);
            }
            return response;
        }

        public async Task<Response<List<CustomerReadDto>>> GetCustomers()
        {
            Response<List<CustomerReadDto>> response;
            try
            {
                await GetBearerToken();
                var data = await client.CustomersAllAsync();
                response = new Response<List<CustomerReadDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<List<CustomerReadDto>>(apiException);
            }
            return response;
        }

        public async Task<Response<CustomerUpdateDto>> GetForUpdateCustomer(int id)
        {
            Response<CustomerUpdateDto> response;
            try
            {
                await GetBearerToken();
                var data = await client.CustomersGETAsync(id);
                response = new Response<CustomerUpdateDto>
                {
                    Data = mapper.Map<CustomerUpdateDto>(data),
                    Success = true
                };
            }
            catch (ApiException apiException)
            {

                response = ConvertApiExceptions<CustomerUpdateDto>(apiException);
            }
            return response;
        }
    }
}
