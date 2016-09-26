using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Logger;

namespace Communication
{
    interface iComm
    {
        
        /// <summary>
        /// Send a command to the device
        /// </summary>
        /// <param name="command">String to be sent to the device, cannot be null.</param>
        /// <returns>If the device responded accordingly</returns>
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
                Log.WriteLine("_serialPort.Open()","Access to serialport denied or already in use", MessageLevel.Error);
                throw;
            }
            catch (ArgumentOutOfRangeException)
            {
                Log.WriteLine("_serialPort.Open()", "Serialport arguments are invalid", MessageLevel.Error);
                throw;
            }
            catch (ArgumentException)
            {
                Log.WriteLine("_serialPort.Open()", "Portname not starting with COM or port type unsupportedd", MessageLevel.Error);
                throw;
            }
            catch (InvalidOperationException)
            {
                Log.WriteLine("_serialPort.Open()", "Serialport already open", MessageLevel.Warning);
            }
            catch (System.IO.IOException)
            {
                Log.WriteLine("_serialPort.Open()", "Serialport at invalid state", MessageLevel.Error);
                throw;
            }
        }
        public bool send(string command)
        {
            if (command == null || command.Length > 50)
            {
                Log.WriteLine("SerialComm.send()", "Command null or longer than 50", MessageLevel.Error);
                throw new ArgumentException("Command null or longer than 50");
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
                Log.WriteLine("_serialPort.Write()", "Writing to serialport resulted a timeout", MessageLevel.Warning);
                return false;
            }
            catch (InvalidOperationException)
            {
                Log.WriteLine("_serialPort.Write()", "Tried to write to a closed serialport", MessageLevel.Error);
                throw;
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
                    Log.WriteLine("_serialPort.ReadTo", "Expected from serial:" + doneAck + ", got: " + response, MessageLevel.Information);
                    return false;
                }
        
            }else
            {
                Log.WriteLine("_serialPort.ReadTo", "Expected from serial:" + startAck + ", got: " + response, MessageLevel.Information);
                return false;
            }
        }
    }
}
