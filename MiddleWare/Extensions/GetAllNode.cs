using MiddleWare.Models.OptionsModels;
using MiddleWare.OPCUALayer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Net;
using StatusCodes = Opc.Ua.StatusCodes;

namespace MiddleWare.Extensions
{
    public class GetAllNode
    {
        private readonly IUaClientSingleton _uaClient;
        public GetAllNode(IUaClientSingleton UAClients)
        {
            _uaClient = UAClients;
        }
        public async Task<JObject> GetAllNodes(string serverUrl, string decodedNodeId)
        {

            var result = new JObject();
            var sourceNode = await _uaClient.ReadNodeAsync(serverUrl, decodedNodeId);
            result["node_id"] = decodedNodeId;
            result["name"] = sourceNode.DisplayName.Text;
            switch (sourceNode.NodeClass)
            {
                case NodeClass.Method:
                    result["type"] = "method";
                    break;
                case NodeClass.Variable:
                    result["type"] = "variable";
                    var varNode = (VariableNode)sourceNode;
                    var uaValue = await _uaClient.ReadUaValueAsync(serverUrl, varNode);
                    result["value"] = uaValue.Value;
                    result["value-schema"] = JObject.Parse(uaValue.Schema.ToString());
                    result["status"] = uaValue.StatusCode?.ToString() ?? "";
                    result["deadBand"] = await _uaClient.GetDeadBandAsync(serverUrl, varNode);
                    result["minimumSamplingInterval"] = varNode.MinimumSamplingInterval;
                    break;
                case NodeClass.Object:
                    result["type"] = await _uaClient.IsFolderTypeAsync(serverUrl, decodedNodeId) ? "folder" : "object";
                    break;
            }
            var linkedNodes = new JArray();
            var refDescriptions = await _uaClient.BrowseAsync(serverUrl, decodedNodeId);
            foreach (var rd in refDescriptions)
            {
                var refTypeNode = await _uaClient.ReadNodeAsync(serverUrl, rd.ReferenceTypeId);
                var targetNode = new JObject
                {
                    ["node-id"] = rd.PlatformNodeId,
                    ["name"] = rd.DisplayName
                };


                switch (rd.NodeClass)
                {
                    case NodeClass.Variable:
                        {
                            targetNode["type"] = "variable";
                            var workNode = await _uaClient.ReadNodeAsync(serverUrl, rd.PlatformNodeId);
                            var varNode = (VariableNode)workNode;
                            var uaValue = await _uaClient.ReadUaValueAsync(serverUrl, varNode);
                            if (uaValue != null)
                                targetNode["value"] = uaValue.Value;
                            else
                                targetNode["value"] = null;
                            targetNode["value-schema"] = JObject.Parse(uaValue.Schema.ToString());
                            targetNode["status"] = uaValue.StatusCode?.ToString() ?? "";
                            targetNode["deadBand"] = await _uaClient.GetDeadBandAsync(serverUrl, varNode);
                            targetNode["minimumSamplingInterval"] = varNode.MinimumSamplingInterval;
                            targetNode["relationship"] = refTypeNode.DisplayName.Text;
                            break;
                        }
                    case NodeClass.Method:
                        targetNode["Type"] = "method";
                        targetNode["relationship"] = refTypeNode.DisplayName.Text;
                        break;
                    case NodeClass.Object:
                        targetNode["Type"] = await _uaClient.IsFolderTypeAsync(serverUrl, rd.PlatformNodeId)
                            ? "folder"
                            : "object";
                        targetNode["relationship"] = refTypeNode.DisplayName.Text;
                        var nextNode = new JObject();
                        nextNode = await GetAllNodes(serverUrl, rd.PlatformNodeId);
                        if (nextNode.HasValues)
                        {
                            targetNode["children"] = nextNode;
                        }
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }


                linkedNodes.Add(targetNode);
            }
            result["Children"] = linkedNodes;


            return result;
        }

    }
}
