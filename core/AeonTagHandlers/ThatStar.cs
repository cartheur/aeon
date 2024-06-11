//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Xml;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// The thatstar element tells the interpreter that it should substitute the contents of a wildcard from a pattern-side that element. 
    /// 
    /// The thatstar element has an optional integer index attribute that indicates which wildcard to use; the minimum acceptable value for the index is "1" (the first wildcard). 
    /// 
    /// An interpreter should raise an error if the index attribute of a star specifies a wildcard that does not exist in the that element's pattern content. Not specifying the index is the same as specifying an index of "1". 
    /// 
    /// The thatstar element does not have any content. 
    /// </summary>
    public class ThatStar : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ThatStar"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public ThatStar(Aeon aeon,
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
            if (TemplateNode.Name.ToLower() == "thatstar")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
                {
                    if (Query.ThatStar.Count > 0)
                    {
                        return Query.ThatStar[0];
                    }
                    Logging.WriteLog("An out-of-bounds index to thatstar was encountered when processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.ThatStar);
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
                                if (Query.ThatStar.Count > 0)
                                {
                                    if (result > 0)
                                    {
                                        return Query.ThatStar[result - 1];
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.ThatStar);
                                }
                                else
                                {
                                    Logging.WriteLog("An out-of-bounds index to thatstar was encountered when processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.ThatStar);
                                }
                            }
                            catch
                            {
                                Logging.WriteLog("A thatstar tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.ThatStar);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
