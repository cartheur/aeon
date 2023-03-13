//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
using System.Collections.Generic;
using System.Drawing;
using System;
using System.Globalization;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using static System.Net.Mime.MediaTypeNames;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /// Gets or sets the name of the aeon.
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
        public System.Timers.Timer AeonAloneTimer { get; set; }
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
        /// Flag to show if aeon is accepting input.
        /// </summary>
        public bool IsAcceptingInput = true;
        /// <summary>
        /// The message to show if a participant tries to use aeon whilst set to not process participant input.
        /// </summary>
        private string NotAcceptingInputMessage
        {
            get
            {
                return GlobalSettings.GrabSetting("notacceptinginputmessage");
            }
        }
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
            DefaultPredicates = new SettingsDictionary(this);
            GlobalSettings = new SettingsDictionary(this);
            PersonSubstitutions = new SettingsDictionary(this);
            Substitutions = new SettingsDictionary(this);
            _customTags = new Dictionary<string, TagHandler>();
            ThisNode = new Node();
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
                GlobalSettings.AddSetting("botmaster", "cartheur");
            }
            if (!GlobalSettings.ContainsSettingCalled("master"))
            {
                GlobalSettings.AddSetting("botmaster", "cartheur");
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

        #region Conversation methods
        /// <summary>
        /// Given some raw input and a unique ID creates a response for a new participant.
        /// </summary>
        /// <param name="rawInput">the raw input.</param>
        /// <param name="participantGuid">The ID for the participant (referenced in the result object).</param>
        /// <returns>Result to the participant.</returns>
        public ParticipantResult Chat(string rawInput, string participantGuid)
        {
            ParticipantRequest request = new ParticipantRequest(rawInput, new Participant(participantGuid, this), this);
            return Chat(request);
        }
        /// <summary>
        /// Given a request containing participant input, produces a result from aeon.
        /// </summary>
        /// <param name="request">The request from the participant.</param>
        /// <returns>The result to be output to the participant.</returns>
        public ParticipantResult Chat(ParticipantRequest request)
        {
            var result = new ParticipantResult(request.ThisParticipant, this, request, CharacteristicEquation);
            // Todo: Set the emotion, where used. It is now known to the core.
            //result.ThisUser.Predicates.UpdateSetting("EMOTION", Mood.CurrentMood);

            if (IsAcceptingInput)
            {
                // Normalize the input.
                AeonLoader loader = new AeonLoader(this);
                SplitIntoSentences splitter = new SplitIntoSentences(this);
                string[] rawSentences = splitter.Transform(request.RawInput);
                foreach (string sentence in rawSentences)
                {
                    result.InputSentences.Add(sentence);
                    string trajectoryGenerated;
                    if (EmotionUsed)
                    {
                        trajectoryGenerated = loader.GenerateTrajectory(sentence, request.ThisParticipant.GetLastAeonOutput(), request.ThisParticipant.Topic, request.ThisParticipant.Emotion, true);
                        result.NormalizedTrajectories.Add(trajectoryGenerated);
                    }
                    else
                    {
                        trajectoryGenerated = loader.GenerateTrajectory(sentence, request.ThisParticipant.GetLastAeonOutput(), request.ThisParticipant.Topic, true);
                        result.NormalizedTrajectories.Add(trajectoryGenerated);
                    }

                }
                // Grab the templates for the various sentences.
                foreach (string trajectory in result.NormalizedTrajectories)
                {
                    ParticipantQuery query = new ParticipantQuery(trajectory);
                    query.Template = ThisNode.Evaluate(trajectory, query, request, MatchState.ParticipantInput, new StringBuilder());
                    result.SubQueries.Add(query);
                }
                // Process the templates into appropriate output.
                foreach (ParticipantQuery query in result.SubQueries)
                {
                    if (query.Template.Length > 0)
                    {
                        try
                        {
                            XmlNode templateNode = AeonHandler.GetNode(query.Template);
                            string outputSentence = ProcessNode(templateNode, query, request, result, request.ThisParticipant);
                            // Integrate the learned output with this query response.
                            if (outputSentence.Length > 0)
                            {
                                result.OutputSentences.Add(outputSentence);
                            }
                        }
                        catch (Exception ex)
                        {
                            Logging.WriteLog("A problem was encountered when trying to process the input: " + request.RawInput + " with the template: \"" + query.Template + ". The following exception message was noted: " + ex.Message, Logging.LogType.Warning, Logging.LogCaller.Aeon);
                        }
                    }
                }
            }
            else
            {
                result.OutputSentences.Add(NotAcceptingInputMessage);
            }
            // Return the indication from the process.
            result.ReturnIndication();
            // Populate the result object and note the performance.
            result.Duration = DateTime.Now - request.StartedOn;
            request.ThisParticipant.AddResult(result);

            return result;
        }
        /// <summary>
        /// Recursively evaluates the template nodes returned from aeon.
        /// </summary>
        /// <param name="node">The node to evaluate.</param>
        /// <param name="query">The query that produced this node.</param>
        /// <param name="request">The request from the participant.</param>
        /// <param name="result">The result to be sent to the participant.</param>
        /// <param name="participant">The participant who originated the request.</param>
        /// <returns>The output string.</returns>
        protected string ProcessNode(XmlNode node, ParticipantQuery query, ParticipantRequest request, ParticipantResult result, Participant participant)
        {
            // Check for timeout (to avoid infinite loops).
            if (request.StartedOn.AddMilliseconds(request.ThisAeon.TimeOut) < DateTime.Now)
            {
                Logging.WriteLog("Request timeout. Participant: " + request.ThisParticipant.Name + " raw input: \"" + request.RawInput + "\" processing template: \"" + query.Template + "\"", Logging.LogType.Warning, Logging.LogCaller.Aeon);
                request.HasTimedOut = true;
                return string.Empty;
            }

            // Process the node.
            string tagName = node.Name.ToLower();
            if (tagName == "template")
            {
                StringBuilder templateResult = new StringBuilder();
                if (node.HasChildNodes)
                {
                    // Recursively check. Stepping in here, how does it get four child nodes after parsing only one?
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        templateResult.Append(ProcessNode(childNode, query, request, result, participant));
                        // Does this really step to the next child node?
                    }
                }
                return templateResult.ToString();
            }
            AeonHandler tagHandler = GetBespokeTags(participant, query, request, result, node);

            if (Equals(null, tagHandler))
            {
                switch (tagName)
                {
                    case "condition":
                        tagHandler = new Condition(this, participant, query, request, result, node);
                        break;
                    case "date":
                        tagHandler = new Date(this, participant, query, request, result, node);
                        break;
                    case "formal":
                        tagHandler = new Formal(this, participant, query, request, result, node);
                        break;
                    case "gender":
                        tagHandler = new Gender(this, participant, query, request, result, node);
                        break;
                    case "get":
                        tagHandler = new Get(this, participant, query, request, result, node);
                        break;
                    case "gossip":
                        tagHandler = new Gossip(this, participant, query, request, result, node);
                        break;
                    case "id":
                        tagHandler = new Id(this, participant, query, request, result, node);
                        break;
                    case "input":
                        tagHandler = new Input(this, participant, query, request, result, node);
                        break;
                    case "learn":
                        tagHandler = new Learn(this, participant, query, request, result, node);
                        break;
                    case "lowercase":
                        tagHandler = new Lowercase(this, participant, query, request, result, node);
                        break;
                    case "person":
                        tagHandler = new Person(this, participant, query, request, result, node);
                        break;
                    case "person2":
                        tagHandler = new Person2(this, participant, query, request, result, node);
                        break;
                    case "presence":
                        tagHandler = new Presence(this, participant, query, request, result, node);
                        break;
                    case "random":
                        tagHandler = new RandomTag(this, participant, query, request, result, node);
                        break;
                    case "sentence":
                        tagHandler = new Sentence(this, participant, query, request, result, node);
                        break;
                    case "set":
                        tagHandler = new Set(this, participant, query, request, result, node);
                        break;
                    case "size":
                        tagHandler = new Size(this, participant, query, request, result, node);
                        break;
                    case "sr":
                        tagHandler = new Sr(this, participant, query, request, result, node);
                        break;
                    case "srai":
                        tagHandler = new Srai(this, participant, query, request, result, node);
                        break;
                    case "star":
                        tagHandler = new Star(this, participant, query, request, result, node);
                        break;
                    case "test":
                        tagHandler = new Test();
                        break;
                    case "that":
                        tagHandler = new That(this, participant, query, request, result, node);
                        break;
                    case "thatstar":
                        tagHandler = new ThatStar(this, participant, query, request, result, node);
                        break;
                    case "think":
                        tagHandler = new Think(this, participant, query, request, result, node);
                        break;
                    case "topicstar":
                        tagHandler = new TopicStar(this, participant, query, request, result, node);
                        break;
                    case "uppercase":
                        tagHandler = new Uppercase(this, participant, query, request, result, node);
                        break;
                    case "version":
                        tagHandler = new Version(this, participant, query, request, result, node);
                        break;
                }
            }
            if (Equals(null, tagHandler))
            {
                return node.InnerText;
            }
            if (tagHandler.IsRecursive)
            {
                if (node.HasChildNodes)
                {
                    // Recursively check.
                    foreach (XmlNode childNode in node.ChildNodes)
                    {
                        if (childNode.NodeType != XmlNodeType.Text)
                        {
                            childNode.InnerXml = ProcessNode(childNode, query, request, result, participant);
                        }
                    }
                }
                return tagHandler.Transform();
            }
            string resultNodeInnerXml = tagHandler.Transform();
            XmlNode resultNode = AeonHandler.GetNode("<node>" + resultNodeInnerXml + "</node>");
            if (resultNode.HasChildNodes)
            {
                StringBuilder recursiveResult = new StringBuilder();
                // Recursively check.
                foreach (XmlNode childNode in resultNode.ChildNodes)
                {
                    recursiveResult.Append(ProcessNode(childNode, query, request, result, participant));
                }
                return recursiveResult.ToString();
            }
            return resultNode.InnerXml;
        }
        /// <summary>
        /// Searches the custom tag collection and processes the aeon files if an appropriate tag handler is found.
        /// </summary>
        /// <param name="participant">The participant who originated the request.</param>
        /// <param name="participantQuery">The query that produced this node.</param>
        /// <param name="request">The request from the participant.</param>
        /// <param name="result">The result to be sent to the participant.</param>
        /// <param name="node">The node to evaluate.</param>
        /// <returns>The output string.</returns>
        public AeonHandler GetBespokeTags(Participant participant, ParticipantQuery participantQuery, ParticipantRequest request, ParticipantResult result, XmlNode node)
        {
            if (_customTags.ContainsKey(node.Name.ToLower()))
            {
                TagHandler customTagHandler = _customTags[node.Name.ToLower()];
                AeonHandler newCustomTag = customTagHandler.Instantiate(_lateBindingAssemblies);
                if (Equals(null, newCustomTag))
                {
                    return null;
                }
                newCustomTag.ThisParticipant = participant;
                newCustomTag.ParticipantQuery = participantQuery;
                newCustomTag.ParticipantRequest = request;
                newCustomTag.ParticipantResult = result;
                newCustomTag.TemplateNode = node;
                newCustomTag.ThisAeon = this;
                return newCustomTag;
            }
            return null;
        }
        #endregion
    }
}