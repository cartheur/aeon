//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Xml;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// The version element tells the interpreter that it should substitute the version number of the interpreter. The version element does not have any content. 
    /// </summary>
    public class Version : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Version"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Version(Aeon aeon,
                        User thisUser,
                        SubQuery query,
                        Request userRequest,
                        Result userResult,
                        XmlNode templateNode)
            : base(aeon, thisUser, query, userRequest, userResult, templateNode)
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
