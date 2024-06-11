//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Reflection;
using System.IO;
using Cartheur.Animals.Core;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// Contains the build details for the application.
    /// </summary>
    public static class Build
    {
        /// <summary>
        /// The path to the executing assembly's debug folder.
        /// </summary>
        public static string DebugFolder = SharedFunctions.PathDebugFolder;
        /// <summary>
        /// The path to the executing assembly's release folder.
        /// </summary>
        public static string ReleaseFolder = SharedFunctions.PathReleaseFolder;
        /// <summary>
        /// A dictionary object that looks after all the settings associated with this aeon.
        /// </summary>
        public static SettingsDictionary BuildSettings;

        private static long compileTime = System.DateTime.UtcNow.Ticks;
        /// <summary>
        /// Returns the compilation date-time.
        /// </summary>
        public static long CompileTime { get => compileTime; set => compileTime = value; }
        /// <summary>
        /// Gets the linker time.
        /// </summary>
        /// <returns>The linker time.</returns>
        /// <param name="assembly">Assembly.</param>
        /// <param name="target">Target.</param>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
        /// <summary>
        /// Gets the path to the build settings.
        /// </summary>
        public static string PathToBuildSettings
        {
            get
            {
                return Path.Combine(DebugFolder, Path.Combine("config", "Build.xml"));
            }
        }

        /// <summary>
        /// Loads the build settings for aeon's environment.
        /// </summary>
        /// <param name="settingsPath">Path to the configuration file.</param>
        public static void LoadSettings(string settingsPath)
        {
            BuildSettings = new SettingsDictionary();
            BuildSettings.LoadSettings(settingsPath);
        }

    }
}
