//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
namespace Aeon.Library
{
    /// <summary>
    /// A durable record of one interaction path and its resulting response.
    /// </summary>
    public sealed class TrajectoryIndication
    {
        /// <summary>Gets or sets the monotonic sequence number within a participant's history.</summary>
        public long Sequence { get; set; }

        /// <summary>Gets or sets the UTC time at which the indication was recorded.</summary>
        public DateTime RecordedAtUtc { get; set; }

        /// <summary>Gets or sets the participant input before normalization.</summary>
        public string RawInput { get; set; } = string.Empty;

        /// <summary>Gets or sets the normalized paths evaluated by the interpreter.</summary>
        public List<string> Paths { get; set; } = new List<string>();

        /// <summary>Gets or sets the response sentences produced for the interaction.</summary>
        public List<string> ResponseSentences { get; set; } = new List<string>();

        /// <summary>Gets or sets whether processing timed out before a complete response was produced.</summary>
        public bool TimedOut { get; set; }
    }
}
