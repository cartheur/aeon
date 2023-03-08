//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// Encapsulates information about a request sent to aeon for knowledge processing.
    /// </summary>
    public class ParticipantRequest
    {
        /// <summary>
        /// The raw input from the participant.
        /// </summary>
        public string RawInput;
        /// <summary>
        /// The time at which this request was created within the system.
        /// </summary>
        public DateTime StartedOn;
        /// <summary>
        /// The participant who made this request.
        /// </summary>
        public Participant ThisParticipant;
        /// <summary>
        /// The aeon to which the request is being made.
        /// </summary>
        public Aeon ThisAeon;
        /// <summary>
        /// The final result produced by this request.
        /// </summary>
        public ParticipantResult ParticipantResult;
        /// <summary>
        /// Flag to show that the request has timed out.
        /// </summary>
        public bool HasTimedOut = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="ParticipantRequest"/> class.
        /// </summary>
        /// <param name="rawInput">The raw input from the participant.</param>
        /// <param name="thisParticipant">The participant who made the request.</param>
        /// <param name="thisAeon">The presence for this request.</param>
        public ParticipantRequest(string rawInput, Participant thisParticipant, Aeon thisAeon)
        {
            RawInput = rawInput;
            ThisParticipant = thisParticipant;
            ThisAeon = thisAeon;
            StartedOn = DateTime.Now;
        }
    }
}
