//
// This autonomous intelligent system software is the property of Cartheur Research, BV. Copyright 2023, all rights reserved.
//
using System.Xml;

namespace Aeon.Library
{
    /// <summary>
    /// The learn element instructs the interpreter to retrieve a resource specified by a URI, and to process its object contents.
    /// </summary>
    public class Learn : AeonHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Learn"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisParticipant">The participant making the request.</param>
        /// <param name="participantQuery">The query that originated this node.</param>
        /// <param name="participantRequest">The request sent by the participant.</param>
        /// <param name="participantResult">The result to be sent back to the participant.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Learn(Aeon aeon, Participant thisParticipant, ParticipantQuery participantQuery, ParticipantRequest participantRequest, ParticipantResult participantResult, XmlNode templateNode)
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
            if (TemplateNode.Name.ToLower() == "learn")
            {
                // Currently only *.aeon files in the local filesystem can be referenced, as per design.
                if (TemplateNode.InnerText.Length > 0)
                {
                    string path = TemplateNode.InnerText;
                    FileInfo fi = new FileInfo(path);
                    if (fi.Exists)
                    {
                        XmlDocument doc = new XmlDocument();
                        try
                        {
                            doc.Load(path);
                            ThisAeon.LoadAeonFromXml(doc, path);
                        }
                        catch
                        {
                            Logging.WriteLog("Failed to learn something new from the following URI: " + path, Logging.LogType.Error, Logging.LogCaller.Learn);
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
