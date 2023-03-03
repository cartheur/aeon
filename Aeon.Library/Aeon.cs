//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
namespace Aeon.Library
{
    /// <summary>
    /// Drawing 700, Feature 713. The area of the program where thematic metaphors are assigned.
    /// </summary>
    public enum Characteristic
    {
        /// <summary>
        /// The attention characteristic.
        /// </summary>
        Attention,
        /// <summary>
        /// The concept characteristic.
        /// </summary>
        Concept,
        /// <summary>
        /// The decision characteristic.
        /// </summary>
        Decision,
        /// <summary>
        /// The drive characteristic.
        /// </summary>
        Drive,
        /// <summary>
        /// The noun characteristic.
        /// </summary>
        Noun,
        /// <summary>
        /// The perception characteristic.
        /// </summary>
        Perception,
        /// <summary>
        /// The predicate characteristic.
        /// </summary>
        Predicate,
        /// <summary>
        /// The sensory characteristic.
        /// </summary>
        Sensory,
        /// <summary>
        /// The stimulus characteristic.
        /// </summary>
        Stimulus,
        /// <summary>
        /// The verb characteristic.
        /// </summary>
        Verb
    }
    public class Aeon
    {
        /// <summary>
        /// Drawing 100, Feature 104. A dictionary object that looks after all the settings associated with this aeon.
        /// </summary>
        public SettingsDictionary GlobalSettings;
        /// <summary>
        /// Drawing 100, Feature 102. The time when this aeon was started.
        /// </summary>
        public DateTime AeonStartedOn;
        /// <summary>
        /// Drawing 600, Feature 601. The time when the alone time started.
        /// </summary>
        public DateTime AeonAloneStartedOn { get; set; }
        /// <summary>
        /// Drawing 600, Feature 603. The timer responsible for determining to trigger and event that the aeon is alone.
        /// </summary>
        public Timer AeonAloneTimer { get; set; }
        /// <summary>
        /// Drawing 700, Feature 710. The characteristic equation that will govern the aeon's runtime behavior.
        /// </summary>
        public string CharacteristicEquation { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Aeon" /> class.
        /// </summary>
        /// <param name="characteristicEquation">The characteristic equation governing the aeon.</param>
        public Aeon(string characteristicEquation)
        {
            CharacteristicEquation = characteristicEquation;
            Setup();
        }

        void Setup()
        {
            GlobalSettings = new SettingsDictionary(this);
            AeonStartedOn = DateTime.Now;
        }

        #region Drawing 600, Feature 603. The Alone Feature

        /// <summary>
        /// Gets the alone threshold.
        /// </summary>
        public TimeSpan AloneThreshold
        {
            get
            {
                return new TimeSpan(0, 0, 0, 0, Convert.ToInt32(GlobalSettings.GrabSetting("alonethreshold")));
            }
        }
        /// <summary>
        /// Computes the alone time duration.
        /// </summary>
        /// <returns>The duration.</returns>
        public TimeSpan AeonAloneDuration()
        {
            return (DateTime.Now - AeonAloneStartedOn);
        }
        /// <summary>
        /// Determines whether the aeon is alone.
        /// </summary>
        /// <returns>If this aeon is feeling alone.</returns>
        public bool IsAlone()
        {
            if (AeonAloneDuration() > AloneThreshold)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}