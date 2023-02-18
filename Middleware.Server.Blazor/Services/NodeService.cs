using AutoMapper;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json.Linq;

namespace Middleware.Server.Blazor.Services
{
    public class NodeService : BaseHttpService, INodeService
    {
        private readonly IClient client;
        private readonly IMapper mapper;
        public NodeService(IClient client, ILocalStorageService localStorageService, IMapper mapper) : base(client, localStorageService)
        {
            this.client = client;
            this.mapper = mapper;
        }

        public async Task<Response<JObject>> GetNode(string nodeId)
        {
            Response<JObject> response = new Response<JObject> { Success = true };
            try
            {
                await GetBearerToken();
               await client.NodesGETAsync(nodeId);
                var client_body = await client.HttpClient.GetAsync($"/api/OpcUa/data-sets/nodes/{nodeId}");
                var contents = await client_body.Content.ReadAsStringAsync();
                JObject json = JObject.Parse(contents);
                response.Data = json;
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<JObject>(apiException);

            }
            return response;
        }

        public async Task<Response<IActionResult>> PostNodeMonitor(string nodeId)
        {
            Response<IActionResult> response = new Response<IActionResult> { Success = true };
            try
            {
                await GetBearerToken();
                var client_body = await client.HttpClient.GetAsync($"/getvaluescada/nodes/{nodeId}");

            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<IActionResult>(apiException);

            }
            return response;
        }

        public async Task<Response<IActionResult>> PostValue(string nodeId, string body)
        {
            Response<IActionResult> response = new Response<IActionResult> { Success = true };
            try
            {
                await GetBearerToken();
                await client.NodesPOSTAsync(nodeId, body);
            }
            catch (ApiException apiException)
            {
                response = ConvertApiExceptions<IActionResult>(apiException);
            }
            return response;
        }
    }
}
