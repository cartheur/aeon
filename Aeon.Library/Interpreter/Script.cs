//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// Facilitates the execution of a script from within the runtime.
    /// </summary>
    public class Script : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Script" /> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The user making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Script(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest userRequest, ParticipantResult userResult, XmlNode templateNode)
            : base(aeon, thisParticipant, participantQuery, userRequest, userResult, templateNode)
        {
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            Logging.WriteLog("The script tag is not yet implemented. Perhaps in a later version it will.", Logging.LogType.Error, Logging.LogCaller.Script);
            return string.Empty;
        }
    }
}
