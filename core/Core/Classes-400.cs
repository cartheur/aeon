//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
using Boagaphish.Format;
using  Cartheur.Animals.Personality;

/// <summary>
/// This file contains the classes mentioned in Figure 4 (drawing 400).
/// </summary>
namespace Cartheur.Animals.Core
{
    /// <summary>
    /// USPTO-405
    /// </summary>
    public static class CurrentMood
    {
        /// <summary>
        /// USPTO-401 yielding 403 via 402.
        /// </summary>
        /// <param name="mood">The mood object</param>
        /// <remarks>This also touches 404</remarks>
        public static Mood Create(this Mood mood)
        {
            var val = new StaticRandom(mood.SeedValue);
            return new Mood(mood.SeedValue, mood.ThisAeon, mood.ThisUser, mood.EmotiveIndication.ToString(), mood.EmotionBias);
        }
        /// <summary>
        /// USPTO-401 yielding 403 via 402.
        /// </summary>
        /// <param name="mood">The mood object</param>
        /// <remarks>This also touches 404 and 405</remarks>
        public static Mood Update(this Mood mood)
        {
            // Update the object.
            return new Mood(mood.SeedValue, mood.ThisAeon, mood.EmotiveIndication.ToString());
        }


    }
    
}