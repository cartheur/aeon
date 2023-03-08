//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The sr element is a shortcut for: 
    /// 
    /// <srai><star/></srai> 
    /// 
    /// The atomic sr does not have any content. 
    /// </summary>
    public class Sr : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Sr"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Sr(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "sr")
            {
                XmlNode starNode = GetNode("<star/>");
                Star recursiveStar = new Star(ThisAeon, ThisParticipant, ParticipantQuery, ParticipantRequest, ParticipantResult, starNode);
                string starContent = recursiveStar.Transform();

                XmlNode sraiNode = GetNode("<srai>"+starContent+"</srai>");
                Srai sraiHandler = new Srai(ThisAeon, ThisParticipant, ParticipantQuery, ParticipantRequest, ParticipantResult, sraiNode);
                return sraiHandler.Transform();
            }
            return string.Empty;
        }
    }
}
