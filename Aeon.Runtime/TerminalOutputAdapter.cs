//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using Aeon.Library;

namespace Aeon.Runtime
{
    /// <summary>Terminal implementation of the text-output adapter.</summary>
    internal sealed class TerminalOutputAdapter : IOutputAdapter
    {
        private readonly TextWriter _writer;
        private readonly object _sync = new object();

        /// <summary>Initializes an adapter that writes to the supplied writer, or the terminal by default.</summary>
        public TerminalOutputAdapter(TextWriter writer = null)
        {
            _writer = writer ?? Console.Out;
        }

        /// <inheritdoc />
        public OutputModality SupportedModalities => OutputModality.Text;

        /// <inheritdoc />
        public void Present(OutputPresentation presentation)
        {
            ArgumentNullException.ThrowIfNull(presentation);
            lock (_sync)
            {
                _writer.WriteLine(string.IsNullOrWhiteSpace(presentation.Speaker)
                    ? presentation.Text
                    : presentation.Speaker + ": " + presentation.Text);
            }
        }
    }
}
