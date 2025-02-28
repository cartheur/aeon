//
// Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact m.e. at: cartheur@pm.me.
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
                return "Override default tag implementation works correctly.";
            }
            return string.Empty;
        }
    }
}
