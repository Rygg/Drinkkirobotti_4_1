using System;
using System.ServiceProcess;
using Logger;
using Communication;
using System.Diagnostics;

namespace BackendService
{
    internal class Backend
    {
        /// <summary>
        /// Launching point of the program:
        /// </summary>
        /// <param name="args">Command line parameters</param>
        private static void Main(string[] args)
        {
            const string function = nameof(Main);

#if DEBUG // Debug launch:
            Log.WriteLine(function, "Starting back-end in debugging mode", MessageLevel.Debug);
            // todo: Debug launch here.
            iComm serial = new SerialComm("COM10");
            if (serial.send("A"))
            {
                Debug.Write("Testi OK");
            }else
            {
                Debug.Write("U fuged up");
            }
            Environment.Exit(0);
#else
            Log.WriteLine(function, "Starting back-end service", MessageLevel.Debug);
            // Create the service base.
            var services = new ServiceBase[]
            {
                new Service()
            };
            ServiceBase.Run(services); // Run.
#endif
            Log.WriteLine(function, "Closing", MessageLevel.Information);
        }
    }
}
