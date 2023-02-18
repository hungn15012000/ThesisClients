using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json.Linq;
using System.Collections;

namespace Middleware.Server.Blazor.Services
{
    public class ProductService : BaseHttpService, IProductService
    {
        private readonly IClient client;
        private readonly IMapper mapper;
        public ProductService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<Response<int>> CreateProduct(ProductCreateDto productCreateDto)
        {
            Response<int> response = new Response<int> { Success = true };
            try
            {
                await GetBearerToken();
                await client.ProductsPOSTAsync(productCreateDto);
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
                await client.ProductsDELETEAsync(id);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<int>(apiException);


            }
            return response;
        }

        public async Task<Response<int>> EditProduct(int id, ProductUpdateDto productUpdateDto)
        {
            Response<int> response = new Response<int>();
            try
            {
                await GetBearerToken();
                await client.ProductsPUTAsync(id, productUpdateDto);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<int>(apiException);
            }
            return response;
        }


        public async Task<Response<ProductDetailDto>> GetProduct(int id)
        {
            Response<ProductDetailDto> response;
            try
            {
                await GetBearerToken();
                var data = await client.ProductsGETAsync(id);
                response = new Response<ProductDetailDto>
                {
                    Data = data,
                    Success = true
                };
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<ProductDetailDto>(apiException);
            }
            return response;
        }

        public async Task<Response<List<ProductReadDto>>> GetProducts()
        {
            Response<List<ProductReadDto>> response;
            try
            {
                await GetBearerToken();
                var data = await client.ProductsAllAsync();
                response = new Response<List<ProductReadDto>>
                {
                    Data = data.ToList(),
                    Success = true
                };
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<List<ProductReadDto>>(apiException);
            }
            return response;
        }

        public async Task<Response<ProductUpdateDto>> GetForUpdateProduct(int id)
        {
            Response<ProductUpdateDto> response;
            try
            {
                await GetBearerToken();
                var data = await client.ProductsGETAsync(id);
                response = new Response<ProductUpdateDto>
                {
                    Data = mapper.Map<ProductUpdateDto>(data),
                    Success = true
                };
            }
            catch (ApiException apiException)
            {

                response = ConvertApiExceptions<ProductUpdateDto>(apiException);
            }
            return response;
        }

        public async Task<Response<JObject>> Download()
        {
            Response<JObject> response = new Response<JObject> { Success = true };
            try
            {
                await GetBearerToken();
               var data = await client.HttpClient.GetAsync("https://middlewarethesisoisp.azurewebsites.net/api/Table/productdata/top10000");
               var contents = await data.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(contents);
                
                //response.Data = File(fileContents, "application/force-download", fileDownloadName);
                response.Data = json;
            }
            catch (ApiException apiException)
            {

                response = ConvertApiExceptions<JObject>(apiException);
            }
            return response;
        }
    }
}
