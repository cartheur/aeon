//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
namespace Aeon.Library
{
    /// <summary>
    /// A response candidate with optional constraints for the current mood and a repeated participant trajectory.
    /// </summary>
    public sealed class ResponseVariant
    {
        /// <summary>Initializes a response candidate.</summary>
        public ResponseVariant(string content, ParentFeeling? parentFeeling = null, string mood = null, bool requiresRepeatedTrajectory = false)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(content);
            if (parentFeeling.HasValue != !string.IsNullOrWhiteSpace(mood))
            {
                throw new ArgumentException("A mood constraint must specify both a parent feeling and a child mood.");
            }
            if (parentFeeling.HasValue && !MoodState.Catalog[parentFeeling.Value].Any(candidate => string.Equals(candidate, mood.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                throw new ArgumentException($"'{mood}' is not a mood of {parentFeeling}.", nameof(mood));
            }

            Content = content;
            ParentFeeling = parentFeeling;
            Mood = mood?.Trim();
            RequiresRepeatedTrajectory = requiresRepeatedTrajectory;
        }

        /// <summary>Gets the response markup or text to return when this variant is selected.</summary>
        public string Content { get; }

        /// <summary>Gets the optional required parent feeling.</summary>
        public ParentFeeling? ParentFeeling { get; }

        /// <summary>Gets the optional required child mood.</summary>
        public string Mood { get; }

        /// <summary>Gets whether the current raw input must have occurred in prior trajectory history.</summary>
        public bool RequiresRepeatedTrajectory { get; }

        /// <summary>Gets the number of explicit feedback constraints on this candidate.</summary>
        public int Specificity => (ParentFeeling.HasValue ? 1 : 0) + (RequiresRepeatedTrajectory ? 1 : 0);
    }

    /// <summary>
    /// The observable result of feedback-based response selection.
    /// </summary>
    public sealed record FeedbackSelection(ResponseVariant Variant, bool IsRepeatedTrajectory, EmotiveIndication EmotiveIndication);

    /// <summary>
    /// Selects the most specific eligible response variant from participant trajectory history and the aeon's current mood.
    /// </summary>
    public static class FeedbackResponseSelector
    {
        /// <summary>
        /// Selects the first most-specific eligible variant. An unannotated variant is an explicit fallback.
        /// </summary>
        public static FeedbackSelection Select(IEnumerable<ResponseVariant> variants, MoodState moodState, TrajectoryHistory trajectoryHistory, string rawInput)
        {
            ArgumentNullException.ThrowIfNull(variants);
            ArgumentNullException.ThrowIfNull(moodState);
            ArgumentNullException.ThrowIfNull(trajectoryHistory);

            ResponseVariant[] candidates = variants.ToArray();
            if (candidates.Length == 0)
            {
                return null;
            }

            bool isRepeatedTrajectory = IsRepeatedTrajectory(trajectoryHistory, rawInput);
            EmotiveIndication indication = moodState.GetCurrentIndication();
            ResponseVariant selected = candidates
                .Where(candidate => IsEligible(candidate, indication, isRepeatedTrajectory))
                .OrderByDescending(candidate => candidate.Specificity)
                .FirstOrDefault();

            return selected == null ? null : new FeedbackSelection(selected, isRepeatedTrajectory, indication);
        }

        private static bool IsEligible(ResponseVariant candidate, EmotiveIndication indication, bool isRepeatedTrajectory)
        {
            if (candidate.RequiresRepeatedTrajectory && !isRepeatedTrajectory)
            {
                return false;
            }
            if (!candidate.ParentFeeling.HasValue)
            {
                return true;
            }
            return indication != null
                && indication.ParentFeeling == candidate.ParentFeeling.Value
                && string.Equals(indication.Mood, candidate.Mood, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsRepeatedTrajectory(TrajectoryHistory trajectoryHistory, string rawInput)
        {
            string normalizedInput = Normalize(rawInput);
            return normalizedInput.Length > 0 && trajectoryHistory.Indications.Any(indication => Normalize(indication.RawInput) == normalizedInput);
        }

        private static string Normalize(string input)
        {
            return (input ?? string.Empty).Trim().ToUpperInvariant();
        }
    }
}
