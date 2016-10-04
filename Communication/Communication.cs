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

    /// <summary>
    /// A Dummy class for testing. Returns always true after 2 seconds
    /// </summary>
    public class DummyComm : iComm
    {
        public bool send(string command)
        {
            System.Threading.Thread.Sleep(2000);
            Log.WriteLine("DummyComm.send()", "Dummy interface message: " + command + " delivered", MessageLevel.Debug);
            return true;
        }
    }
    public class SerialComm : iComm
    {
        
        private SerialPort _serialPort;
        private string _endstring;
        /// <summary>
        /// Creates a serialport connection
        /// </summary>
        /// <param name="portname">The name of the port ex. COM3</param>
        /// <param name="endstring">Optional: string which the connected device uses to indicate end of message, default = !</param>
        /// <param name="baudrate">Optional: baudrate to use, default = 9600</param>
        /// <param name="readtimeout">Optional: time to wait for responses, default = 20000</param>
        public SerialComm(string portname, string endstring = "!", int baudrate = 9600, int readtimeout = 20000)
        {
            _endstring = endstring;
            _serialPort = new SerialPort();
            _serialPort.PortName = portname;
            _serialPort.BaudRate = baudrate;
            _serialPort.Parity = Parity.None;
            _serialPort.DataBits = 8;
            _serialPort.StopBits = StopBits.One;
            _serialPort.Handshake = Handshake.None;

            // Set the read/write timeouts
            _serialPort.ReadTimeout = readtimeout;
            _serialPort.WriteTimeout = 500;

            try
            {
                Log.WriteLine("_serialPort.Open()", "Opening serialport " + portname, MessageLevel.Debug);
                _serialPort.Open();
                if (_serialPort.IsOpen)
                {
                    Log.WriteLine("_serialPort.Open()", "Serialport " + portname + " opened", MessageLevel.Debug);
                }
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
                if (String.IsNullOrEmpty(command) || command.Length > 50)
                {
                    Log.WriteLine("SerialComm.send()", "Command null, empty or longer than 50", MessageLevel.Error);
                    throw new ArgumentException("Command null, empty or longer than 50");
                }
                // Expected results
                string startAck = command + "STARTED";
                string doneAck = command + "DONE";
                // Write command

                _serialPort.WriteLine(command);

                // Read response and compare to expected


                string response = _serialPort.ReadTo(_endstring);
                _serialPort.DiscardInBuffer();
                if (response.Equals(startAck))
                {
                    // Read second response
                    response = _serialPort.ReadTo(_endstring);
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
    /// <summary>
    /// Class for communication with digital input/output-pins
    /// </summary>
    public class DigitalIO : iComm
    {
        public bool send(string command)
        {
            throw new NotImplementedException();
        }
    }
}


    