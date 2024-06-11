        
    class AloneClassSnippet
    {
        /// <summary>
        /// The method to speak the alone message.
        /// </summary>
        /// <param name="alone">if set to <c>true</c> [alone].</param>
        // protected static void AloneMessage(bool alone)
        // {
        //     if (alone)
        //     {
        //         if (!_aeonAloneThread.IsAlive)
        //         {
        //             _aeonAloneThread = new Thread(AeonAloneText) { IsBackground = true };
        //             _aeonAloneThread.Start();
        //         }
        //     }
        // }
        /// <summary>
        /// Check if Aeon is in the state of being alone.
        /// </summary>
        // public static void CheckIfAeonIsAlone()
        // {
        //     if (_thisAeon.IsAlone())
        //     {
        //         AloneMessage(true);
        //         AeonIsAlone = true;
        //         _thisAeon.AeonAloneStartedOn = DateTime.Now;
        //     }
        // }
        // private static void AloneEvent(object source, ElapsedEventArgs e)
        // {
        //     CheckIfAeonIsAlone();
        // }
        // private static void AeonAloneText()
        // {               
        //     AloneMessageVariety = StaticRandom.Next(1, 1750);
        //     if (AloneMessageVariety.IsBetween(1, 250) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 0;
        //     if (AloneMessageVariety.IsBetween(1, 250) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageVariety = StaticRandom.Next(251, 1500);
        //     if (AloneMessageVariety.IsBetween(251, 500) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 1;
        //     if (AloneMessageVariety.IsBetween(251, 500) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageVariety = StaticRandom.Next(501, 1750);
        //     if (AloneMessageVariety.IsBetween(501, 750) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 2;
        //     if (AloneMessageVariety.IsBetween(501, 750) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageVariety = StaticRandom.Next(751, 1500);
        //     if (AloneMessageVariety.IsBetween(751, 1000) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 3;
        //     if (AloneMessageVariety.IsBetween(751, 1000) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageVariety = StaticRandom.Next(1001, 1750);
        //     if (AloneMessageVariety.IsBetween(1001, 1250) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 4;
        //     if (AloneMessageVariety.IsBetween(1001, 1250) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 5;
        //     if (AloneMessageVariety.IsBetween(1251, 1500) && !AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 6;
        //     if (AloneMessageVariety.IsBetween(1551, 1750) && AloneMessageOutput.HasAppearedBefore(PreviousAloneMessageOutput))
        //         AloneMessageOutput = 7;

        //     PreviousAloneMessageOutput = AloneMessageOutput;
        //     AloneMessageOutput = 0;
        //     SwitchMood = 1;
        //     Speak(_thisAeon.GlobalSettings.GrabSetting("alonesalutaion") + "," + _thisUser.UserName + ", " + AloneTextCurrent);
        //     AloneTextCurrent = _thisAeon.GlobalSettings.GrabSetting("alonemessage" + AloneMessageOutput);
        // }
    }