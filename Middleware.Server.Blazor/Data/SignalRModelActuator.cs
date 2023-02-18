namespace Middleware.Server.Blazor.Data
{
    public class SignalRModelActuator
    {
        public List<bool> Data { get; set; }
        public string Label { get; set; }

        public SignalRModelActuator()
        {
            Data = new List<bool>();
        }
    }

    public class ModelEdit
    {
        public string NodeId { get; set; }
        public string value { get; set; }
        public ModelEdit(string nodeId, string value)
        {
            NodeId = nodeId;
            this.value = value;
        }
        public ModelEdit()
        {

        }
    }
    public class SignalRModel
    {
        public bool Data { get; set; }
        public string Label { get; set; }
        public string NodeId { get; set; }
        public string Type { get; set; }
        public SignalRModel()
        {
            Data = new bool();
        }
    }
    public class SignalRModelDisplay
    {
        public string NodeId { get; set; }
        private string HideAlert { get; set; } = "d-none";
        private string AlertContent { get; set; } = "This is a primary alert—check it out!";
        public SignalRModelDisplay()
        {
           
        }
    }
}
