using System;

namespace rapid85_1753
{
    partial class PowerSupply
    {
        private string SendCommand(String Command, string formattedValue)
        {
            if (!ComPort.IsOpen)
            {
                throw new SupplyNotConnectedExeption("the COM port to the supply is not open");
            }
            try
            {
                ComPort.Write($"{Command}{address:00}{formattedValue}\r");
                var responce = ComPort.ReadLine();
                if (!responce.EndsWith("OK"))
                {
                    throw new Exception("supply didn't respond Correctly");
                }

                return responce.Substring(0, responce.Length - 2); ;
            }
            catch (TimeoutException e)
            {
                throw new TimeoutException("the supply ether did not respond or responed in the wrong format", e);
            }
        }

        private string SendCommand(String Command)
        {
            if (!ComPort.IsOpen)
            {
                throw new SupplyNotConnectedExeption("the COM port to the supply is not open");
            }
            try
            {
                ComPort.Write($"{Command}{address:00}\r");
                var responce = ComPort.ReadLine();
                if (!responce.EndsWith("OK"))
                {
                    throw new Exception("supply didn't respond Correctly");
                }

                return responce.Substring(0, responce.Length - 2); ;
            }
            catch (TimeoutException e)
            {
                throw new TimeoutException("the supply ether did not respond or responed in the wrong format", e);
            }
        }

        [Serializable]
        public class SupplyNotConnectedExeption : Exception
        {
            public SupplyNotConnectedExeption() { }
            public SupplyNotConnectedExeption(string message) : base(message) { }
            public SupplyNotConnectedExeption(string message, Exception inner) : base(message, inner) { }
            protected SupplyNotConnectedExeption(
              System.Runtime.Serialization.SerializationInfo info,
              System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
        }

    }
}
