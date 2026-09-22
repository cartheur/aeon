//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Collections.ObjectModel;

namespace Aeon.Library
{
    /// <summary>
    /// The eight parent feelings in Table 1 of the cognitive-emotional interaction patent.
    /// </summary>
    public enum ParentFeeling
    {
        Happy,
        Confident,
        Energized,
        Helped,
        Insecure,
        Sad,
        Hurt,
        Tired
    }

    /// <summary>
    /// A selected parent feeling and one of its seven child moods.
    /// </summary>
    public sealed record MoodSelection(ParentFeeling ParentFeeling, string Mood, int Index);

    /// <summary>
    /// A normalized indication of the current mood, optionally calibrated by explicit learned feedback.
    /// </summary>
    public sealed record EmotiveIndication(ParentFeeling ParentFeeling, string Mood, int Index, double Weight);

    /// <summary>
    /// Implements the Fig. 4 mood wheel: eight parent feelings, seven child moods each, and an off state per parent.
    /// This state is intentionally independent of response selection; that integration belongs to the feedback-loop stage.
    /// </summary>
    public sealed class MoodState
    {
        private static readonly IReadOnlyDictionary<ParentFeeling, IReadOnlyList<string>> MoodCatalog =
            new ReadOnlyDictionary<ParentFeeling, IReadOnlyList<string>>(
                new Dictionary<ParentFeeling, IReadOnlyList<string>>
                {
                    [ParentFeeling.Happy] = new[] { "Hopeful", "Supported", "Charmed", "Grateful", "Optimistic", "Content", "Loving" },
                    [ParentFeeling.Confident] = new[] { "Strong", "Certain", "Assured", "Successful", "Valuable", "Beautiful", "Relaxed" },
                    [ParentFeeling.Energized] = new[] { "Determined", "Inspired", "Creative", "Healthy", "Vibrant", "Alert", "Motivated" },
                    [ParentFeeling.Helped] = new[] { "Cherished", "Befriended", "Appreciated", "Understood", "Empowered", "Accepted", "Loved" },
                    [ParentFeeling.Insecure] = new[] { "Weak", "Hopeless", "Doubtful", "Scared", "Anxious", "Stressed", "Nervous" },
                    [ParentFeeling.Sad] = new[] { "Depressed", "Lonely", "Angry", "Frustrated", "Upset", "Disappointed", "Hateful" },
                    [ParentFeeling.Hurt] = new[] { "Forgotten", "Ignored", "Offended", "Rejected", "Hated", "Mistreated", "Injured" },
                    [ParentFeeling.Tired] = new[] { "Indifferent", "Bored", "Sick", "Weary", "Powerless", "Listless", "Drained" }
                });

        private readonly object _sync = new object();
        private readonly IEmotiveWeightProvider _weightProvider;
        private readonly Dictionary<ParentFeeling, int> _lastMoodIndices = Enum
            .GetValues<ParentFeeling>()
            .ToDictionary(feeling => feeling, _ => 0);

        /// <summary>Gets the Table 1 parent-feeling and child-mood inventory.</summary>
        public static IReadOnlyDictionary<ParentFeeling, IReadOnlyList<string>> Catalog => MoodCatalog;

        /// <summary>Gets the current mood, or <see langword="null"/> while every parent is off.</summary>
        public MoodSelection CurrentMood { get; private set; }

        /// <summary>Gets the last selected 1-based child-mood index for each parent; zero means off.</summary>
        public IReadOnlyDictionary<ParentFeeling, int> LastMoodIndices
        {
            get
            {
                lock (_sync)
                {
                    return new ReadOnlyDictionary<ParentFeeling, int>(new Dictionary<ParentFeeling, int>(_lastMoodIndices));
                }
            }
        }

        /// <summary>Creates a mood state with each parent in the initial off state.</summary>
        public MoodState(IEmotiveWeightProvider weightProvider = null)
        {
            _weightProvider = weightProvider;
        }

        /// <summary>Creates a randomly selected mood from the allowed parent-feeling scope.</summary>
        public static MoodState Create(Random random = null, IEnumerable<ParentFeeling> allowedFeelings = null, IEmotiveWeightProvider weightProvider = null)
        {
            var state = new MoodState(weightProvider);
            state.Randomize(random ?? Random.Shared, allowedFeelings);
            return state;
        }

        /// <summary>Selects a child mood by name and records its 1-based indicator on its parent feeling.</summary>
        public EmotiveIndication SetMood(ParentFeeling parentFeeling, string mood)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(mood);
            IReadOnlyList<string> moods = MoodCatalog[parentFeeling];
            int index = IndexOf(moods, mood);
            if (index < 0)
            {
                throw new ArgumentException($"'{mood}' is not a mood of {parentFeeling}.", nameof(mood));
            }

            lock (_sync)
            {
                CurrentMood = new MoodSelection(parentFeeling, moods[index], index + 1);
                _lastMoodIndices[parentFeeling] = index + 1;
                return ToIndication(CurrentMood);
            }
        }

        /// <summary>Randomly selects a parent feeling and child mood from an optional allowed scope.</summary>
        public EmotiveIndication Randomize(Random random, IEnumerable<ParentFeeling> allowedFeelings = null)
        {
            ArgumentNullException.ThrowIfNull(random);
            ParentFeeling[] allowed = (allowedFeelings ?? Enum.GetValues<ParentFeeling>()).Distinct().ToArray();
            if (allowed.Length == 0)
            {
                throw new ArgumentException("At least one parent feeling must be allowed.", nameof(allowedFeelings));
            }

            ParentFeeling parentFeeling = allowed[random.Next(allowed.Length)];
            IReadOnlyList<string> moods = MoodCatalog[parentFeeling];
            return SetMood(parentFeeling, moods[random.Next(moods.Count)]);
        }

        /// <summary>Returns the current calibrated emotive indication, or <see langword="null"/> if all parents are off.</summary>
        public EmotiveIndication GetCurrentIndication()
        {
            lock (_sync)
            {
                return CurrentMood == null ? null : ToIndication(CurrentMood);
            }
        }

        private static int IndexOf(IReadOnlyList<string> moods, string mood)
        {
            for (int index = 0; index < moods.Count; index++)
            {
                if (string.Equals(moods[index], mood.Trim(), StringComparison.OrdinalIgnoreCase))
                {
                    return index;
                }
            }
            return -1;
        }

        private EmotiveIndication ToIndication(MoodSelection mood)
        {
            double baseline = (((int)mood.ParentFeeling * 7) + mood.Index) / 56.0;
            double weight = _weightProvider?.GetWeight(mood.ParentFeeling, mood.Mood, baseline) ?? baseline;
            return new EmotiveIndication(mood.ParentFeeling, mood.Mood, mood.Index, weight);
        }
    }
}
