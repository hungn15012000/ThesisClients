using System.Text.RegularExpressions;

namespace Middleware.Server.Blazor.Data
{
    public class MonitorNode
    {
        public string NodeId { get; set; }
        public string BrowserName { get; set; }
        public string Value { get; set; }
        public int SamplingInterval { get; set; }
        public string DeadBand { get; set; }
        public double DeadBandValue { get; set; }
        public DateTime Time { get; set; }
        public DateTime monitorTime { get; set; }

        public MonitorNode(string nodeId, string browserName, string value, int samplingInterval, string deadBand, double deadBandValue, DateTime time)
        {
            NodeId = nodeId;
            BrowserName = browserName;
            Value = value;
            SamplingInterval = samplingInterval;
            DeadBand = deadBand;
            DeadBandValue = deadBandValue;
            Time = time;
        }
        public MonitorNode()
        {

        }
    }
}
