//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The template-side that element indicates that an interpreter should substitute the contents of a previous presence output. 
    /// 
    /// The template-side that has an optional index attribute that may contain either a single integer or a comma-separated pair of integers. The minimum value for either of the integers in the index is "1". The index tells the interpreter which previous presence output should be returned (first dimension), and optionally which "sentence" (see [8.3.2.]) of the previous presence output (second dimension). 
    /// 
    /// The interpreter should raise an error if either of the specified index dimensions is invalid at run-time. 
    /// 
    /// An unspecified index is the equivalent of "1,1". An unspecified second dimension of the index is the equivalent of specifying a "1" for the second dimension. 
    /// 
    /// The template-side that element does not have any content. 
    /// </summary>
    public class That : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="That"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public That(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "that")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
                {
                    return ThisParticipant.GetThat();
                }
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 1)
                {
                    if (TemplateNode.Attributes[0].Name.ToLower() == "index")
                    {
                        if (TemplateNode.Attributes[0].Value.Length > 0)
                        {
                            try
                            {
                                // See if there is a split.
                                string[] dimensions = TemplateNode.Attributes[0].Value.Split(",".ToCharArray());
                                if (dimensions.Length == 2)
                                {
                                    int localResult = Convert.ToInt32(dimensions[0].Trim());
                                    int sentence = Convert.ToInt32(dimensions[1].Trim());
                                    if ((localResult > 0) & (sentence > 0))
                                    {
                                        return ThisParticipant.GetThat(localResult - 1, sentence - 1);
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.That);
                                }
                                else
                                {
                                    int localResult = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                                    if (localResult > 0)
                                    {
                                        return ThisParticipant.GetThat(localResult - 1);
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.That);
                                }
                            }
                            catch
                            {
                                Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + ParticipantRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.That);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
