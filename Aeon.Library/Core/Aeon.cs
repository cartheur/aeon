//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
using System.Globalization;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Xml;

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
        /// Gets or sets the name.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Drawing 100, Feature 104. A dictionary object that looks after all the settings associated with this aeon.
        /// </summary>
        public SettingsDictionary GlobalSettings;
        /// <summary>
        /// Drawing 100, Feature 104. A dictionary of first to third-person substitutions.
        /// </summary>
        public SettingsDictionary PersonSubstitutions;
        /// <summary>
        /// Drawing 100, Feature 104. Generic substitutions that take place during the normalization process.
        /// </summary>
        public SettingsDictionary Substitutions;
        /// <summary>
        /// Drawing 100, Feature 104. The default predicates to set up for a participant.
        /// </summary>
        public SettingsDictionary DefaultPredicates;
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
        /// Holds information about the available custom tag handling classes (if loaded).
        /// <param>The class name.
        ///     <name>Key</name>
        /// </param>
        /// <param>The TagHandler class that provides information about the class.
        ///     <name>Value</name>
        /// </param>
        /// </summary>
        private Dictionary<string, TagHandler> _customTags;
        /// <summary>
        /// Holds references to the assemblies that hold the custom tag handling code.
        /// </summary>
        private readonly Dictionary<string, Assembly> _lateBindingAssemblies = new Dictionary<string, Assembly>();
        /// <summary>
        /// Flag to show if aeon is accepting participant input.
        /// </summary>
        public bool IsAcceptingParticipantInput = true;
        /// <summary>
        /// The number of categories aeon has in her brain.
        /// </summary>
        public int Size;
        /// <summary>
        /// If set to false the input from aeon code files will undergo the same normalization process that participant input goes through. If true aeon will assume the code structure is correct. Defaults to true.
        /// </summary>
        public bool TrustCodeFiles = true;
        /// <summary>
        /// The maximum number of characters a "that" element of a path is allowed to be. Anything above this length will cause "that" to be "*". This is to avoid having the core process huge "that" elements in the path that might have been caused by aeon reporting third party data.
        /// </summary>
        public int MaxThatSize = 256;
        /// <summary>
        /// The maximum amount of time a request should take (in milliseconds).
        /// </summary>
        public double TimeOut
        {
            get
            {
                return Convert.ToDouble(GlobalSettings.GrabSetting("timeout"));
            }
        }
        /// <summary>
        /// The message to display in the event of a timeout.
        /// </summary>
        public string TimeOutMessage
        {
            get
            {
                return GlobalSettings.GrabSetting("timeoutmessage");
            }
        }
        /// <summary>
        /// The locale of aeon as a CultureInfo object.
        /// </summary>
        public CultureInfo Locale
        {
            get
            {
                return new CultureInfo(GlobalSettings.GrabSetting("culture"));
            }
        }
        /// <summary>
        /// Will match all the illegal characters that might be input by the participant.
        /// </summary>
        public Regex Strippers
        {
            get
            {
                return new Regex(GlobalSettings.GrabSetting("stripperregex"), RegexOptions.IgnorePatternWhitespace);
            }
        }
        /// <summary>
        /// The email address to correspond with.
        /// </summary>
        public string AdminEmail
        {
            get
            {
                return GlobalSettings.GrabSetting("adminemail");
            }
            set
            {
                if (value.Length > 0)
                {
                    // Check that the email adress is in a valid format.
                    const string patternStrict = @"^(([^<>()[\]\\.,;:\s@\""]+"
                                                 + @"(\.[^<>()[\]\\.,;:\s@\""]+)*)|(\"".+\""))@"
                                                 + @"((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}"
                                                 + @"\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+"
                                                 + @"[a-zA-Z]{2,}))$";
                    Regex reStrict = new Regex(patternStrict);

                    if (reStrict.IsMatch(value))
                    {
                        // Update settings.
                        GlobalSettings.AddSetting("adminemail", value);
                    }
                    else
                    {
                        throw (new Exception("The AdminEmail is not a valid email address"));
                    }
                }
                else
                {
                    GlobalSettings.AddSetting("adminemail", "");
                }
            }
        }
        /// <summary>
        /// Indicates if the aeon is in an error state.
        /// </summary>
        public bool ErrorState { get; set; }
        /// <summary>
        /// The splitters object.
        /// </summary>
        public List<string> Splitters = new List<string>();
        /// <summary>
        /// The brain of aeon.
        /// </summary>
        public Node ThisNode;
        /// <summary>
        /// Determines if learning is active or not.
        /// </summary>
        public enum LearningMode
        {
            /// <summary>
            /// Learning mode is true.
            /// </summary>
            True,
            /// <summary>
            /// Learning mode is false.
            /// </summary>
            False
        };
        /// <summary>
        /// The last message to be entered into the log (for testing purposes)
        /// </summary>
        public string LastLogMessage = string.Empty;
        /// <summary>
        /// Returns the persistence of the aeon (in milliseconds).
        /// </summary>
        public int Persistence()
        {
            if (PersonalityLoaded)
                return (DateTime.Now - AeonStartedOn).Milliseconds;
            return 0;
        }
        /// <summary>
        /// Gets or sets a value indicating whether [emotion used].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [emotion used]; otherwise, <c>false</c>.
        /// </value>
        public bool EmotionUsed { get; set; }
        /// <summary>
        /// Flag to indicate if a personality is loaded.
        /// </summary>
        public static bool PersonalityLoaded;
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

        #region Settings and load methods
        /// <summary>
        /// Allows aeon to load a new xml version of some aeon xms data.
        /// </summary>
        /// <param name="xmlCode">The xml document containing xms.</param>
        /// <param name="filename">The originator of the xml document.</param>
        public void LoadAeonFromXml(XmlDocument xmlCode, string filename)
        {
            AeonLoader loader = new AeonLoader(this);
            loader.LoadAeonFromXml(xmlCode, filename);
        }
        /// <summary>
        /// Loads settings and configuration info from various xml files referenced in the settings file passed in the args. Also generates some default values if such values have not been set by the settings file.
        /// </summary>
        /// <param name="configurationPath">Path to the xml configuration file.</param>
        public void LoadSettings(string configurationPath)
        {
            GlobalSettings.LoadSettings(configurationPath);

            // Checks for some critical default settings.
            if (!GlobalSettings.ContainsSettingCalled("version"))
            {
                GlobalSettings.AddSetting("version", Environment.Version.ToString());
            }
            if (!GlobalSettings.ContainsSettingCalled("name"))
            {
                GlobalSettings.AddSetting("name", "aeon");
            }
            if (!GlobalSettings.ContainsSettingCalled("botmaster"))
            {
                GlobalSettings.AddSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("master"))
            {
                GlobalSettings.AddSetting("botmaster", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("author"))
            {
                GlobalSettings.AddSetting("author", "malfactor");
            }
            if (!GlobalSettings.ContainsSettingCalled("location"))
            {
                GlobalSettings.AddSetting("location", "Unknown");
            }
            if (!GlobalSettings.ContainsSettingCalled("gender"))
            {
                GlobalSettings.AddSetting("gender", "-1");
            }
            if (!GlobalSettings.ContainsSettingCalled("birthday"))
            {
                GlobalSettings.AddSetting("birthday", "2015/09/09");
            }
            if (!GlobalSettings.ContainsSettingCalled("birthplace"))
            {
                GlobalSettings.AddSetting("birthplace", "somewhere");
            }
            if (!GlobalSettings.ContainsSettingCalled("website"))
            {
                GlobalSettings.AddSetting("website", "https://emotional.toys");
            }
            if (GlobalSettings.ContainsSettingCalled("adminemail"))
            {
                string emailToCheck = GlobalSettings.GrabSetting("adminemail");
                AdminEmail = emailToCheck;
            }
            else
            {
                GlobalSettings.AddSetting("adminemail", "");
            }
            if (!GlobalSettings.ContainsSettingCalled("islogging"))
            {
                GlobalSettings.AddSetting("islogging", "true");
            }
            if (!GlobalSettings.ContainsSettingCalled("willcallhome"))
            {
                GlobalSettings.AddSetting("willcallhome", "False");
            }
            if (!GlobalSettings.ContainsSettingCalled("timeout"))
            {
                GlobalSettings.AddSetting("timeout", "2000");
            }
            if (!GlobalSettings.ContainsSettingCalled("timeoutmessage"))
            {
                GlobalSettings.AddSetting("timeoutmessage", "The request has timed out.");
            }
            if (!GlobalSettings.ContainsSettingCalled("culture"))
            {
                GlobalSettings.AddSetting("culture", "en-UK");
            }
            if (!GlobalSettings.ContainsSettingCalled("splittersfile"))
            {
                GlobalSettings.AddSetting("splittersfile", "Splitters.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("personsubstitutionsfile"))
            {
                GlobalSettings.AddSetting("personsubstitutionsfile", "PersonSubstitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("defaultpredicates"))
            {
                GlobalSettings.AddSetting("defaultpredicates", "DefaultPredicates.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("substitutionsfile"))
            {
                GlobalSettings.AddSetting("substitutionsfile", "Substitutions.xml");
            }
            if (!GlobalSettings.ContainsSettingCalled("configdirectory"))
            {
                GlobalSettings.AddSetting("configdirectory", "config");
            }
            if (!GlobalSettings.ContainsSettingCalled("logdirectory"))
            {
                GlobalSettings.AddSetting("logdirectory", "logs");
            }
            if (!GlobalSettings.ContainsSettingCalled("maxlogbuffersize"))
            {
                GlobalSettings.AddSetting("maxlogbuffersize", "64");
            }
            if (!GlobalSettings.ContainsSettingCalled("notacceptingparticipantinputmessage"))
            {
                GlobalSettings.AddSetting("notacceptingparticipantinputmessage", "Aeon is not accepting participant input.");
            }
            if (!GlobalSettings.ContainsSettingCalled("stripperregex"))
            {
                GlobalSettings.AddSetting("stripperregex", "[^0-9a-zA-Z]");
            }
            if (!GlobalSettings.ContainsSettingCalled("password"))
            {
                GlobalSettings.AddSetting("password", "XhUkIjUnYvTqIjUj");
            }
        }
        /// <summary>
        /// Updates a settings entry.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void UpdateSetting(string key, string value)
        {
            GlobalSettings.UpdateSetting(key, value);
        }
        /// <summary>
        /// Adds an entry to settings.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void AddSetting(string key, string value)
        {
            GlobalSettings.AddSetting(key, value);
        }
        /// <summary>
        /// Loads splitters for this aeon from the supplied config file (or sets up some appropriate defaults).
        /// </summary>
        /// <param name="splittersPath">Path to the splitters configuration file.</param>
        public void LoadSplitters(string splittersPath)
        {
            FileInfo splittersFile = new FileInfo(splittersPath);
            if (splittersFile.Exists)
            {
                XmlDocument splittersXmlDoc = new XmlDocument();
                splittersXmlDoc.Load(splittersPath);
                // The XML should have an XML declaration like this:
                // <?xml version="1.0" encoding="utf-8" ?> 
                // followed by a <root> tag with children of the form:
                // <item value="value"/>
                if (splittersXmlDoc.ChildNodes.Count == 2)
                {
                    if (splittersXmlDoc.LastChild.HasChildNodes)
                    {
                        foreach (XmlNode myNode in splittersXmlDoc.LastChild.ChildNodes)
                        {
                            if (myNode.Attributes != null && (myNode.Name == "item") & (myNode.Attributes.Count == 1))
                            {
                                string value = myNode.Attributes["value"].Value;
                                Splitters.Add(value);
                            }
                        }
                    }
                }
            }
            if (Splitters.Count == 0)
            {
                // We don't have any splitters, so lets make do with the following.
                Splitters.Add(".");
                Splitters.Add("!");
                Splitters.Add("?");
                Splitters.Add(";");
            }
        }
        #endregion
    }
}