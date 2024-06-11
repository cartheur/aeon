using Cartheur.Animals.Utilities;

namespace Cartheur.Animals.AeonTagHandlers
{
    /// <summary>
    /// A simple example to provide a template for a custom tag handler.
    /// 
    /// The recipe is as follows:
    /// 
    /// 1. Create a new library project to contain your custom tag classes. DONE
    /// 2. Add the dll as a reference to your project. DONE
    /// 3. Create a public class with the same name as the tag you wish to handle. DONE
    /// 4. Reference System.Xml and Animals.Utilities. DONE
    /// 5. Add the [CustomTag] attribute to the class. DONE
    /// 6. Create a default constructor that puts something in the "this.inputString" attribute. (This is
    /// because AeonTagHandler inherits from the TextTransformer class and because of limitations with
    /// instantiating late bound classes cannot call the "regular" AeonTagHandler constructor that would 
    /// put the XML node's InnerText into inputString). In any case this.inputString is not used by 
    /// AeonTagHandlers as they have direct access to the node to be processed (among other things - see below).
    /// 7. Override the ProcessChange() method. This is where the work happens. Nota Bene: It is good 
    /// practice to check the name of the node being processed and return string.Empty if it doesn't match.
    /// 8. By default the inner XML of the tag is recursively processed before the tag itself is processed. If
    /// you want the result of the tag to be processed first and then the resulting inner XML then set the
    /// this.isRecursive boolean flag to false (useful when working with tags similar to random or condition).
    /// 
    /// When working within ProcessChange you have access to the following useful objects:
    /// 
    /// this.templateNode - An XmlNode object that represents the tag you're processing
    /// this.bot - An instance of Bot that represents the bot that is currently processing the input
    /// this.user - An instance of User that represents the user who originated the current input
    /// this.query - An instance of SubQuery that represents an individual query against the
    /// graphmaster. Contains the various wildcard match collections
    /// this.request - An instance of Request that encapsulates all sorts of useful information about
    /// the input from the user
    /// this.result - An instance of Result that encapsulates all sorts of useful information about 
    /// the output generated by the bot.
    /// 
    /// Finally to load the dll into your bot call the loadCustomTagHandlers(string pathToDLL) method of the
    /// Bot object that is your bot. An exception will be raised if you attempt to duplicate tag handling.
    /// </summary>
    [CustomTag]
    public class Test : AeonTagHandler
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Test"/> class.
        /// </summary>
        public Test()
        {
            InputString = "test";
        }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        /// <returns>
        /// The resulting processed text.
        /// </returns>
        protected override string ProcessChange()
        {
            if (TemplateNode.Name.ToLower() == "test")
            {
                return "The test tag handler works: " + TemplateNode.InnerText;
            }
            return string.Empty;
        }
    }
}
