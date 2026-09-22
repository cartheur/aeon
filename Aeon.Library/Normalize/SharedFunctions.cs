//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Reflection;

namespace Aeon.Library
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
        public static string PathDebugFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory));
        /// <summary>
        /// The path to the release folder.
        /// </summary>
        public static string PathReleaseFolder = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory));
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
                // Load necessary text tools for the conversational elements.
                ThisAeon.PersonSubstitutions.LoadSettings(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("personsubstitutionsfile")));
                ThisAeon.DefaultPredicates.LoadSettings(Path.Combine(configuration.PathToConfigFiles, ThisAeon.GlobalSettings.GrabSetting("predicatesfile")));
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
            int categoriesBeforeLoad = ThisAeon.Size;
            try
            {
                var loader = new AeonLoader(ThisAeon);
                ThisAeon.IsAcceptingParticipantInput = false;
                ThisAeon.IsAcceptingInput = false;
                // Load in the proper order.
                if (ThisAeon.Name.ToLower() == "blank")
                {
                    loader.LoadAeon(configuration.PathToBlankFile);
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
                if (ThisAeon.Size == categoriesBeforeLoad)
                {
                    Logging.WriteLog("No categories were loaded for personality " + ThisAeon.Name + ".", Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                    return false;
                }
                Logging.WriteLog(@"Personality loaded, baseline personality is set to " + ThisAeon.Name.ToLower(), Logging.LogType.Information, Logging.LogCaller.SharedFunction);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                return false;
            }
            finally
            {
                ThisAeon.IsAcceptingParticipantInput = true;
                ThisAeon.IsAcceptingInput = true;
            }
            return true;
        }
        /// <summary>
        /// Loads a blank AGI.
        /// </summary>
        /// <param name="thisAeon"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static bool LoadBlank(this Aeon thisAeon, LoaderPaths configuration)
        {
            ThisAeon = thisAeon;
            int categoriesBeforeLoad = ThisAeon.Size;
            try
            {
                var loader = new AeonLoader(ThisAeon);
                ThisAeon.IsAcceptingParticipantInput = false;
                ThisAeon.IsAcceptingInput = false;
                loader.LoadAeon(configuration.PathToBlankFile);
                if (ThisAeon.Size == categoriesBeforeLoad)
                {
                    Logging.WriteLog("No categories were loaded for the blank personality.", Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                    return false;
                }
                Logging.WriteLog(@"Blank robot loaded", Logging.LogType.Information, Logging.LogCaller.SharedFunction);
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.SharedFunction);
                return false;
            }
            finally
            {
                ThisAeon.IsAcceptingParticipantInput = true;
                ThisAeon.IsAcceptingInput = true;
            }
            return true;
        }
    }
}
