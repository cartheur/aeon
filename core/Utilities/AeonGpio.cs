//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// Class responsible for connecting to the motor controller, via the Gpio interface.
    /// </summary>
    public class AeonGpio
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="AeonGpio"/> is debug.
        /// </summary>
        /// <value>
        ///   <c>true</c> if debug; otherwise, <c>false</c>.
        /// </value>
        public static bool Debug { get; set; }

        /// <summary>
        /// The collection of pinsets for the Chip.
        /// </summary>
        public enum PinSets
        {
            /// <summary>
            /// Chip XIO-P0.
            /// </summary>
            P0 = 1013,
            /// <summary>
            /// Chip XIO-P1.
            /// </summary>
            P1 = 1014,
            /// <summary>
            /// Chip XIO-P2.
            /// </summary>
            P2 = 1015,
            /// <summary>
            /// Chip XIO-P3.
            /// </summary>
            P3 = 1016,
            /// <summary>
            /// Chip XIO-P4.
            /// </summary>
            P4 = 1017,
            /// <summary>
            /// Chip XIO-P5.
            /// </summary>
            P5 = 1018,
            /// <summary>
            /// Chip XIO-P6.
            /// </summary>
            P6 = 1019,
            /// <summary>
            /// Chip XIO-P7.
            /// </summary>
            P7 = 1020
        };
        /// <summary>
        /// In which direction is the data flowing?
        /// </summary>
        public enum PinDirection
        {
            /// <summary>
            /// An inward direction.
            /// </summary>
            In,
            /// <summary>
            /// An outward direction.
            /// </summary>
            Out
        };

        /// <summary>
        /// The gpio path.
        /// </summary>
        private const string GpioPath ="/sys/class/gpio/";
        // Contains a list of pins exported with an OUT direction.
        readonly List<PinSets> _outExported = new List<PinSets>();
        // Contains a list of pins exported with an IN direction.
        readonly List<PinSets> _inExported = new List<PinSets>(); 

        private void SetupPin(PinSets pin, PinDirection direction)
        {
	        // Unexport if it we're using it already
            if (_outExported.Contains(pin) || _inExported.Contains(pin)) UnexportPin(pin);
	        // Export.
            File.WriteAllText(GpioPath + "export", GetPinNumber(pin));

            if (Debug) Logging.WriteLog("Exporting pin " + pin + " as " + direction + ".", Logging.LogType.Information, Logging.LogCaller.Aeon);

            // set i/o direction
            File.WriteAllText(GpioPath + pin + "/direction", direction.ToString().ToLower());

            //record the fact that we've setup that pin
            if (direction == PinDirection.Out)
                _outExported.Add(pin);
            else
                _inExported.Add(pin);
        }
        /// <summary>
        /// Sets an output on the pin.
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <param name="value">if set to <c>true</c> [value].</param>
        public void OutputPin(PinSets pin, bool value)
        {
            // If we havent used the pin before,  or if we used it as an input before, set it up.
            if (!_outExported.Contains(pin) || _inExported.Contains(pin)) SetupPin(pin, PinDirection.Out);

            var writeValue = "0";
            if (value) writeValue = "1";
            File.WriteAllText(GpioPath + pin + "/value", writeValue);

            if (Debug) Logging.WriteLog("Output to pin " + pin + ", value was " + value + ".", Logging.LogType.Information, Logging.LogCaller.Aeon);
        }
        /// <summary>
        /// Sets an input on the pin.
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception"></exception>
        public bool InputPin(PinSets pin)
        {
            var returnValue = false;
            if (!_inExported.Contains(pin) || _outExported.Contains(pin)) SetupPin(pin, PinDirection.In);
         
            var filename = GpioPath + pin + "/value";
            if (File.Exists(filename))
            {
                var readValue = File.ReadAllText(filename);
                if (readValue.Length > 0 && readValue[0] == '1') returnValue = true;
            }
            else
                throw new Exception(string.Format("Cannot read from {0}. File does not exist", pin));

            if (Debug) Logging.WriteLog("Input from pin " + pin + ", value was " + returnValue + ".", Logging.LogType.Information, Logging.LogCaller.Aeon);

            return returnValue;
        }
        /// <summary>
        /// Unexports the pin.
        /// </summary>
        /// <param name="pin">The pin.</param>
        /// <remarks>if for any reason you want to unexport a particular pin use this, otherwise just call CleanUpAllPins when you're done</remarks>
        public void UnexportPin(PinSets pin)
        {
            var found = false;
            if (_outExported.Contains(pin))
            {
                found = true;
                _outExported.Remove(pin);
            }
            if (_inExported.Contains(pin))
            {
                found = true;
                _inExported.Remove(pin);
            }

            if (found)
            {
                File.WriteAllText(GpioPath + "unexport", GetPinNumber(pin));
                if (Debug) Logging.WriteLog("Unexporting  pin " + pin + ".", Logging.LogType.Information, Logging.LogCaller.Aeon);
            }
        }
        /// <summary>
        /// Cleans up all pins.
        /// </summary>
        public void CleanUpAllPins()
        {
            for (var p = _outExported.Count - 1; p >= 0; p--) UnexportPin(_outExported[p]); // Unexport in reverse order.
            for (var p = _inExported.Count - 1; p >= 0; p--) UnexportPin(_inExported[p]);
        }

        private static string GetPinNumber(PinSets pin)
        {
            return ((int) pin).ToString(CultureInfo.InvariantCulture) ; // Returns 17 for enum value of gpio17
        }
    }
}
