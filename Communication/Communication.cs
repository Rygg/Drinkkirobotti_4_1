using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;

namespace Communication
{
    interface iComm
    {
        bool send(string command);
    }

    public class SerialComm : iComm
    {
        private SerialPort _serialPort;
        public SerialComm(string portname)
        {
            _serialPort = new SerialPort();
            _serialPort.PortName = portname;
            _serialPort.BaudRate = 9600;
            _serialPort.Parity = 0;
            _serialPort.DataBits = 0;
            _serialPort.StopBits = 0;
            _serialPort.Handshake =0;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 500;
            _serialPort.WriteTimeout = 500;

            try
            {
                _serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Console.Write("Access to serialport denied or already in use");
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                Console.Write("Serialport arguments are invalid");
                throw;
            }
            catch (ArgumentException)
            {
                Console.Write("Portname not starting with COM or port type unsupported");
                throw;
            }
            catch (InvalidOperationException)
            {
                Console.Write("Serialport already open");
            }
            catch (System.IO.IOException)
            {
                Console.Write("Serialport at invalid state");
                throw;
            }
        }
        public bool send(string command)
        {
            if (command == null || command.Length > 30)
            {
                Console.Write("Command null or longer than 30");
                throw new ArgumentException("Command null or longer than 30");
            }
            // Expected results
            string startAck = command + "STARTED";
            string doneAck = command + "DONE";
            // Write command
            try
            {
                _serialPort.WriteLine(command);
            }
            // TODO: reaction to errors (retry etc.)
            catch (TimeoutException)
            {
                Console.Write("Writing to serialport resulted a timeout");
                return false;
            }
            catch (InvalidOperationException)
            {
                Console.Write("Tried to write to a closed serialport");
                return false;
            }
            
            // Read response and compare to expected
            string response = _serialPort.ReadTo("!");
            _serialPort.DiscardInBuffer();
            if (response.Equals(startAck))
            {
                // Read second response
                response = _serialPort.ReadTo("!");
                _serialPort.DiscardInBuffer();
                // If action was complited, return true
                if (response.Equals(doneAck))
                {
                    return true;
                }else
                {
                    return false;
                }
        
            }else
            {
                return false;
            }
        }
    }
}
