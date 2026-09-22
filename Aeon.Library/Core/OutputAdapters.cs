//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
namespace Aeon.Library
{
    /// <summary>The presentation modalities an output adapter may support.</summary>
    [Flags]
    public enum OutputModality
    {
        Text = 1,
        Voice = 2,
        Tactile = 4,
        Gesture = 8,
        Animation = 16,
        Hardware = 32
    }

    /// <summary>
    /// A response prepared for one or more output adapters, with optional contextual metadata for expressive adapters.
    /// </summary>
    public sealed record OutputPresentation(
        string Speaker,
        string Text,
        OutputModality Modalities,
        EmotiveIndication EmotiveIndication = null,
        InstructionalDisplacement InstructionalDisplacement = null);

    /// <summary>
    /// Boundary for an output device or service. Implementations may synthesize speech, drive hardware, or render animation.
    /// </summary>
    public interface IOutputAdapter
    {
        /// <summary>Gets the modalities this adapter can present.</summary>
        OutputModality SupportedModalities { get; }

        /// <summary>Presents a response. Implementations own their device-specific timing and error handling.</summary>
        void Present(OutputPresentation presentation);
    }

    /// <summary>Routes a presentation only to adapters that support a requested modality.</summary>
    public sealed class OutputDispatcher
    {
        private readonly List<IOutputAdapter> _adapters = new List<IOutputAdapter>();
        private readonly object _sync = new object();
        private readonly Action<IOutputAdapter, Exception> _onAdapterFailure;

        /// <summary>Initializes a dispatcher with optional adapters.</summary>
        public OutputDispatcher(IEnumerable<IOutputAdapter> adapters = null, Action<IOutputAdapter, Exception> onAdapterFailure = null)
        {
            _onAdapterFailure = onAdapterFailure;
            if (adapters != null)
            {
                _adapters.AddRange(adapters.Where(adapter => adapter != null));
            }
        }

        /// <summary>Adds an adapter to this dispatcher.</summary>
        public void Add(IOutputAdapter adapter)
        {
            ArgumentNullException.ThrowIfNull(adapter);
            lock (_sync)
            {
                _adapters.Add(adapter);
            }
        }

        /// <summary>Routes the presentation to each compatible adapter.</summary>
        public void Present(OutputPresentation presentation)
        {
            ArgumentNullException.ThrowIfNull(presentation);
            IOutputAdapter[] adapters;
            lock (_sync)
            {
                adapters = _adapters.ToArray();
            }

            foreach (IOutputAdapter adapter in adapters)
            {
                try
                {
                    if ((adapter.SupportedModalities & presentation.Modalities) != 0)
                    {
                        adapter.Present(presentation);
                    }
                }
                catch (Exception exception)
                {
                    try
                    {
                        _onAdapterFailure?.Invoke(adapter, exception);
                    }
                    catch
                    {
                        // Failure reporting is isolated just like device output.
                    }
                }
            }
        }
    }
}
