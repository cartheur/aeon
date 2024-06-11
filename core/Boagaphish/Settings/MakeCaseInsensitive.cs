//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Settings
{
    /// <summary>
    /// Normalizes the input text into upper case.
    /// </summary>
    public class MakeCaseInsensitive
    {
        /// <summary>
        /// An ease-of-use static method that re-produces the instance transformation methods.
        /// </summary>
        /// <param name="input">The string to transform</param>
        /// <returns>The resulting string</returns>
        public static string TransformInput(string input)
        {
            return input.ToUpper();
        }
    }
}
