//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The version element tells the interpreter that it should substitute the version number of the interpreter. The version element does not have any content. 
    /// </summary>
    public class Version : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The user making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the user.</param>
        /// <param name="participantResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Version(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "version")
            {
                return ThisAeon.GlobalSettings.GrabSetting("version");
            }
            return string.Empty;
        }
    }
}
