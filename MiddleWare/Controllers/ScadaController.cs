using MiddleWare.Data;
using MiddleWare.Extensions;
using MiddleWare.HubConfig;
using MiddleWare.Models.OptionsModels;
using MiddleWare.OPCUALayer;
using MiddleWare.TimerFeature;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Options;
<<<<<<< Updated upstream
=======
using MiddleWare.Models.OPCUA;
using static MiddleWare.Extensions.MonitorScada;
using System.Net;
using Opc.Ua;
using MiddleWare.Exceptions;
using StatusCodes = Opc.Ua.StatusCodes;
>>>>>>> Stashed changes

namespace MiddleWare.Controllers
{
    public class ScadaController : BaseApiController
    {
        private readonly IHubContext<MyHub> hubContext;
        private readonly OPCUAServers[] _uaServers;
        private readonly IUaClientSingleton _uaClient;
        /// <summary>
        /// Constructor an ApiController which controls the servers and client 
        /// </summary>
        /// <param name="servers">options</param>
        /// <param name="UAClient">server </param>

        public ScadaController(IOptions<OPCUAServersOptions> servers, IUaClientSingleton UAClient, IHubContext<MyHub> hubContext)
        {
            this.hubContext = hubContext;
            this._uaServers = servers.Value.Servers;
            for (int i = 0; i < _uaServers.Length; i++) _uaServers[i].Id = i;
            this._uaClient = UAClient;
        }
        [HttpGet("/getvaluescadaPm1200")]
        public async Task<IActionResult> GetChartDataPm1200()
        {
<<<<<<< Updated upstream
            string serverUrl = _uaServers[0].Url;
            var pm1200s = new GetNodeChart(_uaClient, serverUrl);
            var timerMananer = new TimerManager(async () => hubContext.Clients.All.SendAsync("transferchartdataPm1200", await pm1200s.GetPm1200s()));
            return Ok(new { Message = "request completed" });
        }
        [HttpGet("/getvaluescadaPm2100")]
        public async Task<IActionResult> GetChartDataPm2100()
        {
            string serverUrl = _uaServers[0].Url;
            var pm2100s = new GetNodeChart(_uaClient, serverUrl);
            var timerMananer = new TimerManager(async () => hubContext.Clients.AllExcept("transferchartdataPm1200", "transferchartdataDoughnut").SendAsync("transferchartdataPm2100", await pm2100s.GetPm2100s()));
            return Ok(new { Message = "request completed" });
        }
        [HttpGet("/getvaluescadaDoughnut")]
        public async Task<IActionResult> GetChartDataDoughnut()
        {
            string serverUrl = _uaServers[0].Url;
            var doughnutModel = new GetNodeChart(_uaClient, serverUrl);
            var timerMananer = new TimerManager();
            timerMananer.TimerManagerOption1(async () => await hubContext.Clients.AllExcept("transferchartdataPm2100", "transferchartdataPm1200").SendAsync("transferchartdataDoughnut", await doughnutModel.GetCurrentCompare()));
=======
            try
            {
                string serverUrl = _uaServers[0].Url;
                var actuatorModel = new GetNodeChart(_uaClient, serverUrl);
                var timerMananer = new TimerManager(async () => hubContext.Clients.All.SendAsync("TransferDataScada", await actuatorModel.GetModel()));
                
            }
            catch (Exception exc)
            {
                return StatusCode(500 ,exc);
            }
>>>>>>> Stashed changes
            return Ok(new { Message = "request completed" });
            
        }
        [HttpGet("/getcardvalues")]
        public async Task<IActionResult> GetCardResult()
        {
            string serverUrl = _uaServers[0].Url;
            var doughnutModel = new GetNodeChart(_uaClient, serverUrl);
            var timerMananer = new TimerManager();
            timerMananer.TimerManagerOption1(async () => await hubContext.Clients.AllExcept("transferchartdataPm2100", "transferchartdataPm1200").SendAsync("transferCard", await doughnutModel.GetCardValue()));
            return Ok(new { Message = "request completed" });

        }

    }
}
