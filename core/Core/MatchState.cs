//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Cartheur.Animals.Core
{
    /// <summary>
    /// Denotes what part of the input path a node represents. 
    /// </summary>
    /// <remarks>
    /// Used when pushing values represented by wildcards onto collections for the star, thatstar, emotion, and topicstar values.
    /// </remarks>
    public enum MatchState
    {
        /// <summary>
        /// The user input state.
        /// </summary>
        UserInput,
        /// <summary>
        /// The that state.
        /// </summary>
        That,
        /// <summary>
        /// The topic state.
        /// </summary>
        Topic,
        /// <summary>
        /// The emotion state.
        /// </summary>
        Emotion
    }
}
