using MiddleWare.Models.OptionsModels;
using MiddleWare.OPCUALayer;
using Microsoft.Extensions.Options;
using Newtonsoft.Json.Linq;
using Opc.Ua;
using System.Net;
using StatusCodes = Opc.Ua.StatusCodes;
using MiddleWare.Models.DataServerModel;


namespace MiddleWare.Extensions
{
    public class ConvertJsonObjectToModel
    {
        private readonly IUaClientSingleton _uaClient;
        public ConvertJsonObjectToModel(IUaClientSingleton UAClients)
        {
            this._uaClient = UAClients;
        }
        public async Task<TreeOpcUa<ModelOfNode>> ConvertModel(string serverUrl, string decodedNodeId)
        {
            var sourceNode = await _uaClient.ReadNodeAsync(serverUrl, decodedNodeId);
            TreeOpcUa<ModelOfNode> treeOpcUa = new TreeOpcUa<ModelOfNode>();
            treeOpcUa.Root = new TreeNode<ModelOfNode>()
            {
                Data = new ModelOfNode()
                {
                    NodeId = decodedNodeId,
                    Name = sourceNode.DisplayName.Text.ToString(),
                    Type = (sourceNode.NodeClass == NodeClass.Object) ? (await _uaClient.IsFolderTypeAsync(serverUrl, decodedNodeId) ? "folder" : "object") : "variable"
                },
                Parent = null
            };
            if (treeOpcUa.Root.Data.Type == "variable")
            {
                var sourceNodeVariable = (VariableNode)sourceNode;
                var sourceNodeUaValue = await _uaClient.ReadUaValueAsync(serverUrl, sourceNodeVariable);
                treeOpcUa.Root.Data = new ModelOfNode()
                {
                    NodeId = decodedNodeId,
                    Name = sourceNode.DisplayName.Text.ToString(),
                    Type = "variable",
                    Value = (sourceNodeUaValue != null) ? sourceNodeUaValue.Value.ToString() : null,
                    ValueSchemaType = sourceNodeUaValue.Schema.ToString(),
                    Status = sourceNodeUaValue.StatusCode?.ToString() ?? "",
                    DeadBand = await _uaClient.GetDeadBandAsync(serverUrl, sourceNodeVariable),
                    MinimumSamplingInterval = sourceNodeVariable.MinimumSamplingInterval,
                    Relationship = sourceNode.DisplayName.Text
                };
            }
            var refDescriptions = await _uaClient.BrowseAsync(serverUrl, decodedNodeId);
            if (refDescriptions.Count() != 0) treeOpcUa.Root.Data.Relationship = "HasComponent";
            else treeOpcUa.Root.Data.Relationship = "NoComponent";
            treeOpcUa.Root.ChildrenNode = new List<TreeNode<ModelOfNode>>();
            int temp = 0;
            treeOpcUa.Root.ChildrenNode = await LoopTask(serverUrl, decodedNodeId, treeOpcUa.Root);
            int a = 10;

            return treeOpcUa;
        }
        public async Task<List<TreeNode<ModelOfNode>>> LoopTask(string serverUrl, string NodeId, TreeNode<ModelOfNode> parentNode)
        {

            var refDescriptions = await _uaClient.BrowseAsync(serverUrl, NodeId);
            int temp = 0;
            if (refDescriptions.Count() != 0)
            {
                foreach (var rd in refDescriptions)
                {
                    var treeNode = new TreeNode<ModelOfNode>();
                    var refTypeNode = await _uaClient.ReadNodeAsync(serverUrl, rd.ReferenceTypeId);
                    switch (rd.NodeClass)
                    {
                        case NodeClass.Variable:
                            {
                                var workNode = await _uaClient.ReadNodeAsync(serverUrl, rd.PlatformNodeId);
                                var varNode = (VariableNode)workNode;
                                var uaValue = await _uaClient.ReadUaValueAsync(serverUrl, varNode);
                                treeNode.Parent = parentNode;
                                treeNode.Data = new ModelOfNode()//This is where the bug appear
                                {
                                    NodeId = rd.PlatformNodeId,
                                    Type = "variable",
                                    Name = rd.DisplayName.ToString(),
                                    Value = (uaValue != null) ? uaValue.Value.ToString() : null,
                                    ValueSchemaType = uaValue.Schema.ToString(),
                                    Status = uaValue.StatusCode?.ToString() ?? "",
                                    DeadBand = await _uaClient.GetDeadBandAsync(serverUrl, varNode),
                                    MinimumSamplingInterval = varNode.MinimumSamplingInterval,
                                    Relationship = refTypeNode.DisplayName.Text
                                };
                                parentNode.ChildrenNode.Add(treeNode);
                                break;
                            }
                        case NodeClass.Method:
                            temp++;
                            break;
                        case NodeClass.Object:
                            treeNode.Parent = parentNode;
                            treeNode.Data = new ModelOfNode()
                            {
                                NodeId = rd.PlatformNodeId,
                                Name = rd.DisplayName.ToString(),
                                Type = await _uaClient.IsFolderTypeAsync(serverUrl, rd.PlatformNodeId)
                                ? "folder"
                                : "object"
                            };
                            parentNode.ChildrenNode.Add(treeNode);
                            parentNode.ChildrenNode[temp].ChildrenNode = new List<TreeNode<ModelOfNode>>();
                            parentNode.ChildrenNode[temp].ChildrenNode = await LoopTask(serverUrl, rd.PlatformNodeId, parentNode.ChildrenNode[temp]);//This is where the bug appear
                            temp++;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
            return parentNode.ChildrenNode;
        }
    }
}

