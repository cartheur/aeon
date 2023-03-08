//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// Drawing 100, Feature 105. The encapsulation of the entity who is interacting with the aeon.
    /// </summary>
    public class Participant
    {
        /// <summary>
        /// The aeon this participant is using.
        /// </summary>
        public Aeon ParticipantAeon;
        /// <summary>
        /// The GUID that identifies this participant.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// A collection of all the result objects returned to the participant in this session.
        /// </summary>
        public readonly List<ParticipantResult> AeonReplies = new List<ParticipantResult>();
        /// <summary>
        /// The value of the "topic" predicate.
        /// </summary>
        public string Topic
        {
            get
            {
                return Predicates.GrabSetting("topic");
            }
        }
        /// <summary>
        /// The participant's emotion, initially loaded to the "emotion" value set in the predicates file.
        /// </summary>
        public string Emotion { get; set; }
        /// <summary>
		/// The predicates associated with this particular participant.
		/// </summary>
        public SettingsDictionary Predicates;
        /// <summary>
        /// The most recent result to be returned by aeon.
        /// </summary>
        public ParticipantResult LastAeonReply
        {
            get
            {
                return AeonReplies.Count > 0 ? AeonReplies[0] : null;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Participant"/> class.
        /// </summary>
        /// <param name="participantName">The name of the participant.</param>
        /// <param name="aeon">The aeon that the participant is connected to.</param>
        /// <exception cref="System.Exception">The ParticipantID cannot be empty.</exception>
		public Participant(string participantName, Aeon aeon)
		{
            if (participantName.Length > 0)
            {
                Name = participantName;
                ParticipantAeon = aeon;
                Predicates = new SettingsDictionary(ParticipantAeon);
                Predicates = ParticipantAeon.DefaultPredicates;
            }
            else
            {
                throw new Exception("The ParticipantID cannot be empty.");
            }
		}
        /// <summary>
        /// Returns the string to use for the next that part of a subsequent path.
        /// </summary>
        /// <returns>The string to use for that.</returns>
        public string GetLastAeonOutput()
        {
            if (AeonReplies.Count > 0)
            {
                return AeonReplies[0].Output;
            }
            return "*";
        }
        /// <summary>
        /// Returns the first sentence of the last output from aeon.
        /// </summary>
        /// <returns>The first sentence of the last output from aeon.</returns>
        public string GetThat()
        {
            return GetThat(0,0);
        }
        /// <summary>
        /// Returns the first sentence of the output n-steps ago from aeon.
        /// </summary>
        /// <param name="n">The number of steps back to go.</param>
        /// <returns>The first sentence of the output n-steps ago from aeon.</returns>
        public string GetThat(int n)
        {
            return GetThat(n, 0);
        }
        /// <summary>
        /// Returns the sentence numbered by "sentence" of the output n-steps ago from aeon.
        /// </summary>
        /// <param name="n">The number of steps back to go.</param>
        /// <param name="sentence">The sentence number to get.</param>
        /// <returns>The sentence numbered by "sentence" of the output n-steps ago from aeon.</returns>
        public string GetThat(int n, int sentence)
        {
            if ((n >= 0) & (n < AeonReplies.Count))
            {
                ParticipantResult historicResult = AeonReplies[n];
                if ((sentence >= 0) & (sentence < historicResult.OutputSentences.Count))
                {
                    return historicResult.OutputSentences[sentence];
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Returns the first sentence of the last output from aeon.
        /// </summary>
        /// <returns>The first sentence of the last output from aeon.</returns>
        public string GetAeonReply()
        {
            return GetAeonReply(0, 0);
        }
        /// <summary>
        /// Returns the first sentence from the output from aeon n-steps ago.
        /// </summary>
        /// <param name="n">The number of steps back to go.</param>
        /// <returns>The first sentence from the output from aeon n-steps ago.</returns>
        public string GetAeonReply(int n)
        {
            return GetAeonReply(n, 0);
        }
        /// <summary>
        /// Returns the identified sentence number from the output from aeon n-steps ago.
        /// </summary>
        /// <param name="n">The number of steps back to go.</param>
        /// <param name="sentence">The sentence number to return.</param>
        /// <returns>The identified sentence number from the output from aeon n-steps ago.</returns>
        public string GetAeonReply(int n, int sentence)
        {
            if ((n >= 0) & (n < AeonReplies.Count))
            {
                ParticipantResult historicResult = AeonReplies[n];
                if ((sentence >= 0) & (sentence < historicResult.InputSentences.Count))
                {
                    return historicResult.InputSentences[sentence];
                }
            }
            return string.Empty;
        }
        /// <summary>
        /// Adds the latest result from aeon to the results collection.
        /// </summary>
        /// <param name="latestResult">The latest result from aeon.</param>
        public void AddResult(ParticipantResult latestResult)
        {
            AeonReplies.Insert(0, latestResult);
        }
    }
}
