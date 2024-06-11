//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
#define Linux
//#define LinuxVoice
using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Timers;
using System.Xml;
//using NeSpeak;
using Cartheur.Animals.Core;
using Cartheur.Animals.Control;
using Cartheur.Animals.FileLogic;
using Cartheur.Animals.Personality;
using Cartheur.Animals.Utilities;

namespace Cartheur.Huggable.Console
{
    /// <summary>
    /// Application wrapper of the Aeon algorithm. A friendly dialogue you can take anywhere.
    /// </summary>
    /// <remarks>Multiplatform code: Windows and Linux flavoured.</remarks>
    class Program
    {
        // The build configuration of the application.
        public static DateTime BuildDate { get; set; }
        public static LoaderPaths Configuration;
        public static bool StartUpTheme { get; set; }
        public static string StartUpThemeFile { get; set; }
        public static bool TerminalMode { get; set; }
        public static Process TerminalProcess { get; set; }
        // Aeon's personal algorithmic items.
        private static Aeon _thisAeon;
        private static User _thisUser;
        private static Request _thisRequest;
        private static Result _thisResult;
        private static DateTime _aeonChatStartedOn;
        private static TimeSpan _aeonChatDuration;
        private static Thread _aeonAloneThread;
        // Aeon's status.
        private static bool SettingsLoaded { get; set; }
        private static bool AeonLoaded { get; set; }
        private static DateTime AeonStartedOn { get; set; }
        private static TimeSpan AeonSessionLifeDuration { get; set; }
        public static string UserInput { get; set; }
        public static string AeonOutputDialogue { get; set; }
        public static string AeonOutputDebug { get; set; }
        public static int AloneMessageOutput { get; set; }
        public static int PreviousAloneMessageOutput { get; set; }
        public static int AloneMessageVariety { get; set; }
        public static string LastOutput { get; set; }
        public bool EncryptionUsed { get; set; }
        public int AeonSize { get; set; }
        public static string AeonType { get; set; }
        public static bool AeonIsAlone { get; set; }
        public static string AloneTextCurrent { get; set; }
        public static string XmsDirectoryPath { get; set; }
        public static string NucodeDirectoryPath { get; set; }
        public static string PythonLocation { get; set; }
        // Aeon's mood, interaction, and manifest personality.
        public static bool TestHardware { get; set; }
        private static int SwitchMood { get; set; }
        public static bool DetectUserEmotion { get; set; }
        private static string EmotiveEquation { get; set; }
        public static Mood AeonMood { get; set; }
        public static string AeonCurrentMood { get; set; }
        // Speech recognition and synthesizer engines.
        public static bool SapiWindowsUsed { get; set; }
        public static bool PocketSphinxUsed { get; set; }
        // For RabbitMQ messaging on pocketsphinx output.
#if LinuxVoice
        //public static ConnectionFactory Factory { get; set; }
        //public static IModel Channel { get; private set; }
        //public static EventingBasicConsumer Consumer { get; set; }
#endif
        public static Process SphinxProcess { get; set; }
        public static readonly string SphinxStartup = @"pocketsphinx_continuous -hmm /usr/share/pocketsphinx/model/hmm/en_US/en-us -dict /usr/share/pocketsphinx/model/lm/en_US/cmudict-en-us.dict -lm /usr/share/pocketsphinx/model/lm/en_US/en-us.lm.bin -inmic yes -backtrace yes -logfn /dev/null";
        // For remote puppeteering.
        static readonly WebClient HenryClient = new WebClient();
        public static bool UsePythonBottle { get; set; }
        // For voice synthetizer
        public static bool SpeechSynthesizerUsed { get; set; }
#if LinuxVoice
        static ESpeakLibrary PresenceSpeaker { get; set; }
#endif
        public static string PresenceSpeakerVoice { get; set; }
        // External hardware and libraries.
        public static bool BottleServerConnected { get; private set; }
        public static string SayParameter { get; set; }
        public static string BottleIpAddress { get; set; }
        public static Process RobotProcess { get; set; }
        public static string RobotProcessOutput { get; set; }
        public static int RobotProcessExitCode { get; set; }
        public static bool CorrectExecution { get; set; }
        // Facial movements in three-dimensions.
        public static int NumberOfMotors { get; set; }
        public static int EyesOpenGpio { get; set; }
        public static int EyesCloseGpio { get; set; }
        public static int NoseOpenGpio { get; set; }
        public static int NoseCloseGpio { get; set; }
        public static int MouthOpenGpio { get; set; }
        public static int MouthCloseGpio { get; set; }
        public static string EmotiveA { get; set; }
        public static string EmotiveB { get; set; }
        public static string EmotiveC { get; set; }
        // Additive huggable feature-set.
#if Windows
        private static SoundPlayer EmotiveResponse { get; set; }
        private static SoundPlayer TonalResponse { get; set; }
#endif
        public static string TonalRootPath { get; set; }
        public static string TonalA { get; set; }
        public static string TonalB { get; set; }
        public static string TonalC { get; set; }
        public static string TonalD { get; set; }
        public static string TonalE { get; set; }
        public static string TonalF { get; set; }
        public static string TonalFs { get; set; }
        public static string TonalG { get; set; }
        public static string TonalAp { get; set; }
        public static int TonalDelay { get; set; }
        public static bool SpeechToTonal { get; set; }
        public static bool TonalSpeechLimit { get; set; }
        public static int TonalSpeechLimitValue { get; set; }
        public static int Repetition { get; set; }
        /// <summary>
        /// Where the action takes place.
        /// </summary>
        static void Main()
        {
            // Set the build and load its configuration details.
            System.Version BuildVersion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
            BuildDate = File.GetCreationTime(Assembly.GetExecutingAssembly().Location);
            Build.LoadSettings(Build.PathToBuildSettings);
            Configuration = new LoaderPaths(Build.BuildSettings.GrabSetting("configuration"));
            Logging.ActiveConfiguration = Configuration.ActiveRuntime;
            // Create the aeon and load its basic parameters from a config file.
            _thisAeon = new Aeon(Build.BuildSettings.GrabSetting("aeon-algorithm"));
            _thisAeon.LoadSettings(Configuration.PathToSettings);
            SettingsLoaded = _thisAeon.LoadDictionaries(Configuration);
            _thisUser = new User(_thisAeon.GlobalSettings.GrabSetting("username"), _thisAeon);
            _thisAeon.Name = _thisAeon.GlobalSettings.GrabSetting("name");
            _thisAeon.EmotionUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("emotionused"));
            System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("product") + " - Aeon Algorithm: " + Build.BuildSettings.GrabSetting("aeon-algorithm") + ".");
            System.Console.WriteLine("Build date: " + BuildDate + ". Assembly version: " + Assembly.GetExecutingAssembly().GetName().Version.ToString() + ".");
            //System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("ip"));
            //System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("claim"));
            //Thread.Sleep(700);
            //System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("warning"));
            //Thread.Sleep(700);
            //System.Console.WriteLine("------ ******** ------");
            //System.Console.WriteLine("------ Build notes ------");
            //System.Console.WriteLine("------ ******** ------");
            //System.Console.WriteLine(" - - - This version has drawing 300 and 400 of USPTO US20180204107A1 implemented - - - " );
            //System.Console.WriteLine("------ ******** ------");
            //System.Console.WriteLine("------ End build notes ------");
            //System.Console.WriteLine("------ ******** ------");
            System.Console.WriteLine("------ ************ ------");
            System.Console.WriteLine("------ Agent help ------");
            System.Console.WriteLine("------ ************ ------");
            System.Console.WriteLine("The agent is auto-configured so that it is ready to respond to you at the terminal. Start by saying 'hello'.");
            Thread.Sleep(700);
            if (_thisAeon.EmotionUsed)
            {
                System.Console.WriteLine("The agent starts with a random emotions and will change its mood based on its experience with you.");
                System.Console.WriteLine("If you leave the agent alone too long, it will influence its negative emotions.");
            }
            else
            {
                System.Console.WriteLine("The agent has its emotional capacity set to false.");
            }  
            Thread.Sleep(700);
            System.Console.WriteLine("While in terminal mode, type 'exit' to decompose and dissolve the agent.");
            System.Console.WriteLine("------ ******** ------");
            System.Console.WriteLine("------ End help ------");
            System.Console.WriteLine("------ ******** ------");
            Thread.Sleep(700);
            System.Console.WriteLine("Continuing to construct the personality...");
            // Check that the aeon launch is valid.
            UserInput = "";
            DetectUserEmotion = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("emotiondetection"));
            StartUpTheme = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("startuptheme"));
            StartUpThemeFile = _thisAeon.GlobalSettings.GrabSetting("startupthemefile");
            EmotiveEquation = _thisAeon.GlobalSettings.GrabSetting("emotiveequation");
            // Initialize the alone feature, as well as other hardware.
            _thisAeon.AeonAloneTimer = new System.Timers.Timer();
            _thisAeon.AeonAloneTimer.Elapsed += AloneEvent;
            _thisAeon.AeonAloneTimer.Interval = Convert.ToDouble(_thisAeon.GlobalSettings.GrabSetting("alonetimecheck"));
            _thisAeon.AeonAloneTimer.Enabled = false;
            _aeonAloneThread = new Thread(AeonAloneText);
#if LinuxVoice
                PresenceSpeaker = new ESpeakLibrary(NeSpeak.espeak_AUDIO_OUTPUT.AUDIO_OUTPUT_PLAYBACK);
#endif
            SharedFunctions.ThisAeon = _thisAeon;
            // Determine what external hardware is to be used.
            SapiWindowsUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("sapiwindowsused"));
            PocketSphinxUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("pocketsphinxused"));
            SpeechSynthesizerUsed = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("speechsynthesizerused"));
            UsePythonBottle = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("usepythonbottle"));
            // Load any external interpreters for hardware control.
            PythonLocation = _thisAeon.GlobalSettings.GrabSetting("pythonlocation");
            // Physical toy where code will be running hardware controllers.
            TestHardware = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("testhardware"));
            NumberOfMotors = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("numberofmotors"));
            EyesOpenGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("eyesopengpio"));
            EyesCloseGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("eyesclosegpio"));
            NoseOpenGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("noseopengpio"));
            NoseCloseGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("noseclosegpio"));
            MouthOpenGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("mouthopengpio"));
            MouthCloseGpio = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("mouthclosegpio"));
            TerminalMode = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("terminalmode"));
            // Set the path for the emotive audio files.
            EmotiveA = _thisAeon.GlobalSettings.GrabSetting("emotiveafile");
            EmotiveB = _thisAeon.GlobalSettings.GrabSetting("emotivebfile");
            EmotiveC = _thisAeon.GlobalSettings.GrabSetting("emotivecfile");
            TonalRootPath = Configuration.PathToTonalRoot;
            TonalA = _thisAeon.GlobalSettings.GrabSetting("tonalafile");
            TonalB = _thisAeon.GlobalSettings.GrabSetting("tonalbfile");
            TonalC = _thisAeon.GlobalSettings.GrabSetting("tonalcfile");
            TonalD = _thisAeon.GlobalSettings.GrabSetting("tonaldfile");
            TonalE = _thisAeon.GlobalSettings.GrabSetting("tonalefile");
            TonalF = _thisAeon.GlobalSettings.GrabSetting("tonalffile");
            TonalFs = _thisAeon.GlobalSettings.GrabSetting("tonalfsfile");
            TonalG = _thisAeon.GlobalSettings.GrabSetting("tonalgfile");
            TonalAp = _thisAeon.GlobalSettings.GrabSetting("tonalapfile");
            TonalDelay = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("tonaldelay"));
            SpeechToTonal = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("tonalspeech"));
            TonalSpeechLimit = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("tonalspeechlimit"));
            TonalSpeechLimitValue = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("tonalspeechlimitvalue"));
            Repetition = Convert.ToInt32(_thisAeon.GlobalSettings.GrabSetting("repetition"));
            // Initialize the mood feature and set the display to the current mood.
            if (_thisAeon.EmotionUsed)
            {
                // ToDo: Once a mood state is realized, how does it influence the conversation?
                AeonMood = new Mood(StaticRandom.Next(0, 20), _thisAeon, _thisUser, EmotiveEquation, Extensions.Of<Emotions>());
                AeonCurrentMood = AeonMood.EmotionBias.ToString();
                //AeonCurrentMood = AeonMood.GetCurrentMood();
                AeonCurrentMood = AeonMood.Create().ToString(); // USPTO-400
                SwitchMood = 0;
#if Windows
                // What happens next when the animal is to be emotional?
                if (AeonCurrentMood == "Energized" | AeonCurrentMood == "Happy" | AeonCurrentMood == "Confident")
                {
                    EmotiveResponse = new SoundPlayer(Configuration.ActiveRuntime + @"\emotive\tones\" + EmotiveA);
                    //for (int i = 0; i < Repetition; i++) // TODO: Needs queing but thinking creating longer *.wav a better idea.
                    EmotiveResponse.Play();
                    Thread.Sleep(500);
                    // First draft of playing a tonal file sequence, based on the mood.
                    PlayTonalSequence("fun.txt");
                }
                if (AeonCurrentMood == "Helped" | AeonCurrentMood == "Insecure")
                {
                    EmotiveResponse = new SoundPlayer(Configuration.ActiveRuntime + @"\emotive\tones\" + EmotiveB);
                    EmotiveResponse.Play();
                    Thread.Sleep(250);
                    PlayTonalSequence("okay.txt");
                }
                if (AeonCurrentMood == "Hurt" | AeonCurrentMood == "Sad" | AeonCurrentMood == "Tired")
                {
                    EmotiveResponse = new SoundPlayer(Configuration.ActiveRuntime + @"\emotive\tones\" + EmotiveC);
                    EmotiveResponse.Play();
                    Thread.Sleep(250);
                    PlayTonalSequence("contemplate.txt");
                }
#endif
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
            // Attach logging,  xms functionality, and spontaneous file generation.
            Logging.LogModelFile = _thisAeon.GlobalSettings.GrabSetting("logmodelfile");
            Logging.TranscriptModelFile = _thisAeon.GlobalSettings.GrabSetting("transcriptmodelfile");
            // Determine whether to load the Linux or Windows speech synthesizer.
#if LinuxVoice
            if (PocketSphinxUsed && AeonLoaded)
            {
                System.Console.WriteLine("Intializing the pocketsphinx interface with RabbitMQ...");
                InitializeMessageQueue();
            }
#endif
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
            System.Console.WriteLine("The agent type is " + AeonType.ToLower() + ".");
            System.Console.WriteLine("Personality construction completed.");
            System.Console.WriteLine("A presence named '" + _thisAeon.Name + "' has been initialized.");
            System.Console.WriteLine("It has " + _thisAeon.Size + " categories available in its mind.");
            AeonStartedOn = DateTime.Now;
            // Set the runtime state.
            if (_thisAeon.Name.ToLower() == "aeon")
                System.Console.WriteLine("You have selected to load the aeon-assist variety.");
            // Set final parameters and play the welcome message.
#if Windows
            if (StartUpTheme)
            {
                try
                {
                    AnimalActive = new SoundPlayer(Configuration.ActiveRuntime + @"\sounds\" + StartUpThemeFile);
                    AnimalActive.Play();
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                }
            }
#endif
            System.Console.WriteLine("Your agent is ready for an interaction with you.");
            if (_thisAeon.EmotionUsed)
            {
                System.Console.WriteLine("The agent's mood is " + AeonCurrentMood + ".");
                System.Console.WriteLine("The mood polynomial is: " + AeonMood.ReturnMoodPolynomialProperties());
                System.Console.WriteLine("The polynomial properties are: Its roots " + AeonMood.PolynomialRoots[0] + ", " + AeonMood.PolynomialRoots[1] + ", " + AeonMood.PolynomialRoots[2] + " and derivative " + AeonMood.PolynomialDerivative + ".");
            }
            System.Console.WriteLine("Your transcript follows. Enjoy!");
            System.Console.WriteLine("**********************");
            if (TerminalMode)
            {
                System.Console.WriteLine("This version only operates in terminal mode.");
                // Trigger the terminal for typing torch commands.
                while (true && UserInput != "quit")
                {
                    UserInput = "";
                    UserInput = System.Console.ReadLine();
                    if (UserInput != null)
                        ProcessTerminal();
                    if (UserInput == "quit")
                    {
                        System.Console.WriteLine("Leaving terminal mode and starting a conversational aeon.");
                        Thread.Sleep(2000);
                        break;
                    }
                    if (UserInput == "exit")
                    {
                        System.Console.WriteLine("Leaving terminal mode and exiting the application.");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    }
                }
            }
            if (_thisAeon.EmotionUsed)
                System.Console.WriteLine("The agent's current mood is: " + Mood.CurrentMood + ".");

            if (TestHardware && AeonLoaded)
            {
                BlinkRoutine(7);
            }
            // Initialize the queue/pocketsphinx code.
#if LinuxVoice
            if (PocketSphinxUsed && AeonLoaded)
            {
                try
                {
                    ExecuteSphinxProcessToQueue();
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                    System.Console.WriteLine("Pocketsphinx interface error. " + ex.Message);
                }
            }
#endif
            while (true)
            {
                UserInput = System.Console.ReadLine();
                if (UserInput != null)
                    ProcessInput();
                if (UserInput == "aeon quit")
                    break;
            }

            System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("product") + " is closing in ten seconds.");
            Thread.Sleep(10000);
            Logging.WriteLog("Companion shut down at " + DateTime.Now, Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
            Environment.Exit(0);
        }
        public static bool ProcessTerminal()
        {
            //System.Console.WriteLine("Processing terminal command: " + UserInput);
            // For now, pass to the input-processing engine.
            ProcessInput();
            return true;
        }
        /// <summary>
        /// The main input method to pass an enquiry to the system, yielding a reaction/response behavior to the user.
        /// </summary>
        /// <remarks>Once a mood state is realized, how does it influence the conversation?</remarks>
        public static bool ProcessInput(string returnFromProcess = "")
        {
            if (DetectUserEmotion)
            {
                //CorrectExecution = Gpio.RunPythonScript("detectemotion.py", "", Configuration);
                // Will need to return the emotion detected by the script.
            }
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
                System.Console.WriteLine(_thisUser.UserName + ": " + rawInput);
                _thisRequest = new Request(rawInput, _thisUser, _thisAeon);
                _thisResult = _thisAeon.Chat(_thisRequest);
                Thread.Sleep(200);
                System.Console.WriteLine(_thisAeon.Name + ": " + _thisResult.Output);
                Logging.RecordTranscript(_thisUser.UserName + ": " + rawInput);
                Logging.RecordTranscript(_thisAeon.Name + ": " + _thisResult.Output);
                // Record performance vectors for the result.
                _aeonChatDuration = DateTime.Now - _aeonChatStartedOn;
                Logging.WriteLog("Result search was conducted in: " + _aeonChatDuration.Seconds + @"." + _aeonChatDuration.Milliseconds + " seconds", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                // Learning: Send the result to the learning algorithm. For v.1.2.
                //AeonFive = new MeaningFive(_thisResult);
                if (!UsePythonBottle && SpeechSynthesizerUsed)
                    Speak(_thisResult.Output);
                if (SpeechToTonal)
#if Windows
                    TransposeTonalSpeech(_thisResult.Output);
#endif
                if (UsePythonBottle)
                {
                    var message = new NameValueCollection
                    {
                        ["speech"] = _thisResult.Output
                    };

                    try
                    {
                        HenryClient.UploadValues(_thisAeon.GlobalSettings.GrabSetting("bottleipaddress"), "POST", message);
                    }
                    catch (Exception ex)
                    {
                        Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                        System.Console.WriteLine("No response from the emotional toy.");
                    }
                }
                if (TestHardware && NumberOfMotors == 2)
                {
                    CorrectExecution = Gpio.RunPythonScript("emu1.py", "1", Configuration);
                }
                if (TestHardware && NumberOfMotors == 3)
                {
                    CorrectExecution = Gpio.RunPythonScript("emu2.py", "1", Configuration);
                }
                AeonOutputDebug = GenerateAeonOutputDebug();
                AeonOutputDialogue = _thisResult.Output;
                if (UserInput == "exit")
                {
                    System.Console.WriteLine("The console has detected the 'exit' command: Dissolving this agent.");
                    // Print to the console the duration that the aeon has been active.
                    AeonSessionLifeDuration = DateTime.Now - AeonStartedOn;
                    System.Console.WriteLine("This agent has been active for: " + AeonSessionLifeDuration.Hours + " hours, " + AeonSessionLifeDuration.Minutes + " minutes and " + AeonSessionLifeDuration.Seconds + " seconds.");
                    Thread.Sleep(3000);
                    if (_thisAeon.AeonAloneTimer.Enabled == false)
                        System.Console.WriteLine("This agent has not been conversed with.");
                    if(_thisAeon.AeonAloneTimer.Enabled == true)
                        System.Console.WriteLine("This agent has enjoyed your company.");
                    Thread.Sleep(700);
                    System.Console.WriteLine("We hope you have enjoyed your experience -- The Cartheur Dev Team.");
                    Thread.Sleep(1500);
                    System.Console.WriteLine("Goodbye.");
                    Environment.Exit(0);
                }
                _thisAeon.AeonAloneTimer.Enabled = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
                AeonIsAlone = false;
            }
            else
            {
                UserInput = string.Empty;
                System.Console.WriteLine("Aeon is not accepting user input." + Environment.NewLine);
            }
            return true;
        }

        #region Terminal process
        public static bool ExecuteTerminalProcess(string terminalCommand)
        {
            TerminalProcess = new Process();
            TerminalProcess.StartInfo.FileName = "th";
            TerminalProcess.StartInfo.Arguments = " " + terminalCommand;
            TerminalProcess.StartInfo.UseShellExecute = false;
            TerminalProcess.StartInfo.RedirectStandardOutput = true;

            try
            {
                TerminalProcess.Start();
                System.Console.WriteLine(TerminalProcess.StandardOutput.ReadToEnd());
                TerminalProcess.WaitForExit();
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                System.Console.WriteLine("An error ocurred in the terminal process.");
                return false;
            }

        }
        #endregion
#if LinuxVoice
        #region Pocketsphinx process
        /// <summary>
        /// Execute the pocketsphinx process. Connect to the engine.
        /// </summary>
        /// <returns>Standard output and standard error from the process.</returns>
        public static void ExecuteSphinxProcessToQueue()
        {
            SphinxProcess = new Process();
            SphinxProcess.StartInfo.FileName = "/bin/bash";
            SphinxProcess.StartInfo.Arguments = "-c \" " + SphinxStartup + " \"";
            SphinxProcess.StartInfo.UseShellExecute = false;
            SphinxProcess.StartInfo.RedirectStandardOutput = true;
            SphinxProcess.Start();

            while (!SphinxProcess.StandardOutput.EndOfStream)
            {
                // Send the output to the MQ.
                var body = Encoding.UTF8.GetBytes(SphinxProcess.StandardOutput.ReadLine());
                Channel.BasicPublish(exchange: "",
                                     routingKey: "speech",
                                     basicProperties: null,
                                     body: body);
            }
        }
        #endregion
#endif
        #region Learning by my own design

        // When learning, create a new *.aeon file.
        public void PositData(Characteristic local, string pattern, string template)
        {
            // Save current state data to the xms system (overwrites in filesystem but only <template/> value in aeon's memory).
            //pattern = "TRADING SESSION";// Area of the brain.
            //template = "";// Content which includes, srai, star, emotion, etc. as: <srai>I am thinking about <star/></srai>
            // Also, more interestingly as: They are really smart animals.<think><set name="they"><set name="like"><set name="topic">CATS</set></set></set></think>
            // And: I like stories about robots the best.  <think><set name="it"><set name="like"><set name="topic">science fiction</set></set></set></think>
            const string filename = "holiday.aeon"; // Currently human-readable naming, but could be another since the program will read all the files in a given folder, which, in the current iteration is coded as "fragments" in the config file.
            var path = Configuration.PathToNucode + @"\" + filename;
            FileTemplate.PatternText = pattern.ToUpper();
            FileTemplate.TemplateText = template;
            FileTemplate.CreateFileTemplate();
            FileTemplate.WriteNuFile(filename, "learned-factiod");
            var fi = new FileInfo(Configuration.PathToNucode + @"\" + filename);
            if (fi.Exists)
            {
                var doc = new XmlDocument();
                try
                {
                    doc.Load(path);
                    _thisAeon.LoadAeonFromXml(doc, path);
                    // Will add a category thereby increasing aeon's size by the number of entries. Perhaps deleting old values is possible? Or how necessary?
                    Logging.WriteLog("Categories added.  Size is " + _thisAeon.Size + @" categories.", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                }
                catch
                {
                    Logging.WriteLog("Aeon failed to learn something new from the following: " + path, Logging.LogType.Error, Logging.LogCaller.Learn);
                }
            }
            // Do not delete since will use them when loading a fresh aeon with increased capacity.
        }

        #endregion

        #region Social features
        /// <summary>
        /// The method to speak the alone message.
        /// </summary>
        /// <param name="alone">if set to <c>true</c> [alone].</param>
        protected static void AloneMessage(bool alone)
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
        /// <summary>
        /// Check if Aeon is in the state of being alone.
        /// </summary>
        public static void CheckIfAeonIsAlone()
        {
            if (_thisAeon.IsAlone())
            {
                AloneMessage(true);
                AeonIsAlone = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
            }
        }
        private static void AloneEvent(object source, ElapsedEventArgs e)
        {
            CheckIfAeonIsAlone();
        }
        private static void AeonAloneText()
        {               
            SpinVariety();

            if (AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
                SpinVariety();

            PreviousAloneMessageOutput = AloneMessageOutput;
            SwitchMood = 1;
            Speak(_thisAeon.GlobalSettings.GrabSetting("alonesalutaion") + " " + _thisUser.UserName + ", " + AloneTextCurrent);
            System.Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("alonesalutaion") + " " +  _thisUser.UserName + ", " + AloneTextCurrent);
            AloneTextCurrent = _thisAeon.GlobalSettings.GrabSetting("alonemessage" + AloneMessageOutput);
        }
        private static void SpinVariety()
        {
            AloneMessageVariety = StaticRandom.Next(1, 2550);
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
            if (AloneMessageVariety.IsBetween(1251, 1500))
                AloneMessageOutput = 5;
            if (AloneMessageVariety.IsBetween(1501, 1750))
                AloneMessageOutput = 6;
            if (AloneMessageVariety.IsBetween(1751, 2000))
                AloneMessageOutput = 7;
            if (AloneMessageVariety.IsBetween(2001, 2250))
                AloneMessageOutput = 8;
            if (AloneMessageVariety.IsBetween(2251, 2550))
                AloneMessageOutput = 9;
        }
        #endregion

#if LinuxVoice
        #region Linux (including messaging-MQ for pocketsphinx)
        /// <summary>
        /// Here is where the output from pocketsphinx is being publically recieved and sent to the aeon.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="BasicDeliverEventArgs"/> instance containing the event data.</param>
        private static void PocketSphinxSpeechRecognized(object sender, BasicDeliverEventArgs e)
        {
            UserInput = Encoding.UTF8.GetString(e.Body);
            ProcessInput();
            Logging.WriteLog(LastOutput, Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
        }
        /// <summary>
        /// Initializes the messaging queue.
        /// </summary>
        /// <returns></returns>
        public static bool InitializeMessageQueue()
        {
            try
            {
                System.Console.WriteLine("Intializing the message queue...");
                Factory = new ConnectionFactory()
                {
                    HostName = _thisAeon.GlobalSettings.GrabSetting("messagingqueuehost")
                };
                var connection = Factory.CreateConnection();
                Channel = connection.CreateModel();
                Channel.QueueDeclare(queue: "speech",
                                         durable: false,
                                         exclusive: false,
                                         autoDelete: false,
                                         arguments: null);
                Consumer = new EventingBasicConsumer(Channel);
                Consumer.Received += PocketSphinxSpeechRecognized;
                Channel.BasicConsume(queue: "speech",
                         autoAck: true,
                         consumer: Consumer);
                System.Console.WriteLine("Message queue initialized.");
                return true;
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                return false;
            }

        }
        #endregion

#endif

        #region Speech synthesizer as per the OS
        /// <summary>
        /// Speak the text using a native voice synthesizer.
        /// </summary>
        /// <param name="input">The input.</param>
        public static void SpeakText(string input)
        {
            if (UsePythonBottle)
            {
                var message = new NameValueCollection
                {
                    ["speech"] = input
                };
                try
                {
                    HenryClient.UploadValues(_thisAeon.GlobalSettings.GrabSetting("bottleipaddress"), "POST", message);
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                    System.Console.WriteLine("No response from the animal.");
                }
            }
            if (SpeechSynthesizerUsed && PocketSphinxUsed)
            {
                #if LinuxVoice
                try
                {
                    PresenceSpeaker.Synthesize(input); // Linux
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                }
                #endif
            }
        }
        /// <summary>
        /// Speak the text using a native voice synthesizer.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="wait">Wait for reply.</param>
        private static void Speak(string input, bool wait = false)
        {
            if (UsePythonBottle)
            {
                var message = new NameValueCollection
                {
                    ["speech"] = input
                };
                try
                {
                    HenryClient.UploadValues(_thisAeon.GlobalSettings.GrabSetting("bottleipaddress"), "POST", message);
                }
                catch (Exception ex)
                {
                    Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                    System.Console.WriteLine("No response from the animal.");
                }
            }
            if (SpeechSynthesizerUsed)
            {
#if Windows
                // Pay attention to PS. https://github.com/PowerShell/PowerShell/issues/12236
                // This technique may be also useable for the speech recognizer.
                Execute($@"Add-Type -AssemblyName System.speech;  
                $speak = New-Object System.Speech.Synthesis.SpeechSynthesizer;                           
                $speak.Speak(""{input}"");");

                void Execute(string command)
                {
                    // Create a temp file with .ps1 extension.
                    var cFile = System.IO.Path.GetTempPath() + Guid.NewGuid() + ".ps1";

                    // Write the .ps1 file.
                    using var tw = new System.IO.StreamWriter(cFile, false, Encoding.UTF8);
                    tw.Write(command);

                    var start =
                        new System.Diagnostics.ProcessStartInfo()
                        {
                            FileName = "C:\\windows\\system32\\windowspowershell\\v1.0\\powershell.exe",                    
                            LoadUserProfile = false,
                            UseShellExecute = false,
                            CreateNoWindow = true,
                            Arguments = $"-executionpolicy bypass -File {cFile}",
                            WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden
                        };

                    // Start the process object.  
                    var p = System.Diagnostics.Process.Start(start);
                    // Be advised: The wait may not work...
                    if (wait) p.WaitForExit();
                }
#endif
            }
        }
        #endregion

        #region Tonal transposition of spoken output
#if Windows
        /// <summary>
        /// Transpose langauge to tonals. v.1.1 has eleven tones for twenty-six alphabet characters.
        /// </summary>
        /// <param name="input">The spoken input to be toned.</param>
        /// <remarks>Contains a code segment to keeps the aeon from toning too long. v1.1.0 feature.</remarks>
        public static void TransposeTonalSpeech(string input)
        {
            var words = input.Split(' ');
            if (TonalSpeechLimit)
            {
                if (words.Length > TonalSpeechLimitValue)
                {
                    try
                    {
                        Array.Resize(ref words, TonalSpeechLimitValue);
                    }
                    catch
                    {
                        System.Console.WriteLine("Something went wrong with the array resizing. If this persists, set the speech limit to false.");
                    }
                }
            }
            foreach (var word in words)
            {
                foreach (var letter in word)
                {
                    switch (letter.ToString().ToUpper())
                    {
                        case "A":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalA);
                            TonalResponse.Play();
                            break;
                        case "B":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalB);
                            TonalResponse.Play();
                            break;
                        case "C":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalC);
                            TonalResponse.Play();
                            break;
                        case "D":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalD);
                            TonalResponse.Play();
                            break;
                        case "E":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalE);
                            TonalResponse.Play();
                            break;
                        case "F":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalF);
                            TonalResponse.Play();
                            break;
                        case "G":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalFs);
                            TonalResponse.Play();
                            break;
                        case "H":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalG);
                            TonalResponse.Play();
                            break;
                        case "I":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalAp);
                            TonalResponse.Play();
                            break;
                        case "J":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalA);
                            TonalResponse.Play();
                            break;
                        case "K":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalB);
                            TonalResponse.Play();
                            break;
                        case "L":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalC);
                            TonalResponse.Play();
                            break;
                        case "M":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalD);
                            TonalResponse.Play();
                            break;
                        case "N":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalE);
                            TonalResponse.Play();
                            break;
                        case "O":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalF);
                            TonalResponse.Play();
                            break;
                        case "P":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalFs);
                            TonalResponse.Play();
                            break;
                        case "Q":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalG);
                            TonalResponse.Play();
                            break;
                        case "R":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalAp);
                            TonalResponse.Play();
                            break;
                        case "S":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalA);
                            TonalResponse.Play();
                            break;
                        case "T":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalB);
                            TonalResponse.Play();
                            break;
                        case "U":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalC);
                            TonalResponse.Play();
                            break;
                        case "V":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalD);
                            TonalResponse.Play();
                            break;
                        case "W":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalE);
                            TonalResponse.Play();
                            break;
                        case "X":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalF);
                            TonalResponse.Play();
                            break;
                        case "Y":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalFs);
                            TonalResponse.Play();
                            break;
                        case "Z":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalG);
                            TonalResponse.Play();
                            break;
                    }
                    Thread.Sleep(TonalDelay);
                }
            }
        }
#endif
        #endregion

        #region External robot and GPIO
        /// <summary>
        /// Format the input to a manner in which the python-to-Nao interface can understand.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public static string ParseForNaoSay(string input)
        {
            return input.Replace(' ', '\\');
        }
        /// <summary>
        /// Bind the GPIO ports directly to this application, sans scripts. Experimental.
        /// </summary>
        /// <returns></returns>
        public static bool BindGpio()
        {
            var gpio = new AeonGpio();
            for (var i = 0; i < 5; i++) //flash pin 17, 5 times on & off (1 second each)
            {
                // Open the eyes.
                gpio.OutputPin(AeonGpio.PinSets.P4, true);
                Thread.Sleep(1000);
                // Close they eyes.
                gpio.OutputPin(AeonGpio.PinSets.P6, false);
                Thread.Sleep(1000);
            }
            //Console.WriteLine( "Value of pin 18 is " + gpio.InputPin(FileGPIO.enumPIN.gpio18) ); //UNTESTED!
            gpio.CleanUpAllPins();
            return true;
        }
        #endregion

        #region Emotive processing
        /// <summary>
        /// Runs the blink routine.
        /// </summary>
        /// <param name="cycles">The number of cycles to blink.</param>
        public static void BlinkRoutine(int cycles)
        {
            CorrectExecution = Gpio.RunPythonScript("blink.py", cycles.ToString(), Configuration);
        }
#if Windows
        public static bool PlayTonalSequence(string filename)
        {
            try
            {
                var files = File.ReadAllText(TonalRootPath + "\\files\\" + filename, Encoding.UTF8);
                foreach(var value in files)
                {
                    switch (value.ToString())
                    {
                        case "A":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalA);
                            TonalResponse.Play();
                            break;
                        case "B":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalB);
                            TonalResponse.Play();
                            break;
                        case "C":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalC);
                            TonalResponse.Play();
                            break;
                        case "D":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalD);
                            TonalResponse.Play();
                            break;
                        case "E":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalE);
                            TonalResponse.Play();
                            break;
                        case "F":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalF);
                            TonalResponse.Play();
                            break;
                        case "Fs":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalFs);
                            TonalResponse.Play();
                            break;
                        case "G":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalG);
                            TonalResponse.Play();
                            break;
                        case "Ap":
                            TonalResponse = new SoundPlayer(ReturnTonalPath() + TonalAp);
                            TonalResponse.PlaySync();
                            break;
                            
                    }
                    Thread.Sleep(TonalDelay + 30);
                }
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.AeonRuntime);
                return false;
            }
            return true;
        }
        static string ReturnTonalPath()
        {
            return TonalRootPath + "\\tones\\";
        }
#endif
        #endregion

        #region Debugging
        protected static string GenerateAeonOutputDebug()
        {
            if (!Equals(null, _thisResult))
            {
                var result = new StringBuilder();

                foreach (var query in _thisResult.SubQueries)
                {
                    result.Append("Pattern: " + query.Trajectory + Environment.NewLine);
                    result.Append("Template: " + query.Template + Environment.NewLine);
                    result.Append("Emotion stars: ");
                    foreach (var emotion in query.EmotionStar)
                    {
                        result.Append(emotion + ", ");
                    }
                    result.Append(Environment.NewLine);
                    result.Append("Input stars: ");
                    foreach (var star in query.InputStar)
                    {
                        result.Append(star + ", ");
                    }
                    result.Append(Environment.NewLine);
                    result.Append("That stars: ");
                    foreach (var that in query.ThatStar)
                    {
                        result.Append(that + ", ");
                    }
                    result.Append(Environment.NewLine);
                    result.Append("Topic stars: ");
                    foreach (var topic in query.TopicStar)
                    {
                        result.Append(topic + ", ");
                    }
                }
                return result.ToString();
            }
            return "";
        }
        #endregion
    }
}
