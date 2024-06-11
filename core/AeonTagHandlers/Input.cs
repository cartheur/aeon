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
    /// The input element tells the interpreter that it should substitute the contents of a previous user input. 
    /// 
    /// The template-side input has an optional index attribute that may contain either a single integer or a comma-separated pair of integers. The minimum value for either of the integers in the index is "1". The index tells the interpreter which previous user input should be returned (first dimension), and optionally which "sentence" (see [8.3.2.]) of the previous user input. 
    /// 
    /// The interpreter should raise an error if either of the specified index dimensions is invalid at run-time. 
    /// 
    /// An unspecified index is the equivalent of "1,1". An unspecified second dimension of the index is the equivalent of specifying a "1" for the second dimension. 
    /// 
    /// The input element does not have any content. 
    /// </summary>
    public class Input : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Input"/> class.
        /// </summary>
        /// <param name="aeon">The aeon involved in this request.</param>
        /// <param name="thisUser">The user making the request.</param>
        /// <param name="query">The query that originated this node.</param>
        /// <param name="userRequest">The request sent by the user.</param>
        /// <param name="userResult">The result to be sent back to the user.</param>
        /// <param name="templateNode">The node to be processed.</param>
        public Input(Aeon aeon,
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
            if (TemplateNode.Name.ToLower() == "input")
            {
                if (TemplateNode.Attributes != null && TemplateNode.Attributes.Count == 0)
                {
                    return ThisUser.GetAeonReply();
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
                                        return ThisUser.GetAeonReply(localResult - 1, sentence - 1);
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Input);
                                }
                                else
                                {
                                    int result = Convert.ToInt32(TemplateNode.Attributes[0].Value.Trim());
                                    if (result > 0)
                                    {
                                        return ThisUser.GetAeonReply(result - 1);
                                    }
                                    Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Input);
                                }
                            }
                            catch
                            {
                                Logging.WriteLog("An input tag with a badly formed index (" + TemplateNode.Attributes[0].Value + ") was encountered processing the input: " + UserRequest.RawInput, Logging.LogType.Error, Logging.LogCaller.Input);
                            }
                        }
                    }
                }
            }
            return string.Empty;
        }
    }
}
