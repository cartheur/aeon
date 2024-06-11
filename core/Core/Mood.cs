//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Cartheur.Animals.Core;
using Cartheur.Animals.Utilities;
using SweetPolynomial;

namespace Cartheur.Animals.Personality
{
    // Emotions which can be expressed by the program.
    /// <summary>
    /// The set of emotions the aeon can experience.
    /// </summary>
    public enum Emotions { Happy, Confident, Energized, Helped, Insecure, Sad, Hurt, Tired }
    /// <summary>
    /// The set of emotions the aeon can recognize.
    /// </summary>
    public enum UserEmotions { Angry, Happy, Neutral, Sad }
    // Subsets of these emotions. Parent is a feeling while the child is the mood.
    /// <summary>
    /// The subset emotions containing happy feelings.
    /// </summary>
    public enum HappyFeelings { Hopeful, Supported, Charmed, Grateful, Optimistic, Content, Loving }
    /// <summary>
    /// The subset emotions containing confident feelings.
    /// </summary>
    public enum ConfidentFeelings { Strong, Certain, Assured, Successful, Valuable, Beautiful, Relaxed }
    /// <summary>
    /// The subset emotions containing energized feelings.
    /// </summary>
    public enum EnergizedFeelings { Determined, Inspired, Creative, Healthy, Vibrant, Alert, Motivated }
    /// <summary>
    /// The subset emotions containing helped feelings.
    /// </summary>
    public enum HelpedFeelings { Cherished, Befriended, Appreciated, Understood, Empowered, Accepted, Loved }
    /// <summary>
    /// The subset emotions containing insecure feelings.
    /// </summary>
    public enum InsecureFeelings { Weak, Hopeless, Doubtful, Scared, Anxious, Stressed, Nervous }
    /// <summary>
    /// The subset emotions containing sad feelings.
    /// </summary>
    public enum SadFeelings { Depressed, Lonely, Angry, Frustrated, Upset, Disappointed, Hateful }
    /// <summary>
    /// The subset emotions containing hurt feelings.
    /// </summary>
    public enum HurtFeelings { Forgotten, Ignored, Offended, Rejected, Hated, Mistreated, Injured }
    /// <summary>
    /// The subset emotions containing tired feelings.
    /// </summary>
    public enum TiredFeelings { Indifferent, Bored, Sick, Weary, Powerless, Listless, Drained }
    /// <summary>
    /// The class which manifests the mood of the program.
    /// </summary>
    public class Mood
    {
        /// <summary>
        /// The aeon that is experiencing the emotion.
        /// </summary>
        public Aeon ThisAeon;
        /// <summary>
        /// The user that is associated with this aeon.
        /// </summary>
        public User ThisUser;
        /// <summary>
        /// Gets or sets the characteristic equation, which governs aeon's behaviour.
        /// </summary>
        /// <value>
        /// The characteristic equation.
        /// </value>
        public string CharacteristicEquation { get; set; }
        /// <summary>
        /// Gets or sets the polynomial expression used by the mood engine.
        /// </summary>
        private Polynomial MoodPolynomial { get; set; }
        /// <summary>
        /// The derivative of the mood polynomial.
        /// </summary>
        public Polynomial PolynomialDerivative { get; set; }
        /// <summary>
        /// The roots of the mood polynomial.
        /// </summary>
        public Complex[] PolynomialRoots { get; set; }
        /// <summary>
        /// Gets or sets the seed value for the mood engine randomizer.
        /// </summary>
        public int SeedValue { get; set; }
        /// <summary>
        /// Gets or sets the current mood of aeon.
        /// </summary>
        public static string CurrentMood { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [in love].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [in love]; otherwise, <c>false</c>.
        /// </value>
        public static bool InLove { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [intimate relationship].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [intimate relationship]; otherwise, <c>false</c>.
        /// </value>
        public static bool IntimateRelationship { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [friendly relationship].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [friendly relationship]; otherwise, <c>false</c>.
        /// </value>
        public static bool FriendlyRelationship { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [neutral relationship].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [neutral relationship]; otherwise, <c>false</c>.
        /// </value>
        public static bool NeutralRelationship { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [unfriendly relationship].
        /// </summary>
        /// <value>
        /// <c>true</c> if [unfriendly relationship]; otherwise, <c>false</c>.
        /// </value>
        public static bool UnfriendlyRelationship { get; set; }
        // For the next iteration of this implementation.
        /// <summary>
        /// Gets or sets a value indicating whether [affection detect].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [affection detect]; otherwise, <c>false</c>.
        /// </value>
        protected static bool AffectionDetect { get; set; }
        private static int AboutMe { get; set; }
        /// <summary>
        /// Gets or sets the emotive indication.
        /// </summary>
        public Indication EmotiveIndication { get; set; }
        /// <summary>
        /// Gets or sets the training error output from the indication computation.
        /// </summary>
        public double TrainingError { get; private set; }
        private double[,] EmotiveIndicationValue { get; set; }
        // The values for a random determination of mood.
        private static int WheelSpun { get; set; }
        private static int NextWheelPosition { get; set; }
        /// <summary>
        /// The limit of the randomizer.
        /// </summary>
        protected const int Limit = 20;
        /// <summary>
        /// Collects points for insult detection.
        /// </summary>
        /// <remarks>Emotional variety: the emotional weights.</remarks>
        public static int Insult { get; set; }
        /// <summary>
        /// Collects points for dislike detection.
        /// </summary>
        /// <remarks>Emotional variety: the emotional weights.</remarks>
        public static int Dislike { get; set; }
        /// <summary>
        /// Collects points for neutral detection.
        /// </summary>
        /// <remarks>Emotional variety: the emotional weights.</remarks>
        public static int Neutral { get; set; }
        /// <summary>
        /// Collects points for compliment detection.
        /// </summary>
        /// <remarks>Emotional variety: the emotional weights.</remarks>
        public static int Compliment { get; set; }
        /// <summary>
        /// Collects points for love detection.
        /// </summary>
        /// <remarks>Emotional variety: the emotional weights.</remarks>
        public static int Love { get; set; }
        /// <summary>
        /// Collects points for annoyance detection.
        /// </summary>
        /// <remarks>Variety variables for cases of repetition or uncharacteristic input from the user.</remarks>
        public static int Annoy { get; set; }
        /// <summary>
        /// Collects points for shocking detection.
        /// </summary>
        /// <remarks>Variety variables for cases of repetition or uncharacteristic input from the user.</remarks>
        public static int Shock { get; set; }
        /// <summary>
        /// Sets the emotional bias on mood creation.
        /// </summary>
        /// <value>Randomly determined</value>
        public Emotions EmotionBias { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Mood"/> class. Allows the program to express emotional variety to the user.
        /// </summary>
        /// <param name="seedValue">The seed value which controls random emotional expressions. Value should be between 0 and 20.</param>
        /// <param name="thisAeon">The aeon the emotion is attached to.</param>
        /// <param name="emotiveEquation">The characteristic equation for the emotive.</param>
        /// <remarks>This section of code contains what is called the "mood engine". The mood engine is designed, via the <see cref="StaticRandom"/> class, that low seed values perpetuate the happy moods, mid-range sadness (insecurity), and upper-range the more anti-social moods.</remarks>
        public Mood(int seedValue, Aeon thisAeon, string emotiveEquation)
        {
            ThisAeon = thisAeon;
            CharacteristicEquation = emotiveEquation;
            SeedValue = seedValue;
            EstablishMood();
            CheckIfInLove();
            ReturnIndication();// Here is where it will be interesting to extrapolate behaviour from code architecture abstractions.
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Mood"/> class. Allows the program to express emotional variety to the user.
        /// </summary>
        /// <param name="seedValue">The seed value which controls random emotional expressions. Value should be between 0 and 20.</param>
        /// <param name="thisAeon">The aeon the emotion is attached to.</param>
        /// <param name="thisUser">The user that is attached to the aeon.</param>
        /// <param name="emotiveEquation">The characteristic equation for the emotive.</param>
        /// <remarks>This section of code contains what is called the "mood engine". The mood engine is designed, via the <see cref="StaticRandom"/> class, that low seed values perpetuate the happy moods, mid-range sadness (insecurity), and upper-range the more anti-social moods.</remarks>
        public Mood(int seedValue, Aeon thisAeon, User thisUser, string emotiveEquation, Emotions bias)
        {
            ThisAeon = thisAeon;
            ThisUser = thisUser;
            CharacteristicEquation = emotiveEquation;
            SeedValue = seedValue;
            //EmotionBias = Extensions.Of<Emotions>();
            InterpretUserEmotion();
            EstablishMood();
            CheckIfInLove();
            ReturnIndication();// Here is where it will be interesting to extrapolate behaviour from code architecture abstractions.
        }
        /// <summary>
        /// Interpret the user's emotion from the file. An asterisk (*) is the default setting (neutral or undetectable).
        /// </summary>
        private void InterpretUserEmotion()
        {
            switch (ThisUser.Predicates.GrabSetting("emotion"))
            {
                case "*":
                    ThisUser.Emotion = UserEmotions.Neutral.ToString();
                    break;
                case "0":
                    ThisUser.Emotion = UserEmotions.Angry.ToString();
                    break;
                case "1":
                    ThisUser.Emotion = UserEmotions.Happy.ToString();
                    break;
                case "2":
                    ThisUser.Emotion = UserEmotions.Sad.ToString();
                    break;
            }
        }
        // Switches to determine class behaviour.
        /// <summary>
        /// Gets or sets a value indicating whether [store data] regarding the emotion.
        /// </summary>
        /// <value>
        ///   <c>true</c> if [store data]; otherwise, <c>false</c>.
        /// </value>
        public static bool StoreData { get; set; }
        /// <summary>
        /// Records the relationship outcome between the aeon and the user.
        /// </summary>
        /// <param name="sentence">The sentence being analyzed.</param>
        /// <remarks>Method used to construct the dataset. This is a file posited to the dataset directory.</remarks>
        public void RelationshipOutcome(string sentence)
        {   // Start with tones.
            if (sentence.Contains("how are you"))
            {
                AboutMe++;
                Compliment++;
            }
            if (sentence.Contains("I think"))
            {

            }
            if (sentence.Contains("do you"))
            {
                AboutMe++;
            }
            // Collect the instance variables of the sentence utterances.
            if (Compliment >= 10 && Love > 5)
                IntimateRelationship = true;
            if (Compliment.IsBetween(1, 9) & Love.IsBetween(1, 3))
                FriendlyRelationship = true;
            if (Compliment == 0 & Love == 0)
                NeutralRelationship = true;
            if (Compliment < 0 && Love < 0)
                UnfriendlyRelationship = true;

            DataOperations.StoreTrainingSet(ThisAeon);

        }
        /// <summary>
        /// Changes the mood of aeon.
        /// </summary>
        /// <param name="mood">The mood resulting from detecting emotional triggers from a that.</param>
        /// <remarks>What happens at each mood to those parts of the program when it is changed (for some reason)?</remarks>
        public void UpdateMood(Emotions mood)
        {
            switch (mood)
            {
                case Emotions.Confident:
                    CurrentMood = Emotions.Confident.ToString();
                    Love = Love + 1;
                    Neutral = Neutral + 1;
                    Compliment = Compliment + 1;
                    ReturnIndication();
                    break;
                case Emotions.Energized:
                    CurrentMood = Emotions.Energized.ToString();
                    Neutral = Neutral + 1;
                    Love = Love + 1;
                    ReturnIndication();
                    break;
                case Emotions.Hurt:
                    CurrentMood = Emotions.Hurt.ToString();
                    Love = Love - 1;
                    Neutral = Neutral - 1;
                    Compliment = Compliment - 1;
                    Dislike = Dislike + 2;
                    Insult = Insult + 2;
                    ReturnIndication();
                    break;
                case Emotions.Happy:
                    CurrentMood = Emotions.Happy.ToString();
                    Love = Love + 2;
                    Compliment = Compliment + 2;
                    ReturnIndication();
                    break;
                case Emotions.Helped:
                    CurrentMood = Emotions.Helped.ToString();
                    Neutral = Neutral + 1;
                    Compliment = Compliment + 1;
                    ReturnIndication();
                    break;
                case Emotions.Insecure:
                    CurrentMood = Emotions.Insecure.ToString();
                    Love = Love - 1;
                    Neutral = Neutral + 1;
                    Dislike = Dislike + 1;
                    ReturnIndication();
                    break;
                case Emotions.Sad:
                    CurrentMood = Emotions.Sad.ToString();
                    Love = Love - 1;
                    Neutral = Neutral - 1;
                    Dislike = Dislike + 1;
                    Insult = Insult + 1;
                    ReturnIndication();
                    break;
                case Emotions.Tired:
                    CurrentMood = Emotions.Tired.ToString();
                    Neutral = Neutral - 1;
                    Compliment = Compliment + 1;
                    ReturnIndication();
                    break;
            }
        }
        /// <summary>
        /// Changes the mood of the aeon.
        /// </summary>
        public void ChangeMood()
        {
            CurrentMood = "";
            SeedValue = StaticRandom.Next(0, Limit);
            EstablishMood();
        }
        /// <summary>
        /// Gets the current mood.
        /// </summary>
        /// <returns></returns>
        public string GetCurrentMood()
        {
            return CurrentMood;
        }
        /// <summary>
        /// Establishes the mood of the aeon.
        /// </summary>
        public void EstablishMood()
        {
            //if (seedValue > Limit) return;
            WheelSpun = StaticRandom.Next(0, Limit);
            NextWheelPosition = new Random(SeedValue).Next(Limit);
            // Emotional variety: a simple implementation.
            if (NextWheelPosition.IsBetween(0, 5) & WheelSpun.IsBetween(11, Limit))
            {
                CurrentMood = Emotions.Confident.ToString();
                Love = Love + 1;
                Neutral = Neutral + 1;
                Compliment = Compliment + 1;
            }
            if (NextWheelPosition.IsBetween(6, 15) & WheelSpun.IsBetween(0, 10))
            {
                CurrentMood = Emotions.Energized.ToString();
                Neutral += 1;
                Love = Love + 1;
            }
            if (NextWheelPosition.IsBetween(0, 5) & WheelSpun.IsBetween(0, 10))
            {
                CurrentMood = Emotions.Happy.ToString();
                Love = Love + 2;
                Compliment = Compliment + 2;
            }
            if (NextWheelPosition.IsBetween(6, 15) & WheelSpun.IsBetween(11, Limit))
            {
                CurrentMood = Emotions.Helped.ToString();
                Neutral = Neutral + 1;
                Compliment = Compliment + 1;
            }
            if (NextWheelPosition.IsBetween(16, Limit) & WheelSpun.IsBetween(11, Limit))
            {
                CurrentMood = Emotions.Hurt.ToString();
                Love = Love - 1;
                Neutral = Neutral - 1;
                Compliment = Compliment - 1;
                Dislike = Dislike + 2;
                Insult = Insult + 2;
            }
            if (NextWheelPosition.IsBetween(16, Limit) & WheelSpun.IsBetween(0, 5))
            {
                CurrentMood = Emotions.Insecure.ToString();
                Love = Love - 1;
                Neutral = Neutral + 1;
                Dislike = Dislike + 1;
            }
            if (NextWheelPosition.IsBetween(16, Limit) & WheelSpun.IsBetween(0, 10))
            {
                CurrentMood = Emotions.Happy.ToString();
                Love -= 1;
                Neutral -= 1;
                Dislike += 1;
                Insult +=  1;
            }
            if (NextWheelPosition.IsBetween(0, Limit) & WheelSpun.IsBetween(16, Limit))
            {
                CurrentMood = Emotions.Tired.ToString();
                Neutral = Neutral - 1;
                Compliment = Compliment + 1;
            }
        }
        /// <summary>
        /// Checks if aeon is in love.
        /// </summary>
        public bool CheckIfInLove()
        {
            if (Love > 10)
                return InLove = true;
            if (ThisUser.Emotion == Emotions.Happy.ToString())
                return InLove = true;
            return InLove = false;
        }
        /// <summary>
        /// Return the (emotive) indication.
        /// </summary>
        /// <remarks>Trajectory indication in <see cref="Result"/>, emotive indication here.</remarks>
        public void ReturnIndication()
        {
            // Using a simple polynomial expression to create maps in the "brain" and assign it either an emotive or trajectory indication.
            EmotiveIndication = new Indication(Boagaphish.Numeric.TransferFunction.BipolarSigmoid, EmotivePolynomial())
            {
                WindowSize = Convert.ToInt32(ThisAeon.GlobalSettings.GrabSetting("windowsize")),
                SigmoidAlpha = Convert.ToDouble(ThisAeon.GlobalSettings.GrabSetting("sigmoidalpha")),
                Iterations = Convert.ToInt32(ThisAeon.GlobalSettings.GrabSetting("iterations"))
            };
            // NOTE: Using an aggressive solution search for the trajectory indication.
            TrainingError = EmotiveIndication.TrainNetwork();
            // Assign default values to the emotive indication. Non-GPU.
            EmotiveIndicationValue = new double[EmotiveIndication.Iterations - EmotiveIndication.WindowSize, 2];
            EmotiveIndicationValue = EmotiveIndication.SearchSolution();
        }
        /// <summary>
        /// I think a solution is to cast a characteristic polynomial in the space given by the trajectory, the x-axis in a virtual space.
        /// </summary>
        public Polynomial TrajectoryPolynomial()
        {
            MoodPolynomial = new Polynomial(CharacteristicEquation);
            var polynomial = MoodPolynomial.LocalPolynomial;
            return polynomial;
        }
        /// <summary>
        /// I think a solution is to cast a characteristic polynomial in the space given by the emotive, the y-axis in a virtual space.
        /// </summary>
        public Polynomial EmotivePolynomial()
        {
            MoodPolynomial = new Polynomial(CharacteristicEquation);
            var polynomial = MoodPolynomial.LocalPolynomial;
            return polynomial;
        }
        /// <summary>
        /// Gets the current mood polynomial.
        /// </summary>
        /// <returns></returns>
        public string ReturnMoodPolynomialProperties()
        {
            PolynomialRoots = PolynomialProperties.Roots;
            PolynomialDerivative = PolynomialProperties.Derivative;
            return MoodPolynomial.LocalPolynomial.ToString();
        }
    }
}
