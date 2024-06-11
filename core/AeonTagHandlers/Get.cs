//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Xml;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// The get element tells the interpreter that it should substitute the contents of a predicate, if that predicate has a value defined. If the predicate has no value defined, the interpreter should substitute the empty string "". 
    /// 
    /// The interpreter implementation may optionally provide a mechanism that allows the author to designate default values for certain predicates (see [9.3.]). 
    /// 
    /// The get element must not perform any text formatting or other "normalization" on the predicate contents when returning them. 
    /// 
    /// The get element has a required name attribute that identifies the predicate with a predicate name. 
    /// 
    /// The get element does not have any content.
    /// </summary>
    public class Get : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Get"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Get(Aeon aeon,
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
            if (TemplateNode.Name.ToLower() == "get")
            {
                if (ThisAeon.GlobalSettings.Count > 0)
                {
                    if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                    {
                        if (TemplateNode.Attributes[0].Name.ToLower() == "name")
                        {
                            return ThisUser.Predicates.GrabSetting(TemplateNode.Attributes[0].Value);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
