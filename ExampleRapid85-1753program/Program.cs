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
            
            PowerSupply supply = new PowerSupply("COM3", 1);
            supply.Connect();

            supply.SetUpperVoltageLimit(61);
            Console.WriteLine( supply.GetUpperVoltageLimit());

            Console.WriteLine("Paused to read results, Press return to quit.");
            Console.ReadLine();
            supply.Dispose();
        }

    }

}
