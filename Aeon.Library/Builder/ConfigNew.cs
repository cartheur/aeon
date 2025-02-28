using System;
using System.IO;
using System.Xml.Linq;

namespace Aeon.Library.Builder
{
    public class ConfigNew
    {
        private readonly LoaderPaths _runtimeDirectory;

        public ConfigNew(LoaderPaths runtimeDirectory)
        {
            _runtimeDirectory = runtimeDirectory;
        }
        // The templateInput field has a variety of possibilities, given the context of the language.
        // 
        public async Task<string> CreateAeonFile(string patternInput, string templateInput, int instance)
        {
            string personalityDirectory = Path.Combine(_runtimeDirectory.PathToLearnedFiles);
            if (!Directory.Exists(personalityDirectory))
            {
                Directory.CreateDirectory(personalityDirectory);
            }

            string fileName = Path.Combine(personalityDirectory, + instance + "-config.aeon");
            XDocument xmlDocument = new XDocument(
                new XElement("category",
                    new XElement("pattern", patternInput),
                    // The templateInput field can contain:
                    // - Plain text: "Hello, how can I help you today?"
                    // - <srai>: <srai>HELLO</srai>
                    // - <random>: <random><li>Hello!</li><li>Hi there!</li><li>Greetings!</li></random>
                    // - <condition>: <condition name="userMood"><li value="happy">I'm glad to hear you're happy!</li><li value="sad">I'm sorry to hear you're feeling down.</li><li>How are you feeling today?</li></condition>
                    // - <think>: <think><set name="topic">sports</set></think> Let's talk about sports.
                    // - <set>: <set name="name">Alice</set> Nice to meet you, Alice.
                    // - <get>: Hello, <get name="name"/>!
                    // - <bot>: My name is <bot name="name"/>.
                    // - <star>: You said: <star/>
                    // - <that>: <that index="1,1">Yes</that>
                    // - <date>: The current date and time is <date/>.
                    // - <formal>: <formal>this is formal text.</formal>
                    // - <uppercase>: <uppercase>this is uppercase text.</uppercase>
                    // - <lowercase>: <lowercase>THIS IS LOWERCASE TEXT.</lowercase>
                    new XElement("template", templateInput)
                )
            );

            await Task.Run(() => xmlDocument.Save(fileName));
            return fileName;
        }
    }
}
