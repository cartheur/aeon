using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    [CustomTag]
    public class System : AeonTagHandler
    {
        public System()
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
