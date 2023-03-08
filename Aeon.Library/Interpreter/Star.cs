//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The star element indicates that an interpreter should substitute the value "captured" by a particular wildcard from the pattern-specified portion of the match path when returning the template. 
    /// 
    /// The star element has an optional integer index attribute that indicates which wildcard to use. The minimum acceptable value for the index is "1" (the first wildcard), and the maximum acceptable value is equal to the number of wildcards in the pattern. 
    /// 
    /// An interpreter should raise an error if the index attribute of a star specifies a wildcard that does not exist in the category element's pattern. Not specifying the index is the same as specifying an index of "1". 
    /// 
    /// The star element does not have any content. 
    /// </summary>
    public class Star : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Star"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Star(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "star")
            {
                if (ParticipantQuery.InputStar.Count > 0)
                {
                    if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
                    {
                        // Return the first (latest) star in the List<>.
                        return ParticipantQuery.InputStar[0];
                    }
                    if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                    {
                        if (TemplateNode.Attributes[0].Name.ToLower() == "index")
                        {
                            try
                            {
                                int index = Convert.ToInt32(TemplateNode.Attributes[0].Value);
                                index--;
                                if ((index >= 0) & (index < ParticipantQuery.InputStar.Count))
                                {
                                    return ParticipantQuery.InputStar[index];
                                }
                                Logging.WriteLog("InputStar out of bounds reference caused by input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Star);
                            }
                            catch
                            {
                                Logging.WriteLog("Index set to non-integer value while processing star tag in response to the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Star);
                            }
                        }
                    }
                }
                else
                {
                    Logging.WriteLog("A star tag tried to reference an empty InputStar collection when processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Star);
                }
            }
            return string.Empty;
        }
    }
}
