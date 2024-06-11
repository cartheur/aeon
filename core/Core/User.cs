//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// Encapsulates information and history of a user who has interacted with aeon.
    /// </summary>
    public class User
    {
        /// <summary>
        /// The aeon this user is using.
        /// </summary>
        public Aeon UserAeon;
        /// <summary>
        /// The GUID that identifies this user.
        /// </summary>
        public string UserName { get; set; }
        /// <summary>
        /// A collection of all the result objects returned to the user in this session.
        /// </summary>
        public readonly List<Result> AeonReplies = new List<Result>();
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
        /// The user's emotion, initially loaded to the "emotion" value set in the predicates file.
        /// </summary>
        public string Emotion { get; set; }
        /// <summary>
		/// The predicates associated with this particular user.
		/// </summary>
        public SettingsDictionary Predicates;
        /// <summary>
        /// The most recent result to be returned by aeon.
        /// </summary>
        public Result LastAeonReply
        {
            get
            {
                return AeonReplies.Count > 0 ? AeonReplies[0] : null;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="User"/> class.
        /// </summary>
        /// <param name="userName">The name of the user.</param>
        /// <param name="aeon">The aeon the user is connected to.</param>
        /// <exception cref="System.Exception">The UserID cannot be empty.</exception>
		public User(string userName, Aeon aeon)
		{
            if (userName.Length > 0)
            {
                UserName = userName;
                UserAeon = aeon;
                Predicates = new SettingsDictionary(UserAeon);
                Predicates = UserAeon.DefaultPredicates;
            }
            else
            {
                throw new Exception("The UserID cannot be empty.");
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
                Result historicResult = AeonReplies[n];
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
                Result historicResult = AeonReplies[n];
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
        public void AddResult(Result latestResult)
        {
            AeonReplies.Insert(0, latestResult);
        }
    }
}
