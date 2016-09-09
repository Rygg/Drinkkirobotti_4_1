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
        public Service()
        {
            
        }
        /// <summary>
        /// Executed when the service is started. TODO: Initializes the drink robots back-end.
        /// </summary>
        /// <param name="args">arguments</param>
        protected override void OnStart(string[] args)
        {
            // Check for db existance?
        }
        /// <summary>
        /// Executed when the service is stopped. TODO: Closing operations.
        /// </summary>
        protected override void OnStop()
        {
            
        }
        /// <summary>
        /// Executed when the service is paused. TODO: Pauses the robots program cyclecycle.
        /// </summary>
        protected override void OnPause()
        {
            //TODO
        }
        /// <summary>
        /// Executed when the service is continued. TODO: Resumes the robots program cycle.
        /// </summary>
        protected override void OnContinue()
        {
            //TODO
        }
    }
}
