namespace Middleware.Server.Blazor.Services.SignalR
{
    public interface IActuatorHttpRepository
    {
        Task CallChartEndpoint();
        Task CallNodeEndpoint();
    }
}
