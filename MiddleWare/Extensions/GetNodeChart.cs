using MiddleWare.OPCUALayer;
using MiddleWare.Models.ModelHub;
using MiddleWare.ScadaModel;
using Opc.Ua;
using System.Net;

namespace MiddleWare.Extensions
{
    public class GetNodeChart
    {
        private readonly IUaClientSingleton _uaClient;
        private string serverUrl;

        public GetNodeChart(IUaClientSingleton uaClient, string serverUrl)
        {
            _uaClient = uaClient;
            this.serverUrl = serverUrl;
        }
<<<<<<< Updated upstream
        public async Task<List<SignalRModel>> GetPm1200s()
=======
        public async Task<List<SignalRModel>> GetModel()
        {
            
            return new List<SignalRModel>()
            {
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Belt) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Belt) , NodeId =  ActuatorNodeIds.Belt, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Pick) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Pick) , NodeId =  ActuatorNodeIds.Pick, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Release) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Release) , NodeId =  ActuatorNodeIds.Release, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Xleft) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Xleft) , NodeId =  ActuatorNodeIds.Xleft, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Xright) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Xright) , NodeId =  ActuatorNodeIds.Xright, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Zdown) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Zdown) , NodeId =  ActuatorNodeIds.Zdown, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.Zup) , Label = await BrowserName(serverUrl, ActuatorNodeIds.Zup) , NodeId =  ActuatorNodeIds.Zup, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.PushBack) , Label = await BrowserName(serverUrl, ActuatorNodeIds.PushBack) , NodeId =  ActuatorNodeIds.PushBack, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ActuatorNodeIds.PushFront) , Label = await BrowserName(serverUrl, ActuatorNodeIds.PushFront) , NodeId =  ActuatorNodeIds.PushFront, Type = "Actuator"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsBeltBegin) , Label = await BrowserName(serverUrl, SensorNodeIds.SsBeltBegin) , NodeId =  SensorNodeIds.SsBeltBegin, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsBeltEnd) , Label = await BrowserName(serverUrl, SensorNodeIds.SsBeltEnd) , NodeId =  SensorNodeIds.SsBeltEnd, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsPusherBack) , Label = await BrowserName(serverUrl, SensorNodeIds.SsPusherBack) , NodeId =  SensorNodeIds.SsPusherBack, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsPusherFront) , Label = await BrowserName(serverUrl, SensorNodeIds.SsPusherFront) , NodeId =  SensorNodeIds.SsPusherFront, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.Ssx0) , Label = await BrowserName(serverUrl, SensorNodeIds.Ssx0) , NodeId =  SensorNodeIds.Ssx0, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.Ssx1) , Label = await BrowserName(serverUrl, SensorNodeIds.Ssx1) , NodeId =  SensorNodeIds.Ssx1, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.Ssx2) , Label = await BrowserName(serverUrl, SensorNodeIds.Ssx2) , NodeId =  SensorNodeIds.Ssx2, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.Ssx3) , Label = await BrowserName(serverUrl, SensorNodeIds.Ssx3) , NodeId =  SensorNodeIds.Ssx3, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsZDown) , Label = await BrowserName(serverUrl, SensorNodeIds.SsZDown) , NodeId =  SensorNodeIds.SsZDown, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, SensorNodeIds.SsZUp) , Label = await BrowserName(serverUrl, SensorNodeIds.SsZUp) , NodeId =  SensorNodeIds.SsZUp, Type = "Sensor"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S0) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S0) , NodeId =  StateParametersNodeIds.S0, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S1) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S1) , NodeId =  StateParametersNodeIds.S1, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S2) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S2) , NodeId =  StateParametersNodeIds.S2, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S3) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S3), NodeId =  StateParametersNodeIds.S3, Type = "State" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S4) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S4) , NodeId =  StateParametersNodeIds.S4, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S5) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S5) , NodeId =  StateParametersNodeIds.S5, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S6) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S6) , NodeId =  StateParametersNodeIds.S6, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S7) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S7) , NodeId =  StateParametersNodeIds.S7, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S8) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S8) , NodeId =  StateParametersNodeIds.S8, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S9) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S9) , NodeId =  StateParametersNodeIds.S9, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S10) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S10) , NodeId =  StateParametersNodeIds.S10, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S11) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S11) , NodeId =  StateParametersNodeIds.S11, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S12) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S12) , NodeId =  StateParametersNodeIds.S12, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S13) , Label = await BrowserName(serverUrl, StateParametersNodeIds.S13) , NodeId =  StateParametersNodeIds.S13, Type = "State"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ButtonNodeIds.StartID) , Label = "StartButton" , NodeId =  ButtonNodeIds.StartID, Type = "Button"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ButtonNodeIds.StopID) , Label = "StopButton" , NodeId =  ButtonNodeIds.StopID, Type = "Button"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ButtonNodeIds.AutoID) , Label = "AutoButton" , NodeId =  ButtonNodeIds.AutoID, Type = "Button"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ButtonNodeIds.ManualID) , Label = "ManualButton" , NodeId =  ButtonNodeIds.ManualID, Type = "Button"},
                new SignalRModel{Data = await GetValueBoolean(serverUrl, ButtonNodeIds.ResetID) , Label = "ResetButton" , NodeId =  ButtonNodeIds.ResetID, Type = "Button"},

            };
        }
        public async Task<List<SignalRModel>> GetSensorsModel()
>>>>>>> Stashed changes
        {
            return new List<SignalRModel>()
            {
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.AmpereId) }, Label = "Ampere" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.VoltId) }, Label = "Volt" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.PowerId) }, Label = "Power" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.EnergyId) }, Label = "Energy" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.PowerFactorId) }, Label = "PowerFactor" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.FrequencyId) }, Label = "Frequency" }
            };
        }
        public async Task<List<SignalRModel>> GetPm2100s()
        {
            return new List<SignalRModel>()
            {
<<<<<<< Updated upstream
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.AmpereId) }, Label = "Ampere" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.VoltId) }, Label = "Volt" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.PowerId) }, Label = "Power" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.EnergyId) }, Label = "Energy" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.PowerFactorId) }, Label = "PowerFactor" },
                new SignalRModel{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.FrequencyId) }, Label = "Frequency" }
=======
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S0) , Label = "S0" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S1) , Label = "S1" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S2) , Label = "S2" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S3) , Label = "S3" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S4) , Label = "S4" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S5) , Label = "S5" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S6) , Label = "S6" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S7) , Label = "S7" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S8) , Label = "S8" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S9) , Label = "S9" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S10) , Label = "S10" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S11) , Label = "S11" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S12) , Label = "S12" },
                new SignalRModel{Data = await GetValueBoolean(serverUrl, StateParametersNodeIds.S13) , Label = "S13" }
>>>>>>> Stashed changes
            };
        }
        public async Task<List<SignalRModelForLine>> GetCurrentCompare()
        {
            return new List<SignalRModelForLine>()
            {
                new SignalRModelForLine{Data = new List<double> { await GetValue(serverUrl, Pm1200NodeIds.AmpereId) }, Label = "AmperePM1200", Time = DateTime.UtcNow.ToString() },
                new SignalRModelForLine{Data = new List<double> { await GetValue(serverUrl, Pm2100NodeIds.AmpereId) }, Label = "AmperePM2200", Time = DateTime.UtcNow.ToString() }
            };

        }
        public async Task<CardModel> GetCardValue()
        {
            return new CardModel()
            {
                PM1200Current = await GetValue(serverUrl, Pm1200NodeIds.AmpereId),
                PM1200Voltage = await GetValue(serverUrl, Pm1200NodeIds.VoltId),
                PM2100Current = await GetValue(serverUrl, Pm2100NodeIds.AmpereId),
                PM2100Voltage = await GetValue(serverUrl, Pm2100NodeIds.VoltId),
            };

        }
        public async Task<double> GetValue(string serverUrl, string nodeId)
        {
            var decodedNodeId = WebUtility.UrlDecode(nodeId);
            var sourceNode = await _uaClient.ReadNodeAsync(serverUrl, decodedNodeId);
            var varNode = (VariableNode)sourceNode;
            var uaValue = await _uaClient.ReadUaValueAsync(serverUrl, varNode);
            return double.Parse((string)uaValue.Value);
        }
    }
}
