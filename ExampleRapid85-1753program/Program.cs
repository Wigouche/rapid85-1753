using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using rapid85_1753;
using System.IO.Ports;

namespace ExampleRapid85_1753program
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Rapid 85-1753 Programmable power supply Example program");
            System.Console.WriteLine("Select the COM port of the power supply");
            var portName = GetPortName(); //get from my other project
            //todo use result of get ports in supply contructor
            Console.WriteLine("Enter the address of the Power suppply:");
            var address = GetUserInt();
            PowerSupply supply = new PowerSupply(portName, 1);
            supply.Connect();
            
            supply.Output(false);
            System.Console.Write("Enter a desired output voltage: ");
            var voltage =GetUserDec();
            supply.SetV(voltage);
            System.Console.Write("Enter a desired output current Limit: ");
            var current = GetUserDec();
            supply.SetI(current);
            System.Console.WriteLine("Get setpoints from supply.");

            System.Console.WriteLine(supply.GetSetpoints());
            System.Console.WriteLine("press enter to turn on supply output");
            System.Console.ReadLine();
            supply.Output(true);
            System.Console.WriteLine("press return to get measurments from the Power supply");
            System.Console.ReadLine();
            System.Console.WriteLine(supply.GetMesurments());

            Console.WriteLine("Paused to read results, Press return to quit.");
            Console.ReadLine();
            supply.Dispose();
        }

        private static Decimal GetUserDec()
        {
            decimal responce;
            while(!decimal.TryParse(Console.ReadLine(),out responce))
            {
                Console.WriteLine("The entered value is not a decimal value");
            }

            return responce;
        }

        private static int GetUserInt()
        {
            int responce;
            while (!int.TryParse(Console.ReadLine(), out responce))
            {
                Console.WriteLine("The entered value is not a decimal value");
            }
            return responce;
        }

        public static string GetPortName()
        {

            List<String> AvalablePortNames = new List<String>();
            string portName = null;

            Console.WriteLine("Available Ports:");
            foreach (string s in SerialPort.GetPortNames())
            {
                AvalablePortNames.Add(s);
            }
            for (int i = 0; i < AvalablePortNames.Count; i++)
            {
                Console.WriteLine("{0}. {1}", i, AvalablePortNames[i]);
            }

            Console.WriteLine("Enter COM port index value from list(Default: 0. {0}): ", AvalablePortNames[0]);
            while (portName == null)
            {
                string portToSelectInput = Console.ReadLine();

                if (portToSelectInput == "")
                {
                    portName = AvalablePortNames[0];
                }
                else
                {

                    if (int.TryParse(portToSelectInput, out int portToSelect))
                    {
                        if (portToSelect < AvalablePortNames.Count && portToSelect >= 0)
                        {
                            portName = AvalablePortNames[portToSelect];
                        }
                        else
                        {
                            Console.WriteLine("{0} is not a valid index from the list", portToSelect);
                        }
                    }
                    else
                    {
                        Console.WriteLine("{0} is not a valid index from the list", portToSelectInput);
                    }
                }
            }
            return portName;
        }
    }

}
