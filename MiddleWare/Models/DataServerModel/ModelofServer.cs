namespace MiddleWare.Models.DataServerModel
{
    public class TreeNode<T>
    {
        public int Id { get; set; }
        public ModelOfNode? Data { get; set; }
        public List<TreeNode<T>>? ChildrenNode { get; set; }
        public TreeNode<T>? Parent { get; set; }
        public int GetHeight()
        {
            int Height = 1;
            TreeNode<T> node = this;
            while (node != null)
            {
                Height++;
                node = node.Parent;
            }
            return Height;
        }
    }
    public class TreeOpcUa<T>
    {
        public int Id { get; set; }
        public TreeNode<T>? Root { get; set; }
    }
    public class ModelOfNode
    {
        public int Id { get; set; }
        public string? NodeId { get; set; }
        public string? Name { get; set; }
        public string? Type { get; set; }
        public string? Relationship { get; set; }
        public string? Value { get; set; }

        public string? ValueSchemaType { get; set; }
        public string? Status { get; set; }
        public string? DeadBand { get; set; }
        public double? MinimumSamplingInterval { get; set; }
        public ModelOfNode() { }
        public ModelOfNode(string nodeId, string name, string type, string relationship, string value)
        {
            NodeId = nodeId;
            Name = name;
            Type = type;
            Relationship = relationship;
            Value = value;
        }
    }
}

