//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The gossip element instructs the interpreter to capture the result of processing the contents of the gossip elements and to store these contents in a manner left up to the implementation. The most-common use of gossip has been to store such contents in a file, but this needs to be stopped. 
    /// 
    /// The gossip element does not have any attributes. It may contain any template elements.
    /// </summary>
    public class Gossip : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gossip"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Gossip(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "gossip")
            {
                // Gossip is merely logged by aeon and written to the log file. Todo: A more intuitive implementation.
                if (TemplateNode.InnerText.Length > 0)
                {
                    Logging.WriteLog("Gossip from the participant: " + ThisParticipant.Name + ", '" + TemplateNode.InnerText + "'",
                        Logging.LogType.Gossip, Logging.LogCaller.Gossip);
                }
            }
            return string.Empty;
        }
    }
}
