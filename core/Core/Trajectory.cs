//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// The class responsible for engaging the learning algorithm.
    /// </summary>
    /// <remarks>USPTO-301</remarks>
    public class Trajectory
    {
        /// <summary>
        /// The indication type.
        /// </summary>
        public enum TrajectoryType
        {
            /// <summary>
            /// The trajectory type for a language-based scenario.
            /// </summary>
            Langauge,
            /// <summary>
            /// The trajectory type for a nonlanguage-based scenario.
            /// </summary>
            NonLanguage
        }
        /// <summary>
        /// Encapsulate a trajectory for processing.
        /// </summary>
        public Trajectory Create(Topic topic, Sentences sentence, IntentionCatalog catalog)
        {
            return new Trajectory();// USPTO-307.
        }

        /// <summary>
        /// Encapsulate a trajectory for passing to the algorithmic technique - USPTO-310.
        /// </summary>
        public Trajectory ParseTopic(Topic topic, Sentences sentence, IntentionCatalog catalog)
        {
            return new Trajectory();// USPTO-308.
        }
    }
}