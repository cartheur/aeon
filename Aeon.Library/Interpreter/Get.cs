//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The get element tells the interpreter that it should substitute the contents of a predicate, if that predicate has a value defined. If the predicate has no value defined, the interpreter should substitute the empty string "". 
    /// 
    /// The interpreter implementation may optionally provide a mechanism that allows the author to designate default values for certain predicates (see [9.3.1]). 
    /// 
    /// The get element must not perform any text formatting or other "normalization" on the predicate contents when returning them. 
    /// 
    /// The get element has a required name attribute that identifies the predicate with a predicate name. 
    /// 
    /// The get element does not have any content.
    /// </summary>
    public class Get : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Get"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Get(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "get")
            {
                if (ThisAeon.GlobalSettings.Count > 0)
                {
                    if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                    {
                        if (TemplateNode.Attributes[0].Name.ToLower() == "name")
                        {
                            return ThisParticipant.Predicates.GrabSetting(TemplateNode.Attributes[0].Value);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
