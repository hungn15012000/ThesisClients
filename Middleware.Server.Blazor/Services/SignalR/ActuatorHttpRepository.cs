using AutoMapper;
using Blazored.LocalStorage;
using Middleware.Server.Blazor.Services.Base;

namespace Middleware.Server.Blazor.Services.SignalR
{
    public class ActuatorHttpRepository : IActuatorHttpRepository
    {
        private readonly HttpClient _client;

        public ActuatorHttpRepository(HttpClient client)
        {
            _client = client;
        }

        public async Task CallChartEndpoint()
        {
            var result = await _client.GetAsync("/getvaluescada");

            if (!result.IsSuccessStatusCode)
                Console.WriteLine("Something went wrong with the response");
        }

        public async Task CallNodeEndpoint()
        {
            var result = await _client.GetAsync("/monitornodes");

            if (!result.IsSuccessStatusCode)
                Console.WriteLine("Something went wrong with the response");
        }
    }
}
