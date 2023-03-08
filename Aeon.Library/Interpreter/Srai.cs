//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The srai element instructs the interpreter to pass the result of processing the contents of the srai element to the matching loop, as if the input had been produced by the participant (this includes stepping through the entire input normalization process). The srai element does not have any attributes. It may contain any template elements. 
    /// 
    /// As with all elements, nested forms should be parsed from inside out, so embedded srais are perfectly acceptable. 
    /// </summary>
    public class Srai : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Srai"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Srai(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "srai")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    ParticipantRequest subRequest = new ParticipantRequest(TemplateNode.InnerText, ThisParticipant, ThisAeon);
                    // Make sure time is not added to the request.
                    subRequest.StartedOn = ParticipantRequest.StartedOn;
                    ParticipantResult subQuery = ThisAeon.Chat(subRequest);
                    ParticipantRequest.HasTimedOut = subRequest.HasTimedOut;
                    return subQuery.Output;
                }
            }
            return string.Empty;
        }
    }
}
