//
// This AGI is the intellectual property of Dr. Christopher A. Tucker and Cartheur Research, B.V. Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact the company at: cartheur.research@pm.me.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The lowercase element tells the interpreter to render the contents of the element in lowercase, as defined (if defined) by the locale indicated by the specified language (if specified). 
    /// 
    /// If no character in this string has a different lowercase version, based on the Unicode standard, then the original string is returned. 
    /// </summary>
    public class Lowercase : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Lowercase"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Lowercase(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "lowercase")
            {
                return TemplateNode.InnerText.ToLower(ThisAeon.Locale);
            }
            return string.Empty;
        }
    }
}
