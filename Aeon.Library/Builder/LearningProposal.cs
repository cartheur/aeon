//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Xml.Linq;

namespace Aeon.Library.Builder
{
    /// <summary>
    /// A reviewed, literal-text category proposed by a participant for local learning.
    /// </summary>
    public sealed class LearningProposal
    {
        /// <summary>The maximum permitted pattern length.</summary>
        public const int MaximumPatternLength = 256;

        /// <summary>The maximum permitted response length.</summary>
        public const int MaximumResponseLength = 1024;

        private LearningProposal(string pattern, string response)
        {
            Pattern = pattern;
            Response = response;
        }

        /// <summary>Gets the literal participant phrase to match.</summary>
        public string Pattern { get; }

        /// <summary>Gets the literal response to return for the phrase.</summary>
        public string Response { get; }

        /// <summary>Validates and creates a participant-derived learning proposal.</summary>
        public static LearningProposal Create(string pattern, string response)
        {
            return new LearningProposal(Validate(pattern, nameof(pattern), MaximumPatternLength), Validate(response, nameof(response), MaximumResponseLength));
        }

        /// <summary>Builds the exact local XML document that will be saved after participant confirmation.</summary>
        public XDocument ToDocument()
        {
            return new XDocument(
                new XElement("aeon", new XAttribute("version", 1.1),
                    new XElement("category",
                        new XElement("pattern", Pattern),
                        new XElement("template", Response))));
        }

        private static string Validate(string value, string parameterName, int maximumLength)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(value, parameterName);
            string trimmed = value.Trim();
            if (trimmed.Length > maximumLength)
            {
                throw new ArgumentOutOfRangeException(parameterName, "The value exceeds the maximum permitted length of " + maximumLength + " characters.");
            }
            if (trimmed.Any(char.IsControl))
            {
                throw new ArgumentException("The value must be a single line of text without control characters.", parameterName);
            }
            if (parameterName == "pattern" && (trimmed.Contains('*') || trimmed.Contains('_')))
            {
                throw new ArgumentException("A learned phrase must not contain pattern wildcard characters.", parameterName);
            }
            return trimmed;
        }
    }
}
