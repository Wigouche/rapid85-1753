using System;
using System.IO.Ports;
using System.Linq;

namespace rapid85_1753
{
    public partial class PowerSupply : IDisposable
    {
        private readonly SerialPort ComPort;
        private int address;

         /* 
         * could implement in future revision but not currently needed
             * get timed programs GETP
             * get spesific timed program GETP Num
             * set timed program PROP
             * reecall prgram RUNP num(number of times)
             * stop timed program
         */

        /// <summary>
        /// create a new power supply object from connection information provided
        /// </summary>
        /// <param name="portName">string containing the port name for the power supply port</param>
        /// <param name="Addr">the address set on the power supply</param>
        public PowerSupply(string portName, int Addr)
        {
            ComPort = new SerialPort(portName)
            {
                BaudRate = 19200,
                StopBits = StopBits.One,
                Parity = Parity.None,
                NewLine = "\r\r\n*\r\n",//line ending form ST09PSE002 RS485 converter
                ReadTimeout = 2000,
                WriteTimeout = 2000,

            };
            address = Addr;
        }

        /// <summary>
        /// opens the com port connection to the power supply if not already open.
        /// </summary>
        public void Connect()
        {
            if (!ComPort.IsOpen)
            {
                ComPort.Open();
                //lock front pannel
                SendCommand("SESS");
            }
        }

        /// <summary>
        /// Close COM port for power supply if it is open
        /// </summary>
        public void Disconnect()
        {
            if (ComPort.IsOpen)
            {
                SendCommand("ENDS");
                ComPort.Close();
            }
        }

        /// <summary>
        /// removes powersupply from memory including closing the com port if open
        /// </summary>
        public void Dispose()
        {
            Disconnect();
            ComPort.Dispose();
        }

        /// <summary>
        /// controls the output state on and off
        /// </summary>
        /// <param name="state">required state true for on false for off</param>
        public void Output(bool state)
        {
            if (state)
            {
                SendCommand("SOUT", "0");
            }
            else
            {
                SendCommand("SOUT", "1");
            }
        }

        /// <summary>
        /// controls the control pannel lock status of the supply
        /// </summary>
        /// <param name="state">required state true for locked false for unlocked</param>
        public void ContolLock(bool state)
        {
            if (state)
            {
                SendCommand("SESS");
            }
            else
            {
                SendCommand("ENDS");
            }
        }

        /// <summary>
        /// method for retreiving max setable values of the supply
        /// </summary>
        /// <returns>a SetPoint Object that contains the Voltage and Current as deciman values</returns>
        public SetPoint GetMaxSettings()
        {
            var responce = SendCommand("GMAX");

            return new SetPoint(responce.Trim());
        }

        /// <summary>
        /// method to set supply output voltage
        /// </summary>
        /// <param name="value">the decimal value in volts thats the power supply will change to</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public void SetV(decimal value)
        {
            if (value < 0 || value > 60)
            {
                throw new ArgumentOutOfRangeException($"given value ({value}) is outside the supplies max limits");
            }
            else
            {
                value *= 10M;
                var msg = SendCommand("VOLT", $"{value:000}");
                //debug
                Console.WriteLine(msg);
            }
        }

        /// <summary>
        /// method to set power supply current limit
        /// </summary>
        /// <param name="value">the decimal value in Amps that the limit will be set to</param>
        public void SetI(decimal value)
        {
            if (value < 0 || value > 1.5M)
            {
                throw new ArgumentOutOfRangeException($"given value ({value}) is outside the supplies max limits");
            }
            else
            {
                value *= 100M;
                var msg = SendCommand("CURR", $"{value:000}");
            }
        }

        /// <summary>
        /// method for getting the current setpoint for output voltage and current limit
        /// </summary>
        /// <returns>a SetPoint Object that contains the Voltage and Current as deciman values</returns>
        public SetPoint GetSetpoints()
        {
            var responce = SendCommand("GETS");

            return new SetPoint(responce.Trim());
        }

        /// <summary>
        /// method for getting the contents of the given memory bank
        /// </summary>
        /// <param name="number">specifies the memory bank to read</param>
        /// <returns>a SetPoint Object that contains the Voltage and Current as deciman values</returns>
        public SetPoint GetMemoryBank(int number)
        {
            if (number < 1 || number>9)
            {
                throw new ArgumentOutOfRangeException($"bank {number} does not exist only 1-9");
            }
            var banks = GetMemoryBanks();
            return banks[number - 1];
        }

        /// <summary>
        /// Method for getting the contents of all the memory banks
        /// </summary>
        /// <returns>an array of SetPoint Objects that contains the Voltage and Current as deciman values</returns>
        public SetPoint[] GetMemoryBanks()
        {
            var responce = SendCommand("GETM");

            var seperatedResponce = responce.Split(new char[] { '\r' }, 9);
            SetPoint[] banks = new SetPoint[9];
            if (seperatedResponce.Count() != 9)
            {
                throw new Exception();//todo fix
            }
            for (int i = 0; i < 9; i++)
            {
                banks[i] = new SetPoint(seperatedResponce[i].Trim());
            }
            return banks;
        }

        /// <summary>
        /// sets the given memory bank to the given voltage and current
        /// </summary>
        /// <param name="num">the memory bank number 1-9</param>
        /// <param name="voltage">the decimal value in Volts for the output voltage</param>
        /// <param name="current">the decimal value in amps for the output current limit </param>
        public void SetMemoryBank(int num,decimal voltage,decimal current)
        {
            if(num <1 || num > 9)
            {
                throw new ArgumentOutOfRangeException($"bank {num} does not exist only 1-9 are valid");
            }
            if (voltage < 0M ||voltage>60M)
            {
                throw new ArgumentOutOfRangeException($"given value({ voltage }) is outside the supplies max limits");
            }
            if (current<0M || current > 1.5M)
            {
                throw new ArgumentOutOfRangeException($"given value({ current }) is outside the supplies max limits");
            }
            voltage *= 10M;
            current *= 100M;
            SendCommand("PROM", $"{num}{voltage:000}{current:000}");
        }

        /// <summary>
        /// recall the given memory bank value
        /// </summary>
        /// <param name="num">the memory bank number 1-9</param>
        public void RecallMemoryBank(int num)
        {
            if (num < 1 || num > 9)
            {
                throw new ArgumentOutOfRangeException($"bank {num} does not exist only 1-9 are valid");
            }
            SendCommand("RUNM", num.ToString());
        }

        /// <summary>
        /// Method for getting the current measurments from the Suppies output for voltage and current
        /// </summary>
        /// <returns>a measurment Object that contains the Voltage and Current as deciman values and the status of the Cv or CC mode</returns>
        public Measurment GetMesurments()
        {
            var responce = SendCommand("GETd");
            return new Measurment(responce.Trim());
        }

        /// <summary>
        /// get the current upper voltage limt of the power supply
        /// </summary>
        /// <returns>decimal value of the Voltage setting in Volts</returns>
        public decimal GetUpperVoltageLimit()
        {
            var responce = SendCommand("GOVP");

            if(!decimal.TryParse(responce.Trim(), out decimal voltage))
            {
                throw new ArgumentException("cannot convert resonce to voltage");
            }
            voltage /= 10;
            return voltage;
        }

        /// <summary>
        /// set the upper voltage limit of the power supply to the given value
        /// </summary>
        /// <param name="voltage">decimal value in Volts</param>
        public void SetUpperVoltageLimit(decimal voltage)
        {
            if(voltage<0 || voltage > 61)
            {
                throw new ArgumentOutOfRangeException($"given value({ voltage }) is outside the supplies max limits");
            }
            voltage *= 10;
            SendCommand("SOVP", $"{voltage:000}");
        }
    }
}
