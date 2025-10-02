#define Windows

using Cartheur.Animals.Core;
using Cartheur.Animals.Personality;
using Cartheur.Animals.Utilities;
using System;
using System.IO;
using System.Threading;
using System.Timers;

#if Windows
using System.Speech.Recognition;
using System.Speech.Synthesis;
#endif

namespace Cartheur.Animals.Terminal
{
    static class Program
    {
		static MeaningFive AeonFive;
		static bool AeonFiveExists { get; set; }
        // Configuration of the application.
        static LoaderPaths Configuration;
        static bool TerminalMode { get; set; }
        // The feature-set to be included.
        static Aeon _thisAeon;
        static User _thisUser;
        static Thread _aeonAloneThread;
        // Aeon's conversational elements.
        static Request _thisRequest;
        static Result _thisResult;
        static DateTime _aeonChatStartedOn;
        static TimeSpan _aeonChatDuration;
        // Aeon's status.
        static bool SettingsLoaded { get; set; }
        static bool AeonLoaded { get; set; }
        static string UserInput { get; set; }
        static string AeonType { get; set; }
        static string AeonOutputDialogue { get; set; }
        static string NucodeDirectoryPath { get; set; }
        static string LastOutput { get; set; }
        static string LastRoutine { get; set; }
        // Aeon alone feature.
        static bool AeonIsAlone { get; set; }
        static string AloneTextCurrent { get; set; }
        static int AloneMessageVariety { get; set; }
        static int AloneMessageOutput { get; set; }
        static int PreviousAloneMessageOutput { get; set; }
        // Huggable's mood, interaction, and manifest personality.
        static int SwitchMood { get; set; }
        static bool DetectUserEmotion { get; set; }
        static Mood AnimalMood { get; set; }
        static string AnimalCurrentMood { get; set; }
        static int BeatFrequency { get; set; }
        // Speech recognition and synthesizer engines.
        static bool SapiWindowsUsed { get; set; }
        static bool SpeechSynthesizerUsed { get; set; }
        // Speech recognizer and synthesizer for Windows.
#if Windows
        static readonly SpeechRecognitionEngine Recognizer = new SpeechRecognitionEngine();
        static readonly GrammarBuilder GrammarBuilder = new GrammarBuilder();
        static readonly SpeechSynthesizer SpeechSynth = new SpeechSynthesizer();
        static readonly PromptBuilder PromptBuilder = new PromptBuilder();
#endif

        /// <summary>
        /// Defines the entry point of the application.
        /// </summary>
        static void Main()
        {
            Configuration = new LoaderPaths("Debug");
            Logging.ActiveConfiguration = Configuration.ActiveRuntime;
            // Create the aeon and load its basic parameters from a config file.
            _thisAeon = new Aeon("1+2i");
            _thisAeon.LoadSettings(Configuration.PathToSettings);
            SettingsLoaded = _thisAeon.LoadDictionaries(Configuration);
            _thisUser = new User(_thisAeon.GlobalSettings.GrabSetting("username"), _thisAeon);
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("product") + " - Version " + _thisAeon.GlobalSettings.GrabSetting("version") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("ip") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("claim") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("warning") + Environment.NewLine);
            Console.WriteLine("------ Begin help ------");
            Console.WriteLine("If running in terminal mode, type 'quit' to leave and resume an aeon.");
            Console.WriteLine("While in terminal mode, type 'exit' to quit the application entirely.");
            Console.WriteLine("If you want to dissolve your aeon while in non-terminal mode, speak 'aeon quit'.");
            Console.WriteLine("------ End help------" + Environment.NewLine);
            Console.WriteLine("Constructing the personality..." + Environment.NewLine);
            // Check that the aeon launch is valid.
            UserInput = "";
            _thisAeon.Name = _thisAeon.GlobalSettings.GrabSetting("name");
            _thisAeon.EmotionUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("emotionused"));
            SharedFunctions.ThisAeon = _thisAeon;
            // Determine what external hardware is to be used.
            SapiWindowsUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("sapiwindows"));
            SpeechSynthesizerUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("speechsynthesizer"));
            BeatFrequency = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("beatfrequency"));
            TerminalMode = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("terminalmode"));
            // Initialize the alone feature.
            _thisAeon.AeonAloneTimer = new System.Timers.Timer();
            _thisAeon.AeonAloneTimer.Elapsed += AloneEvent;
            _thisAeon.AeonAloneTimer.Interval = Convert.ToDouble(_thisAeon.GlobalSettings.GrabSetting("alonetimecheck"));
            _thisAeon.AeonAloneTimer.Enabled = false;
            _aeonAloneThread = new Thread(AeonAloneText);
            // Initialize the mood feature.
            if (_thisAeon.EmotionUsed)
            {
                // ToDo: Once a mood state is realized, how does it influence the conversation?
                AnimalMood = new Mood(StaticRandom.Next(0, 20), _thisAeon, _thisUser);
                AnimalCurrentMood = AnimalMood.GetCurrentMood();
                SwitchMood = 0;
                // What happens next when the animal is to be emotional?
            }
            // Utilize the correct settings based on the aeon personality.
            if (_thisAeon.Name == "Rhodo" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Henry" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Blank" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadBlank(Configuration);
            if (_thisAeon.Name == "Samantha" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Fred" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Aeon" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Mitsuku" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);

            // Attach logging, xms functionality, and spontaneous file generation.
            Logging.LogModelFile = _thisAeon.GlobalSettings.GrabSetting("logmodelfile");
            Logging.TranscriptModelFile = _thisAeon.GlobalSettings.GrabSetting("transcriptmodelfile");
            // Set the aeon type by personality.
            switch (_thisAeon.Name)
            {
                case "Aeon":
                    AeonType = "Assistive";
                    break;
                case "Fred":
                    AeonType = "Toy";
                    break;
                case "Henry":
                    AeonType = "Toy";
                    break;
                case "Rhodo":
                    AeonType = "Default";
                    break;
                case "Samantha":
                    AeonType = "Friendly";
                    break;
                case "Blank":
                    AeonType = "Huggable";
                    break;
            }
            Console.WriteLine("The aeon type is " + AeonType.ToLower() + ".");
            Console.WriteLine("Personality construction completed.");
            Console.WriteLine("A presence named '" + _thisAeon.Name + "' has been initialized.");
            Console.WriteLine("It has " + _thisAeon.Size + " categories available in its mind.");
            Console.WriteLine("Your aeon is ready for an interaction with you." + Environment.NewLine);

#if Windows
            if (SapiWindowsUsed && AeonLoaded)
                InitializeSapiWindows();
#endif

            Console.WriteLine("Your transcript follows. Enjoy!");
            Console.WriteLine("**********************" + Environment.NewLine);
            if (TerminalMode)
            {
                Console.WriteLine("Terminal mode is active.");
                // Trigger the terminal for typing torch commands.
                while (true && UserInput != "quit")
                {
                    UserInput = "";
                    UserInput = Console.ReadLine();
                    if (UserInput != null)
                        ProcessTerminal();
                    if (UserInput == "quit")
                    {
                        Console.WriteLine("Leaving terminal mode and starting a conversational aeon.");
                        Thread.Sleep(2000);
                        break;
                    }
                    if (UserInput == "exit")
                    {
                        Console.WriteLine("Leaving terminal mode and exiting the application.");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    }
                }
            }
            if (_thisAeon.EmotionUsed)
                Console.WriteLine("The aeon's current mood is: " + Mood.CurrentMood + ".");

            // Set the beat-frequency of the program.
            Console.WriteLine("The interaction frequency of the program is: " + BeatFrequency + " milliseconds.");
            Console.WriteLine("**********************" + Environment.NewLine);
            if (SapiWindowsUsed)
            {
                while (true)
                {
                    UserInput = Console.ReadLine();
                    if (UserInput != null)
                        ProcessInput();
                    if (UserInput == "aeon quit")
                        break;
                }
            }
            if (!SapiWindowsUsed)
            {
                while (true)
                {
                    Thread.Sleep(BeatFrequency);
                }
            }
        }

        static bool ProcessTerminal()
        {
            ProcessInput();
            return true;
        }
        /// <summary>
        /// The main input method to pass an enquiry to the system, yielding a reaction/response behavior to the user.
        /// </summary>
        /// <remarks>Once a mood state is realized, how does it influence the conversation?</remarks>
        static bool ProcessInput(string returnFromProcess = "")
        {
            Syntax.CommandReceived = false;
            if (_thisAeon.IsAcceptingUserInput)
            {
                _aeonChatStartedOn = DateTime.Now;
                Thread.Sleep(250);
                var rawInput = UserInput;
                if (rawInput.Contains("\n"))
                {
                    rawInput = rawInput.TrimEnd('\n');
                }
                Console.WriteLine(_thisUser.UserName + ": " + rawInput);
                _thisRequest = new Request(rawInput, _thisUser, _thisAeon);
                _thisResult = _thisAeon.Chat(_thisRequest);
                Thread.Sleep(200);
                Console.WriteLine(_thisAeon.Name + ": " + _thisResult.Output);
                Logging.RecordTranscript(_thisUser.UserName + ": " + rawInput);
                Logging.RecordTranscript(_thisAeon.Name + ": " + _thisResult.Output);
                // Record performance vectors for the result.
                _aeonChatDuration = DateTime.Now - _aeonChatStartedOn;
                Logging.WriteLog("Result search was conducted in: " + _aeonChatDuration.Seconds + @"." + _aeonChatDuration.Milliseconds + " seconds", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                // Learning: Send the result to the learning algorithm.
                if (!AeonFiveExists)
                {
                	AeonFive = new MeaningFive(_thisResult);
                	Console.WriteLine("---------------------");
                	Console.WriteLine("MeaningFive-5 active.");
                	Console.WriteLine("---------------------");
                	AeonFiveExists = true;
                }
                if (SpeechSynthesizerUsed)
                    SpeakText(_thisResult.Output);
                _thisAeon.AeonAloneTimer.Enabled = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
                AeonIsAlone = false;
                AeonOutputDialogue = _thisResult.Output;
                Thread.Sleep(BeatFrequency);
                if (UserInput == "exit")
                {
                    Console.WriteLine("Detected 'exit'...quitting the application.");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
            }
            else
            {
                UserInput = string.Empty;
                Console.WriteLine("Aeon is not accepting user input." + Environment.NewLine);
            }
            return true;
        }

        #region Social features
        
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
            AloneMessageVariety = StaticRandom.Next(1, 1750);
			Console.WriteLine("Your relationship with your aeon is: " + Mood.RelationshipOutcome(_thisResult.Output) + ".");
            if (AloneMessageVariety.IsBetween(1, 250))
                AloneMessageOutput = 0;
            if (AloneMessageVariety.IsBetween(251, 500))
                AloneMessageOutput = 1;
            if (AloneMessageVariety.IsBetween(501, 750))
                AloneMessageOutput = 2;
            if (AloneMessageVariety.IsBetween(751, 1000))
                AloneMessageOutput = 3;
            if (AloneMessageVariety.IsBetween(1001, 1250))
                AloneMessageOutput = 4;
            if (AloneMessageVariety.IsBetween(1001, 1250))
                AloneMessageOutput = 5;
            if (AloneMessageVariety.IsBetween(1251, 1500))
                AloneMessageOutput = 6;
            if (AloneMessageVariety.IsBetween(1501, 1750))
                AloneMessageOutput = 7;

            PreviousAloneMessageOutput = AloneMessageOutput;
            SwitchMood = 1;
            AloneTextCurrent = _thisAeon.GlobalSettings.GrabSetting("alonemessage" + AloneMessageOutput);
            SpeakText(_thisAeon.GlobalSettings.GrabSetting("alonesalutaion") + ", "+ _thisUser.UserName + ", " + AloneTextCurrent);
        }
        
        #endregion

        #region Speech recognizer as per the OS

#if Windows

        static void SapiWindowsSpeechRecognized(object sender, SpeechRecognizedEventArgs e)
        {
            UserInput = e.Result.Text;
            ProcessInput();
            Logging.WriteLog(LastOutput, Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
        }
        static bool InitializeSapiWindows()
        {
            GrammarBuilder.Culture = Recognizer.RecognizerInfo.Culture;
            try
            {
                // Read in the list of phrases that the speech engine will recognise when it detects it being spoken.
                GrammarBuilder.Append(
                    new Choices(File.ReadAllLines(Path.Combine(Configuration.ActiveRuntime, Path.Combine("grammar", "valid-grammar.txt")))));
                Logging.WriteLog("Windows SAPI detected. Load the grammar file.", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                Console.WriteLine("Windows SAPI detected. Load the grammar file found in the 'bin' folder.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                return false;
            }
            var grammar = new Grammar(GrammarBuilder);
            try
            {
                Recognizer.UnloadAllGrammars();
                Recognizer.RecognizeAsyncCancel();
                Recognizer.RequestRecognizerUpdate();
                Recognizer.LoadGrammar(grammar);
                Recognizer.SpeechRecognized += SapiWindowsSpeechRecognized;
                Recognizer.SetInputToDefaultAudioDevice();
                Recognizer.RecognizeAsync(RecognizeMode.Multiple);
                Logging.WriteLog("Windows SAPI: Recognizer initialized.", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                Console.WriteLine("Windows SAPI: Recognizer initialized.");
                Thread.Sleep(700);
            }
            catch (Exception ex)
            {
                Console.WriteLine("An error has occurred in the recognizer. " + ex.Message);
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.RobotDialogue);
            }
            finally
            {
                SpeakText("I am here");
            }
            Console.WriteLine("Windows SAPI: Loaded the correct grammar file.");
            Thread.Sleep(700);
            return true;
        }
#endif

        #endregion

        #region Speech synthesizer as per the OS

        static void SpeakText(string input)
        {
            if (SpeechSynthesizerUsed)
            {
                try
                {
#if Windows
                    PromptBuilder.ClearContent();
                    PromptBuilder.AppendText(input);
                    SpeechSynth.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult);
                    SpeechSynth.Speak(PromptBuilder);
                    Thread.Sleep(BeatFrequency);
#endif
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                }
            }
        }
        #endregion
    }
}
