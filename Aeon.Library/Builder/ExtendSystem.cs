//
// Copyright 2003-2025 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
namespace Aeon.Library.Builder
{
    public class ExtendSystem
    {
        private readonly LoaderPaths _runtimeDirectory;

        public ExtendSystem(LoaderPaths runtimeDirectory)
        {
            _runtimeDirectory = runtimeDirectory;
        }
        /// <summary>
        /// Saves a validated, reviewed learning proposal in the configured local learning directory.
        /// </summary>
        /// <param name="proposal">The participant-derived category to save.</param>
        /// <param name="instance">The preferred positive file sequence number.</param>
        /// <returns>The newly created local file path.</returns>
        public string CreateAeonFile(LearningProposal proposal, int instance)
        {
            ArgumentNullException.ThrowIfNull(proposal);
            if (instance < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(instance), "The learning file sequence must be positive.");
            }

            string personalityDirectory = Path.Combine(_runtimeDirectory.PathToLearnedFiles);
            Directory.CreateDirectory(personalityDirectory);
            for (int candidate = instance; ; candidate++)
            {
                string fileName = Path.Combine(personalityDirectory, "learned-" + candidate + ".aeon");
                try
                {
                    using var stream = new FileStream(fileName, FileMode.CreateNew, FileAccess.Write, FileShare.None);
                    proposal.ToDocument().Save(stream);
                    return fileName;
                }
                catch (IOException) when (File.Exists(fileName))
                {
                    // Preserve previous learning rather than overwriting it; try the next sequence number.
                }
            }
        }

        // The templateInput field has a variety of possibilities, given the context of the language.
        public string CreateAeonFile(string patternInput, string templateInput, int instance)
        {
            return CreateAeonFile(LearningProposal.Create(patternInput, templateInput), instance);
        }
    }
}
