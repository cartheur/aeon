//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
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
