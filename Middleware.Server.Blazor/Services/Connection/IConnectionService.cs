using Microsoft.AspNetCore.Mvc;
using Middleware.Server.Blazor.Data;
using Middleware.Server.Blazor.Services.Base;
using Newtonsoft.Json.Linq;

namespace Middleware.Server.Blazor.Services.Connection
{
    public interface IConnectionService
    {
        Task<Response<List<OpcUaConnectionUrl>>> GetDataSet();
        Task<Response<object>> GetEndpoints(string body);
    }
}
