//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// Strips any illegal characters found in the input string. Illegal characters are referenced from aeon's Strippers regex that is defined in the setup XML file.
    /// </summary>
    public class StripIllegalCharacters : TextTransformer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StripIllegalCharacters"/> class.
        /// </summary>
        /// <param name="aeon">The aeon is this transformer a part of</param>
        /// <param name="inputString">The input string to be transformed</param>
        public StripIllegalCharacters(Aeon aeon, string inputString) : base(aeon, inputString)
        { }
        /// <summary>
        /// Initializes a new instance of the <see cref="StripIllegalCharacters"/> class.
        /// </summary>
        /// <param name="aeon">The aeon this transformer is a part of</param>
        public StripIllegalCharacters(Aeon aeon)
            : base(aeon) 
        { }
        /// <summary>
        /// The method that does the actual processing of the text.
        /// </summary>
        protected override string ProcessChange()
        {
            return ThisAeon.Strippers.Replace(InputString, " ");
        }
    }
}
