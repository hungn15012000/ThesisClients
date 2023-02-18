using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Data;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Net.Http.Json;

namespace Middleware.Server.Blazor.Services.Connection
{
    public class ConnecitonService: BaseHttpService, IConnectionService
    {
        private readonly IClient client;
        private readonly IMapper mapper;
        public ConnecitonService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<Response<List<OpcUaConnectionUrl>>> GetDataSet()
        {
            Response<List<OpcUaConnectionUrl>> response = new Response<List<OpcUaConnectionUrl>> { Success = true };
            try
            {
                await GetBearerToken();
                await client.DataSetsAsync();

                var contents = await client.HttpClient.GetFromJsonAsync<List<OpcUaConnectionUrl>>("api/OpcUa/data-sets");
                response.Data = contents;
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<List<OpcUaConnectionUrl>>(apiException);

            }
            return response;
        }

        public async Task<Response<object>> GetEndpoints(string body)
        {
            Response<object> response = new Response<object> { Success = true };
            try
            {
                await GetBearerToken();
                StringContent queryString = new StringContent(body);
                //await client.GetEndpointsAsync(body);
                var testReponse = await client.HttpClient.PostAsJsonAsync<string>($"api/OpcUa/get-endpoints", body);
                var testContents = await testReponse.Content.ReadAsStringAsync();
                var json0 = JsonConvert.DeserializeObject(testContents);
                //var client_body = await client.HttpClient.PostAsync($"api/OpcUa/get-endpoints", queryString);
                //var contents = await client_body.Content.ReadAsStringAsync();
                //JObject json1 = JObject.Parse(contents);
                //response.Data = json0;
                response.Data = json0;
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<object>(apiException);

            }
            return response;
        }
    }
}
