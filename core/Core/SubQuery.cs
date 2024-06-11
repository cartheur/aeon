//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System.Collections.Generic;

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// A container class for holding wildcard matches encountered during a path interrogation.
    /// </summary>
    public class SubQuery
    {
        /// <summary>
        /// The trajectory that this query relates to.
        /// </summary>
        public string Trajectory;
        /// <summary>
        /// The template found from searching the brain with the path .
        /// </summary>
        public string Template = string.Empty;
        /// <summary>
        /// If the raw input matches a wildcard then this attribute will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> InputStar = new List<string>();
        /// <summary>
        /// If the "that" part of the normalized path contains a wildcard then this attribute will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> ThatStar = new List<string>();
        /// <summary>
        /// If the "topic" part of the normalized path contains a wildcard then this attribute will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> TopicStar = new List<string>();
        /// <summary>
        /// The "emotional" part of the normalized path contains a wildcard then this attribute will contain the block of text that the user has inputted that is matched by the wildcard.
        /// </summary>
        public List<string> EmotionStar = new List<string>(); 
        /// <summary>
        /// Initializes a new instance of the <see cref="SubQuery"/> class.
        /// </summary>
        /// <param name="trajectory">The trajectory that this query relates to.</param>
        public SubQuery(string trajectory)
        {
            Trajectory = trajectory;
        }
    }
}
