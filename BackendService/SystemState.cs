using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SystemState
{
    /// <summary>
    /// Static class to store the current state of the system
    /// </summary>
    public static class SystemState
    {
        /// <summary>
        /// Is the system in an error state
        /// </summary>
        private static bool ERROR = false;

        /// <summary>
        /// In which state the system is in
        /// </summary>
        private static State STATE = State.Startup;

        /// <summary>
        /// Get the systems state
        /// </summary>
        /// <returns>System current state</returns>
        public static State GetState()
        {
            return STATE;
        }

        /// <summary>
        /// Check if system is in error state
        /// </summary>
        /// <returns>Is the system in an error state</returns>
        public static bool GetError()
        {
            return ERROR;
        }
    }
}
