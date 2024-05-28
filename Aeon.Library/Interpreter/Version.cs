//
// This AGI is the intellectual property of Dr. Christopher A. Tucker and Cartheur Research, B.V. Copyright 2008 - 2024, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact the company at: cartheur.research@pm.me.
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
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Version(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest userRequest, ParticipantResult userResult, XmlNode templateNode)
            : base(aeon, thisParticipant, participantQuery, userRequest, userResult, templateNode)
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
