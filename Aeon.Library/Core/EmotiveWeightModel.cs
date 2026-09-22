//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Text.Json;

namespace Aeon.Library
{
    /// <summary>Provides a learned weight for a selected mood.</summary>
    public interface IEmotiveWeightProvider
    {
        /// <summary>Returns the learned weight, or the supplied baseline when no observations exist.</summary>
        double GetWeight(ParentFeeling parentFeeling, string mood, double baseline);
    }

    /// <summary>A persisted, transparent running-average weight for a mood.</summary>
    public sealed record EmotiveWeight(string Key, int Observations, double Weight);

    /// <summary>
    /// Stores explicit participant-provided mood feedback as a bounded, inspectable running average.
    /// It is intentionally not represented as a neural-network implementation.
    /// </summary>
    public sealed class EmotiveWeightModel : IEmotiveWeightProvider
    {
        private const int MaximumObservations = 1024;
        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        private readonly object _sync = new object();
        private readonly Dictionary<string, EmotiveWeight> _weights = new Dictionary<string, EmotiveWeight>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Gets a snapshot of the learned mood weights.</summary>
        public IReadOnlyList<EmotiveWeight> Weights
        {
            get
            {
                lock (_sync)
                {
                    return _weights.Values.OrderBy(weight => weight.Key, StringComparer.Ordinal).ToArray();
                }
            }
        }

        /// <inheritdoc />
        public double GetWeight(ParentFeeling parentFeeling, string mood, double baseline)
        {
            lock (_sync)
            {
                return _weights.TryGetValue(Key(parentFeeling, mood), out EmotiveWeight learned) ? learned.Weight : baseline;
            }
        }

        /// <summary>Incorporates an explicit target between zero and one for the current mood.</summary>
        public EmotiveWeight Train(EmotiveIndication indication, double target)
        {
            ArgumentNullException.ThrowIfNull(indication);
            if (!double.IsFinite(target) || target < 0 || target > 1)
            {
                throw new ArgumentOutOfRangeException(nameof(target), "Mood feedback must be between zero and one.");
            }

            string key = Key(indication.ParentFeeling, indication.Mood);
            lock (_sync)
            {
                if (!_weights.TryGetValue(key, out EmotiveWeight current))
                {
                    current = new EmotiveWeight(key, 0, indication.Weight);
                }
                int observations = Math.Min(current.Observations + 1, MaximumObservations);
                double weight = current.Observations == 0
                    ? target
                    : current.Weight + ((target - current.Weight) / observations);
                var updated = new EmotiveWeight(key, observations, weight);
                _weights[key] = updated;
                return updated;
            }
        }

        /// <summary>Loads learned weights from JSON if the file exists.</summary>
        public bool Load(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            if (!File.Exists(path)) return false;
            EmotiveWeight[] loaded = JsonSerializer.Deserialize<EmotiveWeight[]>(File.ReadAllText(path), SerializerOptions) ?? Array.Empty<EmotiveWeight>();
            lock (_sync)
            {
                _weights.Clear();
                foreach (EmotiveWeight weight in loaded.Where(weight => weight != null && weight.Observations > 0 && double.IsFinite(weight.Weight) && weight.Weight >= 0 && weight.Weight <= 1))
                {
                    _weights[weight.Key] = weight with { Observations = Math.Min(weight.Observations, MaximumObservations) };
                }
            }
            return true;
        }

        /// <summary>Saves learned weights as JSON using an atomic replacement.</summary>
        public void Save(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            string directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory)) throw new ArgumentException("The weight path must include a directory.", nameof(path));
            EmotiveWeight[] snapshot = Weights.ToArray();
            Directory.CreateDirectory(directory);
            string temporaryPath = path + ".tmp";
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(snapshot, SerializerOptions));
            File.Move(temporaryPath, path, true);
        }

        private static string Key(ParentFeeling parentFeeling, string mood)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(mood);
            return parentFeeling + ":" + mood.Trim();
        }
    }
}
