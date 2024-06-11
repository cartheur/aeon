//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.IO;
using System.Reflection;
using Cartheur.Animals.Core;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// A static class containing commonly-used (shared) functions.
    /// </summary>
    public static class SharedFunctions
    {
        /// <summary>
        /// The application version information.
        /// </summary>
        public static string ApplicationVersion = Assembly.GetExecutingAssembly().GetName().Version.ToString();
        /// <summary>
        /// The path to the debug folder.
        /// </summary>
        public static string PathDebugFolder = Environment.CurrentDirectory + @"";
        /// <summary>
        /// The path to the release folder.
        /// </summary>
        public static string PathReleaseFolder = Path.Combine(Environment.CurrentDirectory, "animals",  @"");
        /// <summary>
        /// Gets or sets the aeon.
        /// </summary>
        public static Aeon ThisAeon { get; set; }
        /// <summary>
        /// Loads the dictionaries.
        /// </summary>
        /// <param name="thisAeon">The this aeon.</param>
        /// <param name="configuration">The active configuration.</param>
        public static bool LoadDictionaries(this Aeon thisAeon, LoaderPaths configuration)
        {
            ThisAeon = thisAeon;
            try
            {
                // Load various default settings and text tools.
                ThisAeon.PersonSubstitutions.LoadSettings(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("personsubstitutionsfile")));
                ThisAeon.DefaultPredicates.LoadSettings(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("defaultpredicates")));
                ThisAeon.Substitutions.LoadSettings(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("substitutionsfile")));
                ThisAeon.LoadSplitters(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("splittersfile")));
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                return false;
            }
            return true;

        }
        /// <summary>
        /// Loads the personality by aeon's name.
        /// </summary>
        /// <param name="thisAeon">The this aeon.</param>
        /// <param name="configuration">The active configuration.</param>
        /// <returns></returns>
        public static bool LoadPersonality(this Aeon thisAeon, LoaderPaths configuration)
        {
            ThisAeon = thisAeon;
            try
            {
                var loader = new AeonLoader(ThisAeon);
                ThisAeon.IsAcceptingUserInput = false;
                // Load in the proper order.
                if (ThisAeon.Name.ToLower() == "aeon")
                {
                    loader.LoadAeon(configuration.PathToAeonAssist);
                }
                if (ThisAeon.Name.ToLower() == "henry" || ThisAeon.Name.ToLower() == "fred")
                {
                    loader.LoadAeon(configuration.PathToReductions);
                    loader.LoadAeon(configuration.PathToMindpixel);
                    loader.LoadAeon(configuration.PathToToyPersonality);
                    loader.LoadAeon(configuration.PathToUpdate);
                    loader.LoadAeon(configuration.PathToFragments);
                }
                if (ThisAeon.Name.ToLower() == "rhodo")
                {
                    loader.LoadAeon(configuration.PathToReductions);
                    loader.LoadAeon(configuration.PathToMindpixel);
                    loader.LoadAeon(configuration.PathToDefaultPersonality);
                    loader.LoadAeon(configuration.PathToUpdate);
                    loader.LoadAeon(configuration.PathToFragments);
                }
                if (ThisAeon.Name.ToLower() == "samantha")
                {
                    loader.LoadAeon(configuration.PathToReductions);
                    loader.LoadAeon(configuration.PathToMindpixel);
                    loader.LoadAeon(configuration.PathToFriendlyPersonality);
                    loader.LoadAeon(configuration.PathToUpdate);
                    loader.LoadAeon(configuration.PathToFragments);
                }
                if (ThisAeon.Name.ToLower() == "mitsuku")
                {
                    loader.LoadAeon(configuration.PathToReductions);
                    loader.LoadAeon(configuration.PathToMindpixel);
                    loader.LoadAeon(configuration.PathToPlayPersonality);
                    loader.LoadAeon(configuration.PathToUpdate);
                    loader.LoadAeon(configuration.PathToFragments);
                }
                Logging.WriteLog(@"Personality loaded, baseline personality is set to " + ThisAeon.Name.ToLower(), Logging.LogType.Information, Logging.LogCaller.SharedFunction);
                ThisAeon.IsAcceptingUserInput = true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Loads a blank robot.
        /// </summary>
        /// <param name="thisAeon"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool LoadBlank(this Aeon thisAeon, LoaderPaths configuration)
        {
            ThisAeon = thisAeon;
            try
            {
                var loader = new AeonLoader(ThisAeon);
                ThisAeon.IsAcceptingUserInput = false;
                loader.LoadAeon(configuration.PathToBlankFile);
                Logging.WriteLog(@"Blank robot loaded", Logging.LogType.Information, Logging.LogCaller.SharedFunction);
                ThisAeon.IsAcceptingUserInput = true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                return false;
            }
            return true;
        }
    }
}
