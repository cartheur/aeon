//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
namespace Aeon.Library
{
    /// <summary>
    /// Creates the participant-facing text emitted when an aeon detects that it is alone.
    /// </summary>
    public static class AlonePrompt
    {
        /// <summary>
        /// Formats an alone-state message consistently with ordinary terminal responses.
        /// </summary>
        /// <param name="aeonName">The name of the aeon issuing the prompt.</param>
        /// <param name="message">The configured alone-state message.</param>
        /// <returns>The prompt ready to present to the participant.</returns>
        /// <exception cref="ArgumentException">Thrown when either value is blank.</exception>
        public static string Format(string aeonName, string message)
        {
            if (string.IsNullOrWhiteSpace(aeonName))
            {
                throw new ArgumentException("An aeon name is required.", nameof(aeonName));
            }

            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException("An alone-state message is required.", nameof(message));
            }

            return aeonName + ": " + message;
        }
    }
}
