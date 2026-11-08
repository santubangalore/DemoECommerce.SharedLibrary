using System;
using System.Collections.Generic;
using Serilog;

namespace eCommerce.SharedLibrary.Logs
{
    public class LogException
    {
        public static void LogExceptions(Exception exception) {
            LogToFile(exception.Message);
            LogToConsole(exception.Message);
            LogToDebugger(exception.Message);
        }

        public static void LogToFile(string message)
        {
            Log.Information(message);
            // Code to log the message to a file
        }   

        public static void LogToConsole(string message)
        {
            Log.Warning(message);
        }

        public static void LogToDebugger(string message)
        {
            Log.Debug(message);
        }
    }
}
