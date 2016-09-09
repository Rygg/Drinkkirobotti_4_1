using System;
using System.Diagnostics;
using System.ServiceProcess;
using Logger;


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
