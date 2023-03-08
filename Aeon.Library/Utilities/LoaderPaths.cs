//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// Sets the paths for the files given the configuration, either debug or release.
    /// </summary>
    public class LoaderPaths
    {
        /// <summary>
        /// The active configuration for the application runtume.
        /// </summary>
        public string ActiveRuntime;
        /// <summary>
        /// Initializes a new instance of the <see cref="LoaderPaths"/> class with a build configuration.
        /// </summary>
        /// <param name="configuration">The active runtime configuration.</param>
        public LoaderPaths(string configuration)
        {
            if (configuration == "Debug")
                ActiveRuntime = SharedFunctions.PathDebugFolder;
            if (configuration == "Release")
                ActiveRuntime = SharedFunctions.PathReleaseFolder;
        }
        /// <summary>
        /// Gets the path to the blank file.
        /// </summary>
        public string PathToBlankFile
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("blankdirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to configuration files.
        /// </summary>
        public string PathToConfigFiles
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("configdirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to the default personality.
        /// </summary>
        public string PathToDefaultPersonality
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("personalitydirectorydefault"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to extra file fragments.
        /// </summary>
        public string PathToFragments
        {
            get
            {
                return Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("fragmentsdirectory"));
            }
        }
        /// <summary>
        /// Gets the path to the friendly personality.
        /// </summary>
        public string PathToFriendlyPersonality
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("personalitydirectoryfriendly"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to mindpixel.
        /// </summary>
        public string PathToMindpixel
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("mindpixeldirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to reductions.
        /// </summary>
        public string PathToReductions
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("reductionsdirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to update.
        /// </summary>
        public string PathToUpdate
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("updatedirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to the settings.
        /// </summary>
        public string PathToSettings
        {
            get
            {
                return Path.Combine(ActiveRuntime, Path.Combine("config", "Settings.xml"));
            }
        }

    }
}
