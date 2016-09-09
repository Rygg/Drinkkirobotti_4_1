using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace BackendService
{
    public class Service : ServiceBase
    {
        /// <summary>
        /// Executed when the service is started. TODO: Initializes the drink robots back-end.
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            
        }
        /// <summary>
        /// Executed when the service is stopped. TODO: Closing operations.
        /// </summary>
        protected override void OnStop()
        {
            
        }
        /// <summary>
        /// Executed when the service is paused. Pauses the robots program cyclecycle.
        /// </summary>
        protected override void OnPause()
        {
            //TODO
        }
        /// <summary>
        /// Executed when the service is continued. Resumes the robots program cycle.
        /// </summary>
        protected override void OnContinue()
        {
            //TODO
        }
    }
}
