//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Cartheur.Animals.Core
{
    /// <summary>
    /// Dynamic-amending the composite database and grammar repository.
    /// </summary>
    /// <remarks>Can be used to leverage USPTO-104.</remarks>
    public static class Amend
    {
        /// <summary>
        /// The kind of amend operation to perform.
        /// </summary>
        public enum Operation
        {
            /// <summary>
            /// Add something to a field.
            /// </summary>
            Add,
            /// <summary>
            /// Delete the entry.
            /// </summary>
            Delete,
            /// <summary>
            /// Replace a field with a better entry.
            /// </summary>
            Replace
        }
        /// <summary>
        /// Amends the conversational database.
        /// </summary>
        /// <param name="incorrect">The incorrect entry.</param>
        /// <param name="alteration">The alteration to replace it.</param>
        /// <param name="operation">The operation to perform.</param>
        /// <returns></returns>
        public static bool AmendDatabase(string incorrect, string alteration, Operation operation)
        {
            
            return true;
        }
        /// <summary>
        /// Amends the grammar repository.
        /// </summary>
        /// <param name="incorrect">The incorrect entry.</param>
        /// <param name="alteration">The alteration to replace it.</param>
        /// <param name="operation">The operation to perform.</param>
        /// <returns></returns>
        public static bool AmendGrammar(string incorrect, string alteration, Operation operation)
        {

            return true;
        }
    }
}
