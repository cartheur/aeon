//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.IO;
using Boagaphish.Core.Variables;

namespace Boagaphish
{
    /// <summary>
    /// The class which performs logging for the library. Originated in MACOS 9.0.4 under CodeWarrior.
    /// </summary>
    public static class Logging
    {
        /// <summary>
        /// The file path for executing assemblies.
        /// </summary>
        public static string FilePath = Environment.CurrentDirectory;
        /// <summary>
        /// The type of log to write.
        /// </summary>
        [Flags]
        public enum LogType { Information, Error, Statistics, Warning };
        /// <summary>
        /// The classes within the interpreter calling the log.
        /// </summary>
        public enum LogCaller { Accounts, AgentCore, AgentGui, AgentVoice, AlgorithmicComputation, Automat, DeepLearning, Genome, Orders, OrderInformation, Positions, Rates, TestFramework, Trades, Transactions, TrendGui }
        /// <summary>
        /// The last message passed to logging.
        /// </summary>
        public static string LastMessage = "";
        /// <summary>
        /// The delegate for returning the last log message to the calling application.
        /// </summary>
        public delegate void LoggingDelegate();

        /// <summary>
        /// Occurs when [returned to console] is called.
        /// </summary>
        public static event LoggingDelegate ReturnedToConsole;
        /// <summary>
        /// Records an event relevant to the algorithm.
        /// </summary>
        /// <param name="dataPoint">The data point itself.</param>
        /// <param name="eventData">The event data.</param>
        /// <param name="intervalPeriod">The interval period.</param>
        /// <param name="intervalNomen">The interval nomen, e.g., s, m, h.</param>
        public static void RecordEvent(DataPoints.AccountDataPoint dataPoint, double eventData, int intervalPeriod, string intervalNomen)
        {
            var stream = new StreamWriter(FilePath + @"\logs\monitor.txt", true);
            switch (dataPoint)
            {
                case DataPoints.AccountDataPoint.Balance:
                    stream.WriteLine(DateTime.Now + " - Balance datapoint value is: [" + eventData + "] on an interval of " + intervalPeriod + " " + intervalNomen + ".");
                    break;
                case DataPoints.AccountDataPoint.MarginUsed:
                    stream.WriteLine(DateTime.Now + " - Margin used datapoint value is: [" + eventData + "] on an interval of " + intervalPeriod + " " + intervalNomen + ".");
                    break;
                case DataPoints.AccountDataPoint.Unrealized:
                    stream.WriteLine(DateTime.Now + " - Unrealized datapoint value is: [" + eventData + "] on an interval of " + intervalPeriod + " " + intervalNomen + ".");
                    break;
            }
            stream.Close();
        }
        /// <summary>
        /// Logs a message sent from the calling application to a file.
        /// </summary>
        /// <param name="message">The message to log. Space between the message and log type enumeration provided.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="caller">The class creating the log entry.</param>
        public static void WriteLog(string message, LogType logType, LogCaller caller)
        {
            LastMessage = message;
            StreamWriter stream = new StreamWriter(FilePath + @"\logs\logfile.txt", true);
            switch (logType)
            {
                case LogType.Error:
                    stream.WriteLine(DateTime.Now + " - " + " ERROR " + " - " + message + " from " + caller + ".");
                    break;
                case LogType.Warning:
                    stream.WriteLine(DateTime.Now + " - " + " WARNING " + " - " + message + " from " + caller + ".");
                    break;
                case LogType.Information:
                    stream.WriteLine(DateTime.Now + " - " + " INFO " + message);
                    break;
                case LogType.Statistics:
                    stream.WriteLine(DateTime.Now + " - " + message + ".");
                    break;
            }
            stream.Close();
            if (!Equals(null, ReturnedToConsole))
            {
                ReturnedToConsole();
            }
        }
        /// <summary>
        /// Writes the log.
        /// </summary>
        /// <param name="message">The message. Space between the message and log type enumeration provided.</param>
        /// <param name="logType">Type of the log.</param>
        /// <param name="caller">The class creating the log entry.</param>
        /// <param name="method">The method creating the log entry.</param>
        public static void WriteLog(string message, LogType logType, LogCaller caller, string method)
        {
            LastMessage = message;
            StreamWriter stream = new StreamWriter(FilePath + @"\logs\logfile.txt", true);
            switch (logType)
            {
                case LogType.Error:
                    stream.WriteLine(DateTime.Now + " - " + " ERROR " + " - " + message + " from class " + caller + " using method " + method + ".");
                    break;
                case LogType.Warning:
                    stream.WriteLine(DateTime.Now + " - " + " WARNING " + " - " + message + " from class " + caller + " using method " + method + ".");
                    break;
                case LogType.Information:
                    stream.WriteLine(DateTime.Now + " - " + message + " called from the class " + caller + " using method " + method + ".");
                    break;
                case LogType.Statistics:
                    stream.WriteLine(DateTime.Now + " - " + message + ".");
                    break;
            }
            stream.Close();
            if (!Equals(null, ReturnedToConsole))
            {
                ReturnedToConsole();
            }
        }
        /// <summary>
        /// Writes a debug log with object parameters.
        /// </summary>
        /// <param name="objects">The objects.</param>
        public static void Debug(params object[] objects)
        {
            StreamWriter stream = new StreamWriter(FilePath + @"\logs\logfile.txt", true);
            foreach (object obj in objects)
            {
                stream.WriteLine(obj);
            }
            stream.WriteLine("--");
            stream.Close();
        }

        public static class ActiveTime
        {
            public static string LogTime(double? time, bool read)
            {
                string path = FilePath + "\\logs\\timelogfile.txt";
                switch (read)
                {
                    case true:
                        return File.ReadAllText(path);
                    case false:
                        {
                            StreamWriter streamWriter = new StreamWriter(path, false);
                            streamWriter.WriteLine(time);
                            streamWriter.Close();
                            streamWriter.Dispose();
                            return "";
                        }
                }
            }

            public static string LogTime(double? time, string timeLogFilePath, bool read)
            {
                switch (read)
                {
                    case true:
                        return File.ReadAllText(timeLogFilePath);
                    case false:
                        {
                            StreamWriter streamWriter = new StreamWriter(timeLogFilePath, false);
                            streamWriter.WriteLine(time);
                            streamWriter.Close();
                            streamWriter.Dispose();
                            return "";
                        }
                }
            }
        }
    }
}
