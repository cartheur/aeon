﻿//
// This AGI is the intellectual property of Dr. Christopher A. Tucker and Cartheur Research, B.V. Copyright 2008 - 2024, all rights reserved. No rights are explicitly granted to persons who have obtained this source code whose sole purpose is to illustrate the method of attaining AGI. Contact the company at: cartheur.research@pm.me.
//
using Aeon.Library;
using System.Diagnostics;
using System.Media;
using System.Timers;

namespace Aeon.Runtime
{
    internal class Program
    {
        // Configuration of the application.
        public static LoaderPaths Configuration;
        public static bool StartUpTheme { get; set; }
        public static string StartUpThemeFile { get; set; }
        public static bool TerminalMode { get; set; }
        public static Process TerminalProcess { get; set; }
        // Aeon's personal procedural items.
        private static Library.Aeon _thisAeon;
        private static Participant _thisParticipant;
        private static ParticipantRequest _thisRequest;
        private static ParticipantResult _thisResult;
        private static DateTime _aeonChatStartedOn;
        private static TimeSpan _aeonChatDuration;
        private static Thread _aeonAloneThread;
        private static Thread _moodicThread;
        // Aeon's status.
        private static bool SettingsLoaded { get; set; }
        private static bool AeonLoaded { get; set; }
        public static string ParticipantInput { get; set; }
        public static string AeonOutputDialogue { get; set; }
        public static string AeonOutputDebug { get; set; }
        public static int AloneMessageOutput { get; set; }
        public static int PreviousAloneMessageOutput { get; set; }
        public static int AloneMessageVariety { get; set; }
        public static string LastOutput { get; set; }
        private static string EmotiveEquation { get; set; }
        public int AeonSize { get; set; }
        public static string AeonType { get; set; }
        public static bool AeonIsAlone { get; set; }
        public static string AloneTextCurrent { get; set; }
        // Vocal introduction of the start-up experience. Uses System.Windows.Extensions.
        private static SoundPlayer AnimalActive { get; set; }

        static void Main(string[] args)
        {
            Configuration = new LoaderPaths("Debug");
            Logging.ActiveConfiguration = Configuration.ActiveRuntime;
            // Create the aeon and load its basic parameters from a config file.
            _thisAeon = new Library.Aeon("1+2i");
            _thisAeon.LoadSettings(Configuration.PathToSettings);
            SettingsLoaded = _thisAeon.LoadDictionaries(Configuration);
            _thisParticipant = new Participant(_thisAeon.GlobalSettings.GrabSetting("participantname"), _thisAeon);
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("product") + " - Version " + _thisAeon.GlobalSettings.GrabSetting("version") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("ip") + ".");
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("claim") + ".");
            Thread.Sleep(700);
            Console.WriteLine(_thisAeon.GlobalSettings.GrabSetting("warning"));
            Thread.Sleep(700);
            Console.WriteLine("------ Begin help ------");
            Console.WriteLine("If running in terminal mode, type 'quit' to leave and resume an aeon.");
            Thread.Sleep(700);
            Console.WriteLine("While in terminal mode, type 'exit' to quit the application entirely.");
            Thread.Sleep(700);
            Console.WriteLine("If you want to dissolve your aeon while in non-terminal mode, type 'aeon quit'.");
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
            TerminalMode = Convert.ToBoolean(_thisAeon.GlobalSettings.GrabSetting("terminalmode"));
            // Initialize the alone feature.
            _thisAeon.AeonAloneTimer = new System.Timers.Timer();
            _thisAeon.AeonAloneTimer.Elapsed += AloneEvent;
            _thisAeon.AeonAloneTimer.Interval = Convert.ToDouble(_thisAeon.GlobalSettings.GrabSetting("alonetimecheck"));
            _thisAeon.AeonAloneTimer.Enabled = false;
            _aeonAloneThread = new Thread(AeonAloneText);
            SharedFunctions.ThisAeon = _thisAeon;
            // Utilize the correct settings based on the aeon personality.
            if (_thisAeon.Name == "Rhodo" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            if (_thisAeon.Name == "Blank" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadBlank(Configuration);
            if (_thisAeon.Name == "Samantha" && SettingsLoaded)
                AeonLoaded = _thisAeon.LoadPersonality(Configuration);
            // Attach logging,  xms functionality, and spontaneous file generation.
            Logging.LogModelFile = _thisAeon.GlobalSettings.GrabSetting("logmodelfile");
            Logging.TranscriptModelFile = _thisAeon.GlobalSettings.GrabSetting("transcriptmodelfile");
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
            Console.WriteLine("Your aeon is ready for an interaction with you.");
            Console.WriteLine("Your transcript follows. Enjoy!");
            Console.WriteLine("**********************");
            if (TerminalMode)
            {
                Console.WriteLine("You have selected terminal mode.");
                if (_thisAeon.Size < 2)
                {
                    _thisAeon.LoadBlank(Configuration);
                    Console.WriteLine("No personality files have been loaded. A blank aeon has been loaded in its place.");
                }
                    
                // Todo: Trigger the terminal for typing commands.
                while (true && ParticipantInput != "quit")
                {
                    ParticipantInput = "";
                    ParticipantInput = Console.ReadLine();
                    if (ParticipantInput != null)
                        ProcessTerminal();
                    if (ParticipantInput == "quit")
                    {
                        Console.WriteLine("Leaving terminal mode and starting a conversational aeon.");
                        Thread.Sleep(2000);
                        break;
                    }
                    if (ParticipantInput == "exit")
                    {
                        Console.WriteLine("Leaving terminal mode and exiting the application.");
                        Thread.Sleep(2000);
                        Environment.Exit(0);
                        break;
                    }
                }
            }
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
            if (_thisAeon.IsAcceptingInput)
            {
                _aeonChatStartedOn = DateTime.Now;
                Thread.Sleep(250);
                var rawInput = ParticipantInput;
                if (rawInput.Contains("\n"))
                {
                    rawInput = rawInput.TrimEnd('\n');
                }
                Console.WriteLine(_thisParticipant.Name + ": " + rawInput);
                _thisRequest = new ParticipantRequest(rawInput, _thisParticipant, _thisAeon);
                _thisResult = _thisAeon.Chat(_thisRequest);
                Thread.Sleep(200);
                Console.WriteLine(_thisAeon.Name + ": " + _thisResult.Output);
                Logging.RecordTranscript(_thisParticipant.Name + ": " + rawInput);
                Logging.RecordTranscript(_thisAeon.Name + ": " + _thisResult.Output);
                // Record performance vectors for the result.
                _aeonChatDuration = DateTime.Now - _aeonChatStartedOn;
                Logging.WriteLog("Result search was conducted in: " + _aeonChatDuration.Seconds + @"." + _aeonChatDuration.Milliseconds + " seconds", Logging.LogType.Information, Logging.LogCaller.AeonRuntime);
                _thisAeon.AeonAloneTimer.Enabled = true;
                _thisAeon.AeonAloneStartedOn = DateTime.Now;
                AeonIsAlone = false;
                AeonOutputDialogue = _thisResult.Output;
                if (ParticipantInput == "exit")
                {
                    Console.WriteLine("Detected 'exit'...quitting the application.");
                    Thread.Sleep(2000);
                    Environment.Exit(0);
                }
            }
            else
            {
                ParticipantInput = string.Empty;
                Console.WriteLine("Aeon is not accepting user input." + Environment.NewLine);
            }
            return true;
        }

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
                //SetMoodic("alone");
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
            AloneMessageVariety = StaticRandom.Next(1, 1750);
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
            AloneTextCurrent = _thisAeon.GlobalSettings.GrabSetting("alonemessage" + AloneMessageOutput);
        }
        #endregion
    }
}