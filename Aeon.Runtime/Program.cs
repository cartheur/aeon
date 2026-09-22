//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
// Learning mode is active.
//
using Aeon.Library;
using System.Diagnostics;
using System.Media;
using System.Runtime.Versioning;
using System.Timers;

namespace Aeon.Runtime
{
    internal class Program
    {
        // Configuration of the application.
        public static LoaderPaths Configuration;
        public static AeonLoader AeonLoader;
        public static bool StartUpTheme { get; set; }
        public static string StartUpThemeFile { get; set; }
        public static bool TerminalMode { get; set; }
        public static Process TerminalProcess { get; set; }
        // Aeon's personal procedural items.
        private static Library.Aeon _thisAeon;
        private static Library.Builder.ExtendSystem _thisConfig;
        private static Participant _thisParticipant;
        private static ParticipantRequest _thisRequest;
        static ParticipantResult _thisResult;
        private static DateTime _aeonChatStartedOn;
        private static TimeSpan _aeonChatDuration;
        private static Thread _aeonAloneThread;
        private static int _instance = 1;
        // Aeon's status.
        private static bool SettingsLoaded { get; set; }
        static bool LearningModeActive { get; set; }
        private static bool AeonLoaded { get; set; }
        public static string ParticipantInput { get; set; }
        public static string AeonResult { get; set; }
        public static string AeonOutputDebug { get; set; }
        public static int AloneMessageOutput { get; set; }
        public static int PreviousAloneMessageOutput { get; set; }
        public static int AloneMessageVariety { get; set; }
        static string LastOutput { get; set; }
        private static string EmotiveEquation { get; set; }
        public int AeonSize { get; set; }
        public static string AeonType { get; set; }
        public static bool AeonIsAlone { get; set; }
        public static string AloneTextCurrent { get; set; }
        private static readonly OutputDispatcher Output = new OutputDispatcher(
            new IOutputAdapter[] { new TerminalOutputAdapter() },
            (adapter, exception) => Logging.WriteLog("Output adapter " + adapter.GetType().Name + " failed: " + exception.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime));
        private static bool VoiceOutputEnabled { get; set; }

        static async Task Main(string[] args)
        {
            Configuration = new LoaderPaths("Debug");
            Logging.ActiveConfiguration = Configuration.ActiveRuntime;
            // Create the aeon and load its basic parameters from a config file.
            _thisAeon = new Library.Aeon("1+2i");
            AeonLoader = new AeonLoader(_thisAeon);
            try
            {
                await Task.Run(() => _thisAeon.LoadSettings(Configuration.PathToSettings));
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine("Aeon could not load its settings: " + ex.Message);
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                Environment.ExitCode = 1;
                return;
            }
            SettingsLoaded = await Task.Run(() => _thisAeon.LoadDictionaries(Configuration));
            if (!SettingsLoaded)
            {
                Console.Error.WriteLine("Aeon could not load its language configuration. See the log for details.");
                Environment.ExitCode = 1;
                return;
            }
            _thisParticipant = new Participant(_thisAeon.GlobalSettings.GrabSetting("participantname"), _thisAeon);
            LearningModeActive = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("learningmodeactive"));
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("product") + " - Version " + _thisAeon.GlobalSettings.GrabSetting("version") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("ip") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("claim") + ".");
            Thread.Sleep(700);
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("warning"));
            Thread.Sleep(700);
            Console.WriteLine("------ Begin help ------");
            Console.WriteLine("While in terminal mode, type 'exit' to quit the application.");
            Console.WriteLine("Type 'quit' to leave terminal mode.");
            Console.WriteLine("------ End help------");
            Thread.Sleep(700);
            Console.WriteLine("Continuing to construct the personality...");
            // Check that the aeon launch is valid.
            ParticipantInput = "";
            _thisAeon.Name = _thisAeon.GlobalSettings.GrabSetting("name");
            _thisAeon.EmotionUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("emotionused"));
            StartUpTheme = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("startuptheme"));
            StartUpThemeFile = _thisAeon.GlobalSettings.GrabSetting("startupthemefile");
            EmotiveEquation = _thisAeon.GlobalSettings.GrabSetting("emotiveequation");
            _thisAeon.CharacteristicEquation = EmotiveEquation;
            TerminalMode = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("terminalmode"));
            ConfigureSpeechOutput();
            // Initialize the alone feature.
            _thisAeon.AeonAloneTimer = new System.Timers.Timer();
            _thisAeon.AeonAloneTimer.Elapsed += AloneEvent;
            _thisAeon.AeonAloneTimer.Interval = Convert.ToDouble(_thisAeon.GlobalSettings.GrabSetting("alonetimecheck"));
            _thisAeon.AeonAloneTimer.Enabled = false;
            _aeonAloneThread = new Thread(AeonAloneText);
            SharedFunctions.ThisAeon = _thisAeon;
            // Utilize the correct settings based on the aeon personality.
            if (_thisAeon.Name == "Rhodo" && SettingsLoaded)
            {
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
                _thisConfig = new Library.Builder.ExtendSystem(Configuration);
            }
            if (_thisAeon.Name == "Blank" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadBlank(Configuration);
            if (_thisAeon.Name == "Samantha" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (!AeonLoaded)
            {
                Console.Error.WriteLine("Aeon could not load its personality. See the log for details.");
                Environment.ExitCode = 1;
                return;
            }
            // Attach logging,  xms functionality, and spontaneous file generation.
            Logging.LogModelFile = _thisAeon.GlobalSettings.GrabSetting("logmodelfile");
            Logging.TranscriptModelFile = _thisAeon.GlobalSettings.GrabSetting("transcriptmodelfile");
            LoadTrajectoryHistory();
            LoadEmotiveWeights();
            // Set the aeon type by personality.
            switch (_thisAeon.Name)
            {
                case "Rhodo":
                    AeonType = "Default";
                    break;
                case "Samantha":
                    AeonType = "Friendly";
                    break;
                case "Blank":
                    AeonType = "Empty";
                    break;
            }
            Console.WriteLine("The aeon type is " + AeonType.ToLower() + ".");
            Console.WriteLine("Personality construction completed.");
            Console.WriteLine("A presence named '" + _thisAeon.Name + "' has been initialized.");
            Console.WriteLine("It has " + _thisAeon.Size + " categories available in its mind.");
            // Set final parameters and play the welcome message.
            if (StartUpTheme)
            {
                PlayStartupTheme();
            }
            Console.WriteLine("Your aeon is ready for an interaction with you.");
            Console.WriteLine("Your transcript follows. Enjoy!");
            Console.WriteLine("**********************");
            if (TerminalMode)
            {
                Console.WriteLine("You have selected terminal mode.");
                if (_thisAeon.Size < 2)
                {
                    await Task.Run(() => _thisAeon.LoadBlank(Configuration));
                    Console.WriteLine("No personality files have been loaded. A blank aeon has been loaded in its place.");
                }

                // Todo: Trigger the terminal for typing commands.
                while (true)
                {
                    ParticipantInput = Console.ReadLine();
                    if (ParticipantInput is null)
                    {
                        break;
                    }
                    ParticipantInput = ParticipantInput.Trim();
                    if (ParticipantInput.Equals("exit", StringComparison.OrdinalIgnoreCase))
                    {
                        break;
                    }
                    if (ParticipantInput.Equals("quit", StringComparison.OrdinalIgnoreCase))
                    {
                        Console.WriteLine("Leaving terminal mode.");
                        break;
                    }
                    if (LearningModeActive && ParticipantInput.Equals("learn", StringComparison.OrdinalIgnoreCase))
                    {
                        await LearningMode();
                        continue;
                    }
                    if (ParticipantInput.Length > 0)
                        await ProcessTerminal();
                }
            }
        }

        static async Task<bool> ProcessTerminal()
        {
            await ProcessInput();
            return true;
        }
        static void LoadTrajectoryHistory()
        {
            try
            {
                string path = Configuration.PathToTrajectoryHistory(_thisParticipant.Name);
                if (_thisParticipant.TrajectoryHistory.Load(path))
                {
                    Logging.WriteLog("Loaded " + _thisParticipant.TrajectoryHistory.Indications.Count + " trajectory indications.", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog("Could not load trajectory history: " + ex.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
            }
        }
        static void SaveTrajectoryHistory()
        {
            try
            {
                _thisParticipant.TrajectoryHistory.Save(Configuration.PathToTrajectoryHistory(_thisParticipant.Name));
            }
            catch (Exception ex)
            {
                Logging.WriteLog("Could not save trajectory history: " + ex.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
            }
        }
        static void LoadEmotiveWeights()
        {
            try
            {
                _thisAeon.EmotiveWeights.Load(Configuration.PathToEmotiveWeights);
            }
            catch (Exception ex)
            {
                Logging.WriteLog("Could not load emotive weights: " + ex.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
            }
        }
        static void SaveEmotiveWeights()
        {
            try
            {
                _thisAeon.EmotiveWeights.Save(Configuration.PathToEmotiveWeights);
            }
            catch (Exception ex)
            {
                Logging.WriteLog("Could not save emotive weights: " + ex.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
            }
        }
        // Once a mood state is realized, how does it influence the conversation?
        static async Task<bool> ProcessInput(string returnFromProcess = "")
        {
            if (_thisAeon.IsAcceptingInput)
            {
                _aeonChatStartedOn = DateTime.Now;
                await Task.Delay(250);
                var rawInput = ParticipantInput;// Here in what is being said.
                if (rawInput.Contains('\n'))
                {
                    rawInput = rawInput.TrimEnd('\n');
                }
                Console.WriteLine(_thisParticipant.Name + ": " + rawInput);
                _thisRequest = new ParticipantRequest(rawInput, _thisParticipant, _thisAeon);
                _thisResult = _thisAeon.Chat(_thisRequest);
                await Task.Delay(200);
                Output.Present(new OutputPresentation(
                    _thisAeon.Name,
                    _thisResult.Output,
                    RequestedOutputModalities,
                    _thisAeon.Mood.GetCurrentIndication(),
                    _thisResult.InstructionalDisplacement));
                Logging.RecordTranscript(_thisParticipant.Name + ": " + rawInput);
                Logging.RecordTranscript(_thisAeon.Name + ": " + _thisResult.Output);
                _aeonChatDuration = DateTime.Now - _aeonChatStartedOn;
                Logging.WriteLog("Result search was conducted in: " + _aeonChatDuration.Seconds + @"." + _aeonChatDuration.Milliseconds + " seconds", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                _thisAeon.AeonAloneTimer.Enabled = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
                AeonIsAlone = false;
                AeonResult = _thisResult.Output;// Here is what the aeon has said.
                SaveTrajectoryHistory();
                SaveEmotiveWeights();
            }
            else
            {
                ParticipantInput = string.Empty;
                Console.WriteLine("Aeon is not accepting user input." + Environment.NewLine);
            }
            return true;
        }

        #region Learning mode feature
        static async Task LearningMode()
        {
            if (_thisConfig == null)
            {
                Console.WriteLine("Learning mode is unavailable because no writable personality is loaded.");
                return;
            }

            Console.WriteLine("Learning mode: enter a phrase and its literal response. Leave either field blank to cancel.");
            Console.Write("Phrase: ");
            string pattern = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(pattern))
            {
                Console.WriteLine("Learning cancelled.");
                return;
            }
            Console.Write("Response: ");
            string response = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(response))
            {
                Console.WriteLine("Learning cancelled.");
                return;
            }

            Library.Builder.LearningProposal proposal;
            try
            {
                proposal = Library.Builder.LearningProposal.Create(pattern, response);
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine("Learning cancelled: " + ex.Message);
                return;
            }

            Console.WriteLine("Review learned category:");
            Console.WriteLine("  Phrase: " + proposal.Pattern);
            Console.WriteLine("  Response: " + proposal.Response);
            Console.Write("Save this local category? Type yes to confirm: ");
            if (!string.Equals(Console.ReadLine(), "yes", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Learning cancelled; no file was created.");
                return;
            }

            try
            {
                string filename = _thisConfig.CreateAeonFile(proposal, _instance);
                AeonLoader.LoadAeonCodeFile(filename);
                _instance++;
                Console.WriteLine("Learned category saved locally and loaded: " + filename);
            }
            catch (Exception ex)
            {
                Logging.WriteLog("Could not save or load a reviewed learning proposal: " + ex.Message, Logging.LogType.Error, Logging.LogCaller.LearningThread);
                Console.WriteLine("Learning could not be completed. See the log for details.");
            }
            await Task.CompletedTask;
        }
        #endregion

        #region Alone feature
        static void AloneMessage(bool alone)
        {
            if (alone)
            {
                if (!_aeonAloneThread.IsAlive)
                {
                    _aeonAloneThread = new Thread(AeonAloneText) { IsBackground = true };
                    _aeonAloneThread.Start();
                }
            }
        }
        static void CheckIfAeonIsAlone()
        {
            if (_thisAeon.IsAlone())
            {
                AloneMessage(true);
                //SetMoodic("alone");
                AeonIsAlone = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
            }
        }
        static void AloneEvent(object source, ElapsedEventArgs e)
        {
            CheckIfAeonIsAlone();
        }
        static void AeonAloneText()
        {
            do
            {
                AloneMessageOutput = StaticRandom.Next(0, 10);
            }
            while (AloneMessageOutput == PreviousAloneMessageOutput);

            PreviousAloneMessageOutput = AloneMessageOutput;
            AloneTextCurrent = _thisAeon.GlobalSettings.GrabSetting("alonemessage" + AloneMessageOutput);
            if (AloneTextCurrent.Length > 0)
            {
                string prompt = AlonePrompt.Format(_thisAeon.Name, AloneTextCurrent);
                Output.Present(new OutputPresentation(
                    _thisAeon.Name,
                    AloneTextCurrent,
                    RequestedOutputModalities,
                    _thisAeon.Mood.GetCurrentIndication()));
                Logging.RecordTranscript(prompt);
            }
        }

        private static OutputModality RequestedOutputModalities => VoiceOutputEnabled
            ? OutputModality.Text | OutputModality.Voice
            : OutputModality.Text;

        private static void ConfigureSpeechOutput()
        {
            VoiceOutputEnabled = TryGetBooleanSetting("voiceenabled");
            if (!VoiceOutputEnabled)
            {
                return;
            }

            string backendSetting = _thisAeon.GlobalSettings.GrabSetting("voicebackend");
            SpeechBackend? backend = backendSetting.Trim().ToLowerInvariant() switch
            {
                "windows-sapi" when OperatingSystem.IsWindows() => SpeechBackend.WindowsSapi,
                "aeonvoice" when OperatingSystem.IsLinux() => SpeechBackend.AeonVoice,
                "espeak" when OperatingSystem.IsLinux() => SpeechBackend.Espeak,
                "auto" or "" when OperatingSystem.IsWindows() => SpeechBackend.WindowsSapi,
                "auto" or "" when OperatingSystem.IsLinux() => SpeechBackend.AeonVoice,
                _ => null
            };
            if (backend is null)
            {
                VoiceOutputEnabled = false;
                Logging.WriteLog("Voice output is enabled but no supported backend is configured for this operating system.", Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
                return;
            }

            int timeoutMilliseconds = 30000;
            if (int.TryParse(_thisAeon.GlobalSettings.GrabSetting("voicetimeoutmilliseconds"), out int configuredTimeout))
            {
                timeoutMilliseconds = Math.Clamp(configuredTimeout, 1000, 120000);
            }
            Output.Add(new SpeechOutputAdapter(
                backend.Value,
                _thisAeon.GlobalSettings.GrabSetting("voiceprofile"),
                _thisAeon.GlobalSettings.GrabSetting("linuxaudioplayer"),
                _thisAeon.GlobalSettings.GrabSetting("espeakcommand"),
                _thisAeon.GlobalSettings.GrabSetting("espeakvoice"),
                TimeSpan.FromMilliseconds(timeoutMilliseconds),
                message => Logging.WriteLog(message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime)));
        }

        private static bool TryGetBooleanSetting(string name)
        {
            return bool.TryParse(_thisAeon.GlobalSettings.GrabSetting(name), out bool value) && value;
        }

        private static void PlayStartupTheme()
        {
            string filename = string.IsNullOrWhiteSpace(StartUpThemeFile) ? "startup-theme.wav" : StartUpThemeFile;
            string themePath = Path.IsPathFullyQualified(filename)
                ? filename
                : Path.Combine(Configuration.ActiveRuntime, "sounds", filename);
            if (!File.Exists(themePath))
            {
                Logging.WriteLog("The configured startup theme was not found: " + themePath, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
                return;
            }

            if (OperatingSystem.IsWindows())
            {
                PlayStartupThemeOnWindows(themePath);
                return;
            }

            if (OperatingSystem.IsLinux())
            {
                try
                {
                    var startInfo = new ProcessStartInfo
                    {
                        FileName = "aplay",
                        UseShellExecute = false,
                        CreateNoWindow = true
                    };
                    startInfo.ArgumentList.Add("--quiet");
                    startInfo.ArgumentList.Add(themePath);
                    Process.Start(startInfo);
                }
                catch (Exception exception)
                {
                    Logging.WriteLog("Could not start the Linux startup theme player: " + exception.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
                }
                return;
            }

            Logging.WriteLog("Startup-theme playback is unavailable on this operating system.", Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
        }

        [SupportedOSPlatform("windows")]
        private static void PlayStartupThemeOnWindows(string themePath)
        {
            _ = Task.Run(() =>
            {
                try
                {
                    using var player = new SoundPlayer(themePath);
                    player.PlaySync();
                }
                catch (Exception exception)
                {
                    Logging.WriteLog("Could not play the startup theme: " + exception.Message, Logging.LogType.Warning, Logging.LogCaller.AeonRuntime);
                }
            });
        }
        #endregion
    }
}
