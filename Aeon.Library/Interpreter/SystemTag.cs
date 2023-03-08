//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
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
