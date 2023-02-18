namespace MiddleWare.ScadaModel
{
    public class Actuator
    {
        public bool Belt { get; set; }
        public bool Pick { get; set; }
        public bool Release { get; set; }
        public bool Xleft { get; set; }
        public bool Xright { get; set; }
        public bool Zdown { get; set; }
        public bool Zup { get; set; }
        public bool PushBack { get; set; }
        public bool PushFront { get; set; }


        public Actuator()
        {

        }

        public Actuator(bool belt, bool pick, bool release, bool xleft, bool xright, bool zdown, bool zup, bool pushback, bool pushfront)
        {
            Belt = belt;
            Pick = pick;
            Release = release;
            Xleft = xleft;
            Xright = xright;
            Zdown = zdown;
            Zup = zup;
            PushBack = pushback;
            PushFront = pushfront;
        }


    }
    public static class ActuatorNodeIds
    {
        public const string Belt = "2-15095";
        public const string Pick = "2-15101";
        public const string Release = "2-15102";
        public const string Xleft = "2-15100";
        public const string Xright = "2-15099";
        public const string Zdown = "2-15097";
        public const string Zup = "2-15098";
        public const string PushBack = "2-15092";
        public const string PushFront = "2-15093";
    }

}
