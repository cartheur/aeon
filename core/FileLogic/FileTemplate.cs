//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.IO;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.FileLogic
{
    /// <summary>
    /// A static class for writing new files to the aeon's working memory.
    /// </summary>
    public static class FileTemplate
    {
        private const string NuFilenameBlank = "blank.aeon";
        /// <summary>
        /// Gets or sets the pattern text.
        /// </summary>
        public static string PatternText { get; set; }
        //private static string _patternFragment = "<pattern>" + PatternText + "</pattern>";
        /// <summary>
        /// Gets or sets the template text.
        /// </summary>
        public static string TemplateText { get; set; }
        //private static string _templateFragment = "<template>" + TemplateText + "</template>";
        /// <summary>
        /// The XMS file template blank
        /// </summary>
        public static string FileTemplateBlank = "";
        /// <summary>
        /// Creates the XMS template.
        /// </summary>
        /// <returns></returns>
        public static string CreateFileTemplate()
        {
            return FileTemplateBlank = "<?xml version=\"1.0\" encoding=\"UTF-8\"?><aeon version = \"" + SharedFunctions.ApplicationVersion + "\"><category><pattern>" + PatternText + "</pattern><template>" + TemplateText + "</template></category></aeon>";
        }
        /// <summary>
        /// Writes a nufile to data storage.
        /// </summary>
        /// <param name="filename">The name of the nufile. If empty, then will use the xms filename field.</param>
        /// <param name="group">The thematic group of the addition. (Experimental)</param>
        /// <returns>True if successful.</returns>
        public static bool WriteNuFile(string filename, string group)
        {
            if (filename == "")
                filename = NuFilenameBlank;
            try
            {
                File.WriteAllText(Environment.CurrentDirectory + @"\nucode\" + filename, FileTemplateBlank, System.Text.Encoding.UTF8);
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
