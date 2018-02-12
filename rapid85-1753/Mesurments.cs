using System;

namespace rapid85_1753
{
    public partial class PowerSupply
    {
        public class SetPoint
        {
            private string responce;
            public decimal Voltage { get; private set; }
            public decimal Current { get; private set; }

            internal SetPoint(string responce)
            {
                if (responce.Length != 6)
                {
                    throw new ArgumentException("input string in incorrect length");
                }
                this.responce = responce;
                
                Voltage = decimal.Parse(responce.Substring(0, 3)) / 10;
                Current = decimal.Parse(responce.Substring(3)) / 100;
            }
            public override string ToString()
            {
                return $"{Voltage:#0.0} V, {Current:0.00} A";
            }
        }
           public enum RegMode
        {
            ConstVoltage = '0',
            ConstCurrent = '1'
        }

        public class Measurment
        {
            private string responce;
            public decimal Voltage { get; private set; }
            public decimal Current { get; private set; }
            public RegMode RegulationMode { get; private set; }

            internal Measurment(string responce)
            {
                if (responce.Length != 9)
                {
                    throw new ArgumentException("input string in incorrect length");
                }
                this.responce = responce;


                Voltage = decimal.Parse(responce.Substring(0, 4)) / 100;
                Current = decimal.Parse(responce.Substring(4, 4)) / 1000;
                RegulationMode = (RegMode)responce[8];
            }
            public override string ToString()
            {
                return $"{Voltage:#0.000} V, {Current:0.000} A";
            }
        }
    }
}