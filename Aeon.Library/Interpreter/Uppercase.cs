//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The uppercase element tells the interpreter to render the contents of the element in uppercase, as defined (if defined) by the locale indicated by the specified language if specified).
    /// 
    /// If no character in this string has a different uppercase version, based on the Unicode (UTF-8) standard, then the original string is returned. 
    /// </summary>
    public class Uppercase : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Uppercase"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Uppercase(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "uppercase")
            {
                return TemplateNode.InnerText.ToUpper(ThisAeon.Locale);
            }
            return string.Empty;
        }
    }
}
