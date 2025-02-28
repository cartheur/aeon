//
// Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact m.e. at: cartheur@pm.me.
//
using System.Text;
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The formal element tells the interpreter to render the contents of the element 
    /// such that the first letter of each word is in uppercase, as defined (if defined) by 
    /// the locale indicated by the specified language (if specified). This is similar to methods 
    /// that are sometimes called "Title Case". 
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode 
    /// standard, then the original string is returned.
    /// </summary>
    public class Formal : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Formal"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Formal(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "formal")
            {
                StringBuilder result = new StringBuilder();
                if (TemplateNode.InnerText.Length > 0)
                {
                    string[] words = TemplateNode.InnerText.ToLower().Split();
                    foreach (string word in words)
                    {
                        string newWord = word.Substring(0, 1);
                        newWord = newWord.ToUpper();
                        if (word.Length > 1)
                        {
                            newWord += word.Substring(1);
                        }
                        result.Append(newWord + " ");
                    }
                }
                return result.ToString().Trim();
            }
            return string.Empty;
        }
    }
}
