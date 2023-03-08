//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The sentence element tells the interpreter to render the contents of the element such that the first letter of each sentence is in uppercase, as defined (if defined) by the locale indicated by the specified language (if specified). Sentences are interpreted as strings whose last character is the period or full-stop character .. If the string does not contain a ., then the entire string is treated as a sentence. If no character in this string has a different uppercase version, based on the Unicode standard, then the original string is returned. 
    /// </summary>
    public class Sentence : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sentence"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Sentence(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
            : base(aeon, thisParticipant, participantQuery, participantRequest, participantResult, templateNode)
        {
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The resulting processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            if(TemplateNode.Name.ToLower()=="sentence")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    StringBuilder result = new StringBuilder();
                    char[] letters = TemplateNode.InnerText.Trim().ToCharArray();
                    bool doChange = true;
                    for (int i = 0; i < letters.Length; i++)
                    {
                        string letterAsString = Convert.ToString(letters[i]);
                        if (ThisAeon.Splitters.Contains(letterAsString))
                        {
                            doChange = true;
                        }

                        Regex lowercaseLetter = new Regex("[a-zA-Z]");

                        if (lowercaseLetter.IsMatch(letterAsString))
                        {
                            if (doChange)
                            {
                                result.Append(letterAsString.ToUpper(ThisAeon.Locale));
                                doChange = false;
                            }
                            else
                            {
                                result.Append(letterAsString.ToLower(ThisAeon.Locale));
                            }
                        }
                        else
                        {
                            result.Append(letterAsString);
                        }
                    }
                    return result.ToString();
                }
                // Atomic version of the node.
                XmlNode starNode = GetNode("<star/>");
                Star recursiveStar = new Star(ThisAeon, ThisParticipant, ParticipantQuery, ParticipantRequest, ParticipantResult, starNode);
                TemplateNode.InnerText = recursiveStar.Transform();
                if (!string.IsNullOrEmpty(TemplateNode.InnerText))
                {
                    return ProcessChange();
                }
                return string.Empty;
            }

            return string.Empty;
        }
    }
}
