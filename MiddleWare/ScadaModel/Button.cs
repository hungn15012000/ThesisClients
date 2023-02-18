namespace MiddleWare.ScadaModel
{
    public class Button
    {
        public bool Start { get; set; }
        public bool Stop { get; set; }
        public bool Auto_Manual { get; set; }
        public bool Reset { get; set; }

        public Button()
        {

        }
        public Button(bool start, bool stop, bool auto_manual, bool reset)
        {
            Start = start;
            Stop = stop;
            Auto_Manual = auto_manual;
            Reset = reset;
        }
    }
    public static class ButtonNodeIds
    {
<<<<<<< Updated upstream
        public const string StartID = "2-15095";
        public const string StopID = "2-15097";
        public const string AutoManualID = "2-15101";
        public const string ResetID = "2-15099";
=======
        public const string StartID = "2-15080";
        public const string StopID = "2-15082";
        public const string AutoID = "2-15086";
        public const string ResetID = "2-15084";
        public const string ManualID = "2-15088";

>>>>>>> Stashed changes
    }
}
