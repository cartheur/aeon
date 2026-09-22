//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Text.Json;

namespace Aeon.Library
{
    /// <summary>
    /// Maintains a bounded, per-participant history of trajectory indications.
    /// </summary>
    public sealed class TrajectoryHistory
    {
        /// <summary>The default maximum number of indications retained for one participant.</summary>
        public const int DefaultCapacity = 128;

        private static readonly JsonSerializerOptions SerializerOptions = new JsonSerializerOptions { WriteIndented = true };
        private readonly object _sync = new object();
        private readonly List<TrajectoryIndication> _indications = new List<TrajectoryIndication>();
        private long _lastSequence;

        /// <summary>Initializes a bounded trajectory history.</summary>
        /// <param name="capacity">The maximum number of indications to retain.</param>
        public TrajectoryHistory(int capacity = DefaultCapacity)
        {
            if (capacity < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(capacity), "Trajectory history capacity must be positive.");
            }

            Capacity = capacity;
        }

        /// <summary>Gets the maximum number of indications retained in memory and on disk.</summary>
        public int Capacity { get; }

        /// <summary>Gets an oldest-to-newest snapshot of the retained indications.</summary>
        public IReadOnlyList<TrajectoryIndication> Indications
        {
            get
            {
                lock (_sync)
                {
                    return _indications.ToArray();
                }
            }
        }

        /// <summary>Adds an indication and discards the oldest entry when capacity is exceeded.</summary>
        public void Add(TrajectoryIndication indication)
        {
            ArgumentNullException.ThrowIfNull(indication);
            lock (_sync)
            {
                indication.Paths ??= new List<string>();
                indication.ResponseSentences ??= new List<string>();
                indication.RawInput ??= string.Empty;
                indication.Sequence = ++_lastSequence;
                _indications.Add(indication);
                TrimToCapacity();
            }
        }

        /// <summary>Loads a persisted history if it exists.</summary>
        /// <returns><see langword="true"/> when a history file was loaded; otherwise <see langword="false"/>.</returns>
        public bool Load(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            if (!File.Exists(path))
            {
                return false;
            }

            List<TrajectoryIndication> loaded = JsonSerializer.Deserialize<List<TrajectoryIndication>>(File.ReadAllText(path), SerializerOptions)
                ?? new List<TrajectoryIndication>();
            lock (_sync)
            {
                _indications.Clear();
                _indications.AddRange(loaded.Where(indication => indication != null).OrderBy(indication => indication.Sequence));
                foreach (TrajectoryIndication indication in _indications)
                {
                    indication.Paths ??= new List<string>();
                    indication.ResponseSentences ??= new List<string>();
                    indication.RawInput ??= string.Empty;
                }

                TrimToCapacity();
                _lastSequence = _indications.Count == 0 ? 0 : _indications.Max(indication => indication.Sequence);
            }
            return true;
        }

        /// <summary>Persists the bounded history to JSON using an atomic replacement.</summary>
        public void Save(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path);
            string directory = Path.GetDirectoryName(path);
            if (string.IsNullOrEmpty(directory))
            {
                throw new ArgumentException("The history path must include a directory.", nameof(path));
            }

            TrajectoryIndication[] snapshot;
            lock (_sync)
            {
                snapshot = _indications.ToArray();
            }

            Directory.CreateDirectory(directory);
            string temporaryPath = path + ".tmp";
            File.WriteAllText(temporaryPath, JsonSerializer.Serialize(snapshot, SerializerOptions));
            File.Move(temporaryPath, path, true);
        }

        private void TrimToCapacity()
        {
            int overflow = _indications.Count - Capacity;
            if (overflow > 0)
            {
                _indications.RemoveRange(0, overflow);
            }
        }
    }
}
