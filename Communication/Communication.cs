using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Ports;
using Logger;

namespace Communication
{
    public interface iComm
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
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = 20000;
            _serialPort.WriteTimeout = 500;

            try
            {
                _serialPort.Open();
            }
            catch (UnauthorizedAccessException)
            {
                Log.WriteLine("_serialPort.Open()", "Access to serialport denied or already in use", MessageLevel.Error);
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
            try
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

                _serialPort.WriteLine(command);

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
                    }
                    else
                    {
                        Log.WriteLine("_serialPort.ReadTo", "Expected from serial:" + doneAck + ", got: " + response, MessageLevel.Information);
                        return false;
                    }

                }
                else
                {
                    Log.WriteLine("_serialPort.ReadTo", "Expected from serial:" + startAck + ", got: " + response, MessageLevel.Information);
                    return false;
                }
            }
            catch (Exception Err)
            {
                Log.WriteLine("serialPort.send()", "Exception: " + Err.GetType().Name + ", message: " + Err.Message, MessageLevel.Warning);
                throw;
            }

        }
    }
}


    