//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.IO;
using System.Xml;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// The learn element instructs the interpreter to retrieve a resource specified by a URI, and to process its aeon object contents.
    /// </summary>
    public class Learn : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Learn"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Learn(Aeon aeon,
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
