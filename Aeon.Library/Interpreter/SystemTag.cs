//
// This AGI is the intellectual property of Dr. Christopher A. Tucker and Cartheur Research, B.V. Copyright 2008 - 2024, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact the company at: cartheur.research@pm.me.
//
namespace Aeon.Library
{
    [CustomTag]
    public class SystemTag : AeonHandler
    {
        public SystemTag()
        {
            InputString = "testtag";
        }

        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "system")
            {
                return "Override default tag implementation works correctly";
            }
            return string.Empty;
        }
    }
}
