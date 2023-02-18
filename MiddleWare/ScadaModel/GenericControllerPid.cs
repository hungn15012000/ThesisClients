namespace MiddleWare.ScadaModel
{
    public class GenericControllerPid
    {
        public double Kp { get; set; }
        public double Ki { get; set; }
        public double Kd { get; set; }
        public double Setpoint { get; set; }


        public GenericControllerPid()
        {

        }
        public GenericControllerPid(double kp, double ki, double kd, double setpoint)
        {
            Kp = kp;
            Ki = ki;
            Kd = kd;
            Setpoint = setpoint;
        }

    }
    public static class GenericControllerPidNodeIds
    {
        public const string PID_Setpoint = "2-15103";
        public const string PID_Kp = "2-15104";
        public const string PID_Ki = "2-15105";
        public const string PID_Kd = "2-15106";
    }
}
