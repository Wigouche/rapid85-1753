using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using rapid85_1753;

namespace ExampleRapid85_1753program
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Console.WriteLine("Rapid 85-1753 Programmable power supply Example program");
            System.Console.WriteLine("Select the COM port of the power supply");
            //getPortName(); //get from my other project
            //todo use result of get ports in supply contructor
            var address = GetUserInt();
            PowerSupply supply = new PowerSupply("COM3", 1);
            supply.Connect();
            
            supply.output(false);
            System.Console.WriteLine("Enter a desired output voltage: ");
            var voltage = GetUserDec();
            supply.SetV(voltage);
            System.Console.WriteLine("Enter a desired output current Limit: ");
            var current = GetUserDec();
            System.Console.WriteLine("Get setpoints from supply.");

            System.Console.WriteLine(supply.GetSetpoints());
            System.Console.WriteLine("press enter to turn on supply output");
            System.Console.ReadLine();
            supply.output(true);
            System.Console.WriteLine("press return to get measurments from the Power supply");
            System.Console.ReadLine();
            System.Console.WriteLine(supply.GetMeasurments());

            Console.WriteLine("Paused to read results, Press return to quit.");
            Console.ReadLine();
            supply.Dispose();
        }

    }

}
