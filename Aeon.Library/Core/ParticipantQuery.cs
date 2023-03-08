//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// A container class for holding wildcard participant-query matches encountered during a path interrogation.
    /// </summary>
    public class ParticipantQuery
    {
        /// <summary>
        /// The trajectory that this participant query relates to.
        /// </summary>
        public string Trajectory;
        /// <summary>
        /// The template found from searching the brain with the path .
        /// </summary>
        public string Template = string.Empty;
        /// <summary>
        /// If the raw input matches a wildcard then this attribute will contain the block of text that the participant has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> InputStar = new List<string>();
        /// <summary>
        /// If the "that" part of the normalized path contains a wildcard then this attribute will contain the block of text that the participant has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> ThatStar = new List<string>();
        /// <summary>
        /// If the "topic" part of the normalized path contains a wildcard then this attribute will contain the block of text that the participant has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> TopicStar = new List<string>();
        /// <summary>
        /// The "emotional" part of the normalized path contains a wildcard then this attribute will contain the block of text that the participant has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> EmotionStar = new List<string>();
        /// <summary>
        /// The "knowldge-relational" part of the normalized path contains a wildcard then this attribute will contain the block of text that the participant has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> KnowledgeStar = new List<string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipantQuery"/> class.
        /// </summary>
        /// <param name="trajectory">The trajectory that this participant query relates to.</param>
        public ParticipantQuery(string trajectory)
        {
            Trajectory = trajectory;
        }
    }
}
