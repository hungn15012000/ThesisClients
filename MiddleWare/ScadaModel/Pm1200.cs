namespace MiddleWare.ScadaModel
{
    public class Pm1200
    {
        public double Ampere { get; set; }
        public string AmpereStatus { get; set; }
        public double Volt { get; set; }
        public string VoltStatus { get; set; }
        public double Power { get; set; }
        public string PowerStatus { get; set; }
        public double Frequency { get; set; }
        public double PowerFactor { get; set; }
        public double Energy { get; set; }
        public bool Status { get; set; }
        public Pm1200()
        {

        }

        public Pm1200(double ampere, string ampereStatus, double volt, string voltStatus, double power, string powerStatus, double frequency, double powerFactor, double energy, bool status)
        {
            Ampere = ampere;
            AmpereStatus = ampereStatus;
            Volt = volt;
            VoltStatus = voltStatus;
            Power = power;
            PowerStatus = powerStatus;
            Frequency = frequency;
            PowerFactor = powerFactor;
            Energy = energy;
            Status = status;
        }


    }
    public static class Pm1200NodeIds
    {
        public const string AmpereId = "2-15044";
        public const string AmpereStatusId = "2-15045";
        public const string VoltId = "2-15046";
        public const string VoltStatusId = "2-15047";
        public const string PowerId = "2-15049";
        public const string PowerStatusId = "2-15050";
        public const string FrequencyId = "2-15048";
        public const string PowerFactorId = "2-15051";
        public const string EnergyId = "2-15043";
        public const string StatusId = "2-15052";
    }
}
