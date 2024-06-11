//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Diagnostics;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.Control
{
    /// <summary>
    /// Class managing the external GPIO hardware.
    /// </summary>
    public static class Gpio
    {
        /// <summary>
        /// Runs a python script.
        /// </summary>
        /// <returns><c>true</c>, if python script was run, <c>false</c> otherwise.</returns>
        /// <param name="file">The file to run.</param>
        /// <param name="configuration">The runtime configuration.</param>
        public static bool RunPythonScript(string file, LoaderPaths configuration)
        {
            ProcessStartInfo ps = new ProcessStartInfo
            {
                FileName = "python",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = configuration.PathToScripts + @"/" + file
            };
            try
            {
                Process ppy = Process.Start(ps);
                ppy.StandardOutput.ReadToEnd();
                ppy.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                return false;
            }

        }
        /// <summary>
        /// Runs a python script.
        /// </summary>
        /// <returns><c>true</c>, if python script was run, <c>false</c> otherwise.</returns>
        /// <param name="file">The file to run.</param>
        /// <param name="parameter">The parameter to pass.</param>
        /// <param name="configuration">The runtime configuration.</param>
        public static bool RunPythonScript(string file, string parameter, LoaderPaths configuration)
        {
            ProcessStartInfo ps = new ProcessStartInfo
            {
                FileName = "python",
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = configuration.PathToScripts + @"/" + file + " " + parameter
            };
            try
            {
                Process ppy = Process.Start(ps);
                ppy.StandardOutput.ReadToEnd();
                ppy.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                return false;
            }
        }
    }
}
