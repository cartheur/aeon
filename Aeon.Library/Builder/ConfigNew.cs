using System;
using System.IO;
using System.Xml.Linq;

namespace Aeon.Library.Builder
{
    public class ConfigNew
    {
        private readonly string _runtimeDirectory;

        public ConfigNew(string runtimeDirectory)
        {
            _runtimeDirectory = runtimeDirectory;
        }

        public void CreateAeonFile(string input)
        {
            string personalityDirectory = Path.Combine(_runtimeDirectory, "personality");
            if (!Directory.Exists(personalityDirectory))
            {
                Directory.CreateDirectory(personalityDirectory);
            }

            string fileName = Path.Combine(personalityDirectory, "config.aeon");
            XDocument xmlDocument = new XDocument(
                new XElement("category",
                    new XElement("pattern", input),
                    new XElement("template", "I'm sorry, I don't understand that.")
                )
            );

            xmlDocument.Save(fileName);
        }
    }
}