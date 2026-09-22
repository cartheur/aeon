//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Text.RegularExpressions;

namespace Aeon.Library
{
    /// <summary>Identifies whether input is a dialogue utterance or an explicit command.</summary>
    public enum InteractionKind
    {
        Dialogue,
        Command
    }

    /// <summary>A bounded subject-verb-predicate interpretation of one dialogue utterance.</summary>
    public sealed record DialogueIntent(string Subject, string Verb, string Predicate, IReadOnlyList<string> Tokens);

    /// <summary>An explicit slash command and its whitespace-delimited arguments.</summary>
    public sealed record CommandInstruction(string Name, IReadOnlyList<string> Arguments);

    /// <summary>The routed interpretation of a participant input.</summary>
    public sealed record InteractionRoute(InteractionKind Kind, DialogueIntent Dialogue, CommandInstruction Command);

    /// <summary>Routes slash commands separately from dialogue and produces a small, inspectable dialogue parse.</summary>
    public static class InteractionRouter
    {
        private static readonly Regex TokenPattern = new Regex("[\\p{L}\\p{Nd}']+", RegexOptions.Compiled);
        private static readonly Regex CommandPattern = new Regex("^/([a-z][a-z0-9-]*)(?:\\s+(.*))?$", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        /// <summary>Routes input. Commands must start with a slash; all other input is treated as dialogue.</summary>
        public static InteractionRoute Route(string input)
        {
            string text = (input ?? string.Empty).Trim();
            Match commandMatch = CommandPattern.Match(text);
            if (commandMatch.Success)
            {
                string argumentsText = commandMatch.Groups[2].Value;
                IReadOnlyList<string> arguments = string.IsNullOrWhiteSpace(argumentsText)
                    ? Array.Empty<string>()
                    : argumentsText.Split((char[])null, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                return new InteractionRoute(
                    InteractionKind.Command,
                    null,
                    new CommandInstruction(commandMatch.Groups[1].Value.ToLowerInvariant(), arguments));
            }

            string[] tokens = TokenPattern.Matches(text).Select(match => match.Value).ToArray();
            string subject = tokens.ElementAtOrDefault(0) ?? string.Empty;
            string verb = tokens.ElementAtOrDefault(1) ?? string.Empty;
            string predicate = tokens.Length > 2 ? string.Join(" ", tokens.Skip(2)) : string.Empty;
            return new InteractionRoute(
                InteractionKind.Dialogue,
                new DialogueIntent(subject, verb, predicate, tokens),
                null);
        }
    }
}
