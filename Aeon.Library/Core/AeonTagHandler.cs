//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The template for all classes that handle the <aeon/> tags found within template nodes of a category.
    /// </summary>
    public abstract class AeonTagHandler : TextTransformer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AeonTagHandler"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        protected AeonTagHandler(Aeon aeon, Participant thisParticipant, SubQuery query, Request participantRequest, Result participantResult, XmlNode templateNode)
            : base(aeon, templateNode.OuterXml)
        {
            ThisParticipant = thisParticipant;
            Query = query;
            ParticipantRequest = participantRequest;
            ParticipantResult = participantResult;
            TemplateNode = templateNode;
            XmlAttributeCollection xmlAttributeCollection = TemplateNode.Attributes;
            if (xmlAttributeCollection != null) xmlAttributeCollection.RemoveNamedItem("xmlns");
        }
        /// <summary>
        /// Default to use when late-binding.
        /// </summary>
        protected AeonTagHandler() { }
        /// <summary>
        /// A flag to denote if inner tags are to be processed recursively before processing this tag.
        /// </summary>
        public bool IsRecursive = true;
        /// <summary>
        /// A representation of the participant making the request.
        /// </summary>
        public Participant ThisParticipant;
        /// <summary>
        /// The query that produced this node containing the wildcard matches.
        /// </summary>
        public SubQuery Query;
        /// <summary>
        /// A representation of the input made by the participant.
        /// </summary>
        public Request ParticipantRequest;
        /// <summary>
        /// A representation of the result to be returned to the participant.
        /// </summary>
        public Result ParticipantResult;
        /// <summary>
        /// The template node to be processed by the class.
        /// </summary>
        public XmlNode TemplateNode;
        /// <summary>
        /// Helper method that turns the passed string into an xml node.
        /// </summary>
        /// <param name="outerXml">The string to xmlize.</param>
        /// <returns>The xml node.</returns>
        public static XmlNode GetNode(string outerXml)
        {
            XmlDocument temp = new XmlDocument();
            temp.LoadXml(outerXml);
            return temp.FirstChild;
        }
    }
}
