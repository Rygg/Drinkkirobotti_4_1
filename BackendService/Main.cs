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
            if (serial.send("TEST"))
            {
                Debug.Write("Serial communication test passed\n");
            }else
            {
                Debug.Write("Serial communication test failed!\n");
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
