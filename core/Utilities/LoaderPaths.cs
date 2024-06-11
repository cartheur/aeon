//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.IO;

namespace Cartheur.Animals.Utilities
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
        /// Gets the path to the nucode file area.
        /// </summary>
        public string PathToNucode
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("nucodedirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to xms file storage.
        /// </summary>
        public string PathToXms
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("xmsdirectory"));
                return new Uri(path).LocalPath;
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
        /// Gets the path to the play personality.
        /// </summary>
        public string PathToPlayPersonality
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("personalitydirectoryplay"));
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
        /// Gets the path to the toy personality.
        /// </summary>
        public string PathToToyPersonality
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("personalitydirectorytoy"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to the aeon-assist personality.
        /// </summary>
        public string PathToAeonAssist
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("aeonassistdirectory"));
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
        /// Gets the path to extra compiled libraries.
        /// </summary>
        public string PathToLibraries
        {
            get
            {
                return Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("librariesdirectory"));
            }
        }
        /// <summary>
        /// Gets the path to the build settings.
        /// </summary>
        public string PathToBuildSettings
        {
            get
            {
                return Path.Combine(ActiveRuntime, Path.Combine("config", "Build.xml"));
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
        /// Gets the path to learning map.
        /// </summary>
        public string PathToLearningMap
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("mapdirectory"));
                return new Uri(path).LocalPath;
            }
        }
        /// <summary>
        /// Gets the path to encrypted files.
        /// </summary>
        public string PathToEncryptedFiles
        {
            get
            {
                return Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("encryptedfilesdirectory"));
            }
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
        /// Gets the path to the python or lua script files.
        /// </summary>
        public string PathToScripts
        {
            get
            {
                return Path.Combine(ActiveRuntime, "scripts");
            }
        }
        /// <summary>
        /// Gets the path to the language processing model files.
        /// </summary>
        public string PathToLanguageModel
        {
            get
            {
                return Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("languagemodeldirectory"));
            }
        }
        /// <summary>
        /// Gets the path to tonal language file definitions.
        /// </summary>
        public string PathToTonalRoot
        {
            get
            {
                var path = Path.Combine(ActiveRuntime, SharedFunctions.ThisAeon.GlobalSettings.GrabSetting("tonalrootdirectory"));
                return new Uri(path).LocalPath;
            }
        }
    }
}
