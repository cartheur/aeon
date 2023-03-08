//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
using System.Text;

namespace Aeon.Library
{
    /// <summary>
    /// Encapsulates information about the result of a request to the mind.
    /// </summary>
    public class ParticipantResult
    {
        /// <summary>
        /// The participant's aeon that is providing the answer.
        /// </summary>
        public Aeon ThisAeon;
        /// <summary>
        /// The participant for whom this is a result.
        /// </summary>
        public Participant ThisParticipant;
        /// <summary>
        /// The request from the participant which contains participant input.
        /// </summary>
        public ParticipantRequest ParticipantRequest;
        /// <summary>
        /// The normalized sentence(s) (paths) fed into the brain.
        /// </summary>
        public List<string> NormalizedTrajectories = new List<string>();
        /// <summary>
        /// Gets or sets the equation trajectory.
        /// </summary>
        /// <value>
        /// The equation trajectory.
        /// </value>
        public string EquationTrajectory { get; set; }
        /// <summary>
        /// The amount of time the request took to process.
        /// </summary>
        public TimeSpan Duration;
        /// <summary>
        /// The last message time.
        /// </summary>
        public DateTime LastMessageTime;
        /// <summary>
        /// Todo: Something which helps the program evolve from its own knowledge paths.
        /// </summary>
        //public Indication TrajectoryIndication { get; set; }
        /// <summary>
        /// Gets or sets the trajectory indication value.
        /// </summary>
        /// <value>
        /// The trajectory indication value.
        /// </value>
        public double[,] TrajectoryIndicationValue { get; set; }
        /// <summary>
        /// The result from the presence including logging and checking.
        /// </summary>
        public string Output
        {
            get
            {
                if (OutputSentences.Count > 0)
                {
                    return RawOutput;
                }
                if (ParticipantRequest.HasTimedOut)
                {
                    return ThisAeon.TimeOutMessage;
                }
                StringBuilder trajectories = new StringBuilder();
                foreach (string pattern in NormalizedTrajectories)
                {
                    trajectories.Append(pattern + "\r\n");
                }
                LastMessageTime = DateTime.Now;
                ThisAeon.ErrorState = true;
                Logging.WriteLog("Program error. Output is completely empty, which indicates there is no path for the query. You said: \"" + ParticipantRequest.RawInput + "\". The path is: " + trajectories, Logging.LogType.Warning, Logging.LogCaller.Result);

                return string.Empty;
            }
        }
        /// <summary>
        /// Returns the raw sentences. This method is depreciated.
        /// </summary>
        public string RawOutput
        {
            get
            {
                StringBuilder result = new StringBuilder();
                foreach (string sentence in OutputSentences)
                {
                    string sentenceForOutput = sentence.Trim();
                    if (!CheckEndsAsSentence(sentenceForOutput))
                    {
                        sentenceForOutput += ".";
                    }
                    result.Append(sentenceForOutput + " ");
                    LastMessageTime = DateTime.Now;
                }
                return result.ToString();
            }
        }
        /// <summary>
        /// The SubQuery objects processed by the brain which contain the templates that are to be converted into the collection of Sentences.
        /// </summary>
        public List<ParticipantQuery> SubQueries = new List<ParticipantQuery>();
        /// <summary>
        /// The individual sentences produced by the brain that form the complete response.
        /// </summary>
        public List<string> OutputSentences = new List<string>();
        /// <summary>
        /// The individual sentences that constitute the raw input from the participant.
        /// </summary>
        public List<string> InputSentences = new List<string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipantResult"/> class.
        /// </summary>
        /// <param name="thisParticipant">The participant for whom this is a result.</param>
        /// <param name="aeon">The brain providing the result.</param>
        /// <param name="participantRequest">The request that originated this result.</param>
        /// <param name="trajectoryEquation">The characteristic equation for the trajectory.</param>
        public ParticipantResult(Participant thisParticipant, Aeon aeon, ParticipantRequest participantRequest, string trajectoryEquation)
        {
            ThisParticipant = thisParticipant;
            ThisAeon = aeon;
            ParticipantRequest = participantRequest;
            ParticipantRequest.ParticipantResult = this;
            EquationTrajectory = trajectoryEquation;
        }
        /// <summary>
        /// Returns the raw output from the brain.
        /// </summary>
        /// <returns>The raw output from the brain.</returns>
        public override string ToString()
        {
            return Output;
        }
        /// <summary>
        /// Checks that the provided sentence ends with a sentence splitter.
        /// </summary>
        /// <param name="sentence">The sentence to check.</param>
        /// <returns>True if ends with an appropriate sentence splitter.</returns>
        private bool CheckEndsAsSentence(string sentence)
        {
            foreach (string splitter in ThisAeon.Splitters)
            {
                if (sentence.Trim().EndsWith(splitter))
                {
                    return true;
                }
            }
            return false;
        }
        //Todo: Drawing 300, Feature 307.
        /// <summary>
        /// Returns the trajectory indication.
        /// </summary>
        /// <remarks>Emotive indication in <see cref="Mood"/>, trajectory indication here.</remarks>
        public void ReturnIndication()
        {
            //TrajectoryIndication = new Indication(Boagaphish.Numeric.TransferFunction.BipolarSigmoid, EquationTrajectory)
            //{
               // WindowSize = Convert.ToInt32(ThisAeon.GlobalSettings.GrabSetting("windowsize")),
                //Iterations = Convert.ToInt32(ThisAeon.GlobalSettings.GrabSetting("iterations"))
            //};
            // Using a simple polynomial expression to create maps in the "brain".
            //TrajectoryIndication.TrajectoryPolynomial();
            // Using an aggressive solution search for the trajectory indication.
            //var trainingError = TrajectoryIndication.TrainNetwork();
            //TrajectoryIndicationValue = TrajectoryIndication.SearchSolution();
        }
    }
}
