//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The atomic version of the gender element is a shortcut for:
    /// 
    /// <gender><star/></gender> 
    ///
    /// The atomic gender does not have any content. Combined with person substitutions.
    /// 
    /// The non-atomic gender element instructs the interpreter to: 
    /// 
    /// 1. Replace male-gendered words in the result of processing the contents of the gender element 
    /// with the grammatically-corresponding female-gendered words; and 
    /// 
    /// 2. Replace female-gendered words in the result of processing the contents of the gender element 
    /// with the grammatically-corresponding male-gendered words. 
    /// 
    /// The definition of "grammatically-corresponding" is left up to the implementation.
    /// </summary>
    public class Gender : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Gender"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Gender(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "gender")
            {
                if (TemplateNode.InnerText.Length > 0)
                {
                    // Non-atomic version of the node.
                    return ApplySubstitutions.Substitute(ThisAeon, ThisAeon.PersonSubstitutions, TemplateNode.InnerText);
                }
                // Atomic version of the node.
                XmlNode starNode = GetNode("<star/>");
                Star recursiveStar = new Star(ThisAeon, ThisParticipant, ParticipantQuery, ParticipantRequest, ParticipantResult, starNode);
                TemplateNode.InnerText = recursiveStar.Transform();
                if (!string.IsNullOrEmpty(TemplateNode.InnerText))
                {
                    return ProcessChange();
                }
                return string.Empty;
            }
            return string.Empty;
        }
    }
}
