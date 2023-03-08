//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// Ranslatestay Englishway ordsway intoway Igpay Atinlay
    /// </summary>
    /// <remarks>Translates English words into Pig Latin</remarks>
    [CustomTag]
    public class Piglatin : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Piglatin"/> class.
        /// </summary>
        public Piglatin()
        {
            InputString = "piglatin";
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The resulting processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "piglatin")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    StringBuilder result = new StringBuilder();
                    string[] words = TemplateNode.InnerText.ToLower().Split(" ".ToCharArray());

                    foreach (string word in words)
                    {
                        char[] letters = word.ToCharArray();

                        const string consonantEnd = "ay";
                        const string vowelEnd = "way";
                        string[] doubleConsonants = { "ph", "th", "ch", "pn", "sh", "st", "sp" };
                        string[] punctuation = { "\"", ".", "!", ";", "?", ")" };
                        Regex vowels = new Regex("[aeiou]", RegexOptions.IgnoreCase);
                        Regex validChars = new Regex("[a-z]", RegexOptions.IgnoreCase);
                        int locationOfFirstLetter = 0;
                        bool isVowelEnding = false;
                        string firstChar = "";
                        foreach (char character in letters)
                        {
                            if (vowels.IsMatch(character.ToString(CultureInfo.InvariantCulture)))
                            {
                                isVowelEnding = true;
                                firstChar = character.ToString(CultureInfo.InvariantCulture);
                                break;
                            }
                            if (validChars.IsMatch(character.ToString(CultureInfo.InvariantCulture)))
                            {
                                isVowelEnding = false;
                                string firstCharPair = word.Substring(locationOfFirstLetter, 2);
                                foreach (string doubleCheck in doubleConsonants)
                                {
                                    if (firstCharPair == doubleCheck)
                                    {
                                        firstChar = firstCharPair;
                                    }
                                }
                                if (firstChar.Length == 0)
                                {
                                    firstChar = character.ToString(CultureInfo.InvariantCulture);
                                }
                                break;
                            }
                            locationOfFirstLetter++;
                        }
                        // stitch together
                        if (locationOfFirstLetter > 0)
                        {
                            // start the word with any non-character chars (e.g. open brackets)
                            result.Append(word.Substring(0, locationOfFirstLetter));
                        }
                        int newStart;
                        if (isVowelEnding)
                        {
                            newStart = locationOfFirstLetter;
                        }
                        else
                        {
                            newStart = locationOfFirstLetter + firstChar.Length;
                        }
                        string tail;
                        if (isVowelEnding)
                        {
                            tail = vowelEnd;
                        }
                        else
                        {
                            tail = consonantEnd;
                        }

                        for (int i = newStart; i < letters.Length; i++)
                        {
                            string letter = letters[i].ToString(CultureInfo.InvariantCulture);
                            bool isCharacter = true;
                            foreach (string puntuationEnd in punctuation)
                            {
                                if (letter == puntuationEnd)
                                {
                                    tail += letter;
                                    isCharacter = false;
                                }
                            }

                            if (isCharacter)
                            {
                                result.Append(letter);
                            }
                        }
                        if (!isVowelEnding)
                        {
                            result.Append(firstChar);
                        }
                        result.Append(tail + " ");
                    }
                    XmlNode dummySentence = GetNode("<sentence>" + result.ToString().Trim() + "</sentence>");
                    Sentence sentenceMaker = new Sentence(ThisAeon, ThisParticipant, ParticipantQuery, ParticipantRequest, ParticipantResult, dummySentence);

                    return sentenceMaker.Transform();
                }
            }
            return string.Empty;
        }
    }
}
