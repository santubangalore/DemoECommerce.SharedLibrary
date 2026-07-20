using System;
using System.Collections.Generic;
using Serilog;

namespace eCommerce.SharedLibrary.Logs
{
    internal class LogException
    {
        public static void LogExceptions(Exception exception) {
            LogToFile(exception.Message);
            LogToConsole(exception.Message);
            LogToDebugger(exception.Message);
        }

        private static void LogToFile(string message)
        {
            Log.Information(message);
            // Code to log the message to a file
        }   

        private static void LogToConsole(string message)
        {
            Log.Warning(message);
        }

        private static void LogToDebugger(string message)
        {
            Log.Debug(message);
        }
    }
}
