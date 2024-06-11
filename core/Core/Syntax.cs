//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Cartheur.Animals.Core
{
    /// <summary>
    /// Commands recognized by aeon to perform an activity outside of its conversational significance.
    /// </summary>
    public class Syntax
    {
        /// <summary>
        /// The aeon listen command, inclusive trailing space.
        /// </summary>
        public static string AeonListenCommand = "aeon ";
        /// <summary>
        /// The listen command using more obvious intonation from the speaker and that cannot be confused with ordinary words.
        /// </summary>
        public static string ListenCommand = "baytow";// Not used but keeping it here.
        /// <summary>
        /// The amend conversation command.
        /// </summary>
        public static string AmendConversationCommand = "alter conversation";
        /// <summary>
        /// The change emotion command.
        /// </summary>
        public static string ChangeEmotionCommand = "aeon change emotion";
        /// <summary>
        /// The nao stand up command.
        /// </summary>
        public static string StandUpCommand = "nao stand up";
        /// <summary>
        /// The nao rest position command.
        /// </summary>
        public static string RestPositionCommand = "nao rest position";
        /// <summary>
        /// Gets or sets a value indicating whether [command received].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [command received]; otherwise, <c>false</c>.
        /// </value>
        public static bool CommandReceived { get; set; }
    }
}
