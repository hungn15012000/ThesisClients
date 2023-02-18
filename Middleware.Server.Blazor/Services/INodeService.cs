using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json.Linq;

namespace Middleware.Server.Blazor.Services
{
    public interface INodeService
    {
        Task<Response<JObject>> GetNode(string nodeId);
        Task<Response<IActionResult>> PostValue(string nodeId, string body);

        Task<Response<IActionResult>> PostNodeMonitor(string nodeId);
       
    }
}
