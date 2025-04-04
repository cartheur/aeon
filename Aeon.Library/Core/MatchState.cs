//
// Copyright 2003 - 2025, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact m.e. at: cartheur@pm.me.
//
namespace Aeon.Library
{
    /// <summary>
    /// Drawing 700. Denotes what part of the input path a node represents. 
    /// </summary>
    /// <remarks>
    /// Used when pushing values represented by wildcards onto collections for the star, thatstar, emotion, and topicstar values. This class expands the static capabilities of knowledge representation in the program.
    /// </remarks>
    public enum MatchState
    {
        /// <summary>
        /// The participant input state.
        /// </summary>
        ParticipantInput,
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
