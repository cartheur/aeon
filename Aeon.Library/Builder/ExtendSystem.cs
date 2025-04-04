//
// Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact m.e. at: cartheur@pm.me.
//
using System.Xml.Linq;

namespace Aeon.Library.Builder
{
    public class ExtendSystem
    {
        private readonly LoaderPaths _runtimeDirectory;

        public ExtendSystem(LoaderPaths runtimeDirectory)
        {
            _runtimeDirectory = runtimeDirectory;
        }
        // The templateInput field has a variety of possibilities, given the context of the language.
        public string CreateAeonFile(string patternInput, string templateInput, int instance)
        {
            string personalityDirectory = Path.Combine(_runtimeDirectory.PathToLearnedFiles);
            if (!Directory.Exists(personalityDirectory))
            {
                Directory.CreateDirectory(personalityDirectory);
            }

            string fileName = Path.Combine(personalityDirectory, + instance + "-.aeon");
            XDocument xmlDocument = new XDocument(
                new XElement("aeon", new XAttribute("version", 1.1),
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
                )));
            xmlDocument.Save(fileName);
            return fileName;
        }
    }
}
