//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The topicstar element tells the interpreter that it should substitute the contents of a wildcard from the current topic (if the topic contains any wildcards).
    /// 
    /// The topicstar element has an optional integer index attribute that indicates which wildcard to use; the minimum acceptable value for the index is "1" (the first wildcard). Not specifying the index is the same as specifying an index of "1". 
    /// 
    /// The topicstar element does not have any content. 
    /// </summary>
    public class TopicStar : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TopicStar"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public TopicStar(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
            : base(aeon, thisParticipant, participantQuery, participantRequest, participantResult, templateNode)
        {
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "topicstar")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
                {
                    if (ParticipantQuery.TopicStar.Count > 0)
                    {
                        return ParticipantQuery.TopicStar[0];
                    }
                    Logging.WriteLog("An out-of-bounds index to topicstar was encountered when processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.TopicStar);
                }
                else if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                {
                    if (TemplateNode.Attributes[0].Name.ToLower() == "index")
                    {
                        if (TemplateNode.Attributes[0].Value.Length > 0)
                        {
                            try
                            {
                                int result = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                                if (ParticipantQuery.TopicStar.Count > 0)
                                {
                                    if (result > 0)
                                    {
                                        return ParticipantQuery.TopicStar[result - 1];
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.TopicStar);
                                }
                                else
                                {
                                    Logging.WriteLog("An out-of-bounds index to topicstar was encountered when processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.TopicStar);
                                }
                            }
                            catch
                            {
                                Logging.WriteLog("A thatstar tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.TopicStar);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
