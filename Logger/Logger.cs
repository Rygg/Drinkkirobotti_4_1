using System;
using System.Globalization;
using System.IO;

namespace Logger
{
    /// <summary>
    /// A static class generated for logging.
    /// </summary>
    public static class Log
    {
        /// <summary>
        /// Logging level.
        /// </summary>
        private static LogLevel _logLevel = LogLevel.Debug; // Todo:
        /// <summary>
        /// Private DateTime-object to contain the date.
        /// </summary>
        private static DateTime _date = DateTime.Today;
        /// <summary>
        /// Private string containing the logFile name.
        /// </summary>
        private static string _logFileName = "DrinkrobotService-log"; // default value.
        /// <summary>
        /// Private string containing the logging folder.
        /// </summary>
        private static string _logDirectory = @".\logs"; // default value.
        /// <summary>
        /// Private boolean containing the value if the log folder already exists.
        /// </summary>
        private static bool _logInit = false;
        /// <summary>
        /// Writes a log message to the initialized logfile.
        /// </summary>
        /// <param name="functionName">Name of the function being executed</param>
        /// <param name="logMessage">Message to write</param>
        /// <param name="severity">The seriousness of the log message</param>
        public static void WriteLine(string functionName, string logMessage, MessageLevel severity)
        {
            if (!_logInit) // See if log folder exists.
            {
                if (Directory.Exists(_logDirectory)) _logInit = true;
                else
                {
                    Directory.CreateDirectory(_logDirectory); // TODO: Catch possible exception?
                    _logInit = true;
                }
            }
            string messageToWrite = null; // The message to write.
            if (severity == MessageLevel.Debug && _logLevel == LogLevel.Release) return; // Check if debug.
            var msgTime = DateTime.Now;
            CheckForDayChange(msgTime); // Check for day change.
            messageToWrite += msgTime.ToString("HH:mm:ss.ffff",CultureInfo.InvariantCulture)+":"; // Write the timestamp.
            switch (severity)
            {
                case MessageLevel.Error:
                    messageToWrite += "CRITICAL ERROR:";
                    break;
                case MessageLevel.Warning:
                    messageToWrite += "Warning:";
                    break;
                case MessageLevel.Debug:
                    messageToWrite += "DEBUG:";
                    break;
                case MessageLevel.Information:
                    messageToWrite += "Info:";
                    break;
                default:
                    return;
            }
            messageToWrite += functionName + ":" + logMessage + Environment.NewLine; // Write function name and log message.
            File.AppendAllText(FullLogFile(), messageToWrite); // Write the generated string into the log file.
            System.Diagnostics.Debug.WriteLine(messageToWrite);
        }

        /// <summary>
        /// Changes the logging file name.
        /// </summary>
        /// <param name="logFileName">The new logfile name</param>
        public static void ChangeLogFile(string logFileName = "DrinkrobotService-log")
        {
            _logFileName = logFileName;
        }
        /// <summary>
        /// Changes the log file folder.
        /// </summary>
        /// <param name="folderName">Name of the new folder.</param>
        public static void ChangeLoggingFolder(string folderName = "logs")
        {
            _logDirectory = @".\" + folderName;
        }

        public static void ChangeLogLevel(LogLevel lvl)
        {
            _logLevel = lvl;
        }

        #region privateFunctions
        /// <summary>
        /// Returns the full path of the file to write the log to.
        /// </summary>
        /// <returns></returns>
        private static string FullLogFile()
        {
            return Path.Combine(_logDirectory, _logFileName + "-" + _date.ToShortDateString() + ".log");
        }
        /// <summary>
        /// Checks for dateChange.
        /// </summary>
        private static void CheckForDayChange(DateTime writeTime)
        {
            if (_date.Day != writeTime.Day) return;  // Same day, return.
            _date = DateTime.Today; // Change the date.
        }
        #endregion
    }
}
