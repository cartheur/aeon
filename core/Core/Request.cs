//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// Encapsulates information about a request sent to aeon for processing.
    /// </summary>
    public class Request
    {
        /// <summary>
        /// The raw input from the user.
        /// </summary>
        public string RawInput;
        /// <summary>
        /// The time at which this request was created within the system.
        /// </summary>
        public DateTime StartedOn;
        /// <summary>
        /// The user who made this request.
        /// </summary>
        public User ThisUser;
        /// <summary>
        /// The aeon to which the request is being made.
        /// </summary>
        public Aeon ThisAeon;
        /// <summary>
        /// The final result produced by this request.
        /// </summary>
        public Result UserResult;
        /// <summary>
        /// Flag to show that the request has timed out.
        /// </summary>
        public bool HasTimedOut = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="Request"/> class.
        /// </summary>
        /// <param name="rawInput">The raw input from the user.</param>
        /// <param name="thisUser">The user who made the request.</param>
        /// <param name="thisAeon">The presence for this request.</param>
        public Request(string rawInput, User thisUser, Aeon thisAeon)
        {
            RawInput = rawInput;
            ThisUser = thisUser;
            ThisAeon = thisAeon;
            StartedOn = DateTime.Now;
        }
    }
}
