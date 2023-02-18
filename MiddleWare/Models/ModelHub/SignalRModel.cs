namespace MiddleWare.Models.ModelHub
{
    public class SignalRModel
    {
        public List<double> Data { get; set; }
        public string Label { get; set; }
        public SignalRModel()
        {
            Data = new List<double>();
        }

    }
    public class SignalRModelForLine
    {
        public List<double> Data { get; set; }
        public string Label { get; set; }
        public string Time { get; set; }
        public SignalRModelForLine()
        {
            Data = new List<double>();
        }

    }

    public class CardModel
    {
        public double PM1200Current { get; set; }
        public double PM1200Voltage { get; set; }
        public double PM2100Current { get; set; }
        public double PM2100Voltage { get; set; }


    }



}
