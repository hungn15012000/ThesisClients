namespace MiddleWare.ScadaModel
{
    public class Sensor
    {
        public bool Ssx0 { get; set; }
        public bool Ssx1 { get; set; }
        public bool Ssx2 { get; set; }
        public bool Ssx3 { get; set; }
        public bool SsBeltBegin { get; set; }
        public bool SsBeltEnd { get; set; }
        public bool SsPusherBack { get; set; }
        public bool SsPusherFront { get; set; }
        public bool SsZDown { get; set; }
        public bool SsZUp { get; set; }

        public Sensor()
        {

        }

        public Sensor(bool ssx0, bool ssx1, bool ssx2, bool ssx3, bool ssBeltBegin, bool ssBeltEnd, bool ssPusherBack, bool ssPusherFront, bool ssZDown, bool ssZUp)
        {
            Ssx0 = ssx0;
            Ssx1 = ssx1;
            Ssx2 = ssx2;
            Ssx3 = ssx3;
            SsBeltBegin = ssBeltBegin;
            SsBeltEnd = ssBeltEnd;
            SsPusherBack = ssPusherBack;
            SsPusherFront = ssPusherFront;
            SsZDown = ssZDown;
            SsZUp = ssZUp;
        }


    }
    public static class SensorNodeIds
    {
        public const string Ssx0 = "2-15108";
        public const string Ssx1 = "2-15109";
        public const string Ssx2 = "2-15110";
        public const string Ssx3 = "2-15111";
        public const string SsBeltBegin = "2-15104";
        public const string SsBeltEnd = "2-15105";
        public const string SsPusherBack = "2-15113";
        public const string SsPusherFront = "2-15112";
        public const string SsZDown = "2-15107";
        public const string SsZUp = "2-15106";
    }
}
