//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The set element instructs the interpreter to set the value of a predicate to the result of processing the contents of the set element. The set element has a required attribute name, which must be a valid predicate name. If the predicate has not yet been defined, the interpreter should define it in memory. 
    /// 
    /// The interpreter should, generically, return the result of processing the contents of the set element. The set element must not perform any text formatting or other "normalization" on the predicate contents when returning them. 
    /// 
    /// The interpreter implementation may optionally provide a mechanism that allows the author to designate certain predicates as "return-name-when-set", which means that a set operation using such a predicate will return the name of the predicate, rather than its captured value. (See [9.2].) 
    /// 
    /// A set element may contain any template elements.
    /// </summary>
    public class Set : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Set"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Set(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "set")
            {
                if (ThisAeon.GlobalSettings.Count > 0)
                {
                    if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                    {
                        if (TemplateNode.Attributes[0].Name.ToLower() == "name")
                        {
                            if (TemplateNode.InnerText.Length > 0)
                            {
                                ThisParticipant.Predicates.AddSetting(TemplateNode.Attributes[0].Value, TemplateNode.InnerText);
                                return ThisParticipant.Predicates.GrabSetting(TemplateNode.Attributes[0].Value);
                            }
                            // Remove the predicate.
                            ThisParticipant.Predicates.RemoveSetting(TemplateNode.Attributes[0].Value);
                            return string.Empty;
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
