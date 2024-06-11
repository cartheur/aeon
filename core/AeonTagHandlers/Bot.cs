//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Xml;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// An element called bot, which may be considered a restricted version of get, is used to tell the interpreter that it should substitute the contents of a "bot predicate". The value of a bot predicate is set at load-time, and cannot be changed at run-time. The interpreter may decide how to set the values of bot predicate at load-time. If the bot predicate has no value defined, the interpreter should substitute an empty string. The bot element has a required name attribute that identifies the bot predicate. 
    /// 
    /// The bot element does not have any content. 
    /// </summary>
    public class Bot : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Bot"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Bot(Aeon aeon,
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
            if (TemplateNode.Name.ToLower() == "bot")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                {
                    if (TemplateNode.Attributes[0].Name.ToLower() == "name")
                    {
                        string key = TemplateNode.Attributes["name"].Value;
                        return ThisAeon.GlobalSettings.GrabSetting(key);
                    }
                }
            }
            return string.Empty;
        }
    }
}
