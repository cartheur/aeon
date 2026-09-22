//
// Copyright 2003-2026 Cartheur. All rights reserved. Reference-only use is permitted under the LICENSE file.
//
using System.Globalization;

namespace Aeon.Library
{
    /// <summary>
    /// The coordinates and characteristic block produced by the Fig. 7 instructional-displacement classifier.
    /// </summary>
    public sealed record InstructionalDisplacement(
        string Equation,
        double TrajectoryCoordinate,
        double EmotiveCoordinate,
        double TimeCoordinate,
        double Value,
        Characteristic Block);

    /// <summary>
    /// Safely evaluates a small arithmetic characteristic equation over trajectory (x), emotive (y), and time (t) coordinates.
    /// </summary>
    public static class InstructionalDisplacementClassifier
    {
        /// <summary>
        /// Creates a characteristic displacement. Equations support +, -, *, /, ^, parentheses, x, y, t, and implicit multiplication (for example, 3x).
        /// </summary>
        public static bool TryClassify(string equation, TrajectoryIndication trajectory, EmotiveIndication emotive, TimeSpan executionTime, out InstructionalDisplacement displacement)
        {
            displacement = null;
            if (string.IsNullOrWhiteSpace(equation) || trajectory == null)
            {
                return false;
            }

            try
            {
                double x = ToTrajectoryCoordinate(trajectory);
                double y = emotive?.Weight ?? 0;
                double t = Math.Max(0, executionTime.TotalSeconds);
                double value = new EquationParser(ExtractExpression(equation), x, y, t).Parse();
                if (!double.IsFinite(value))
                {
                    return false;
                }

                Characteristic[] blocks = Enum.GetValues<Characteristic>();
                int blockIndex = (int)Math.Floor(ToUnitInterval(value) * blocks.Length);
                displacement = new InstructionalDisplacement(equation, x, y, t, value, blocks[Math.Min(blockIndex, blocks.Length - 1)]);
                return true;
            }
            catch (ArgumentException)
            {
                return false;
            }
            catch (OverflowException)
            {
                return false;
            }
        }

        /// <summary>Returns a stable [0, 1) coordinate for an indication's normalized trajectory and raw input.</summary>
        public static double ToTrajectoryCoordinate(TrajectoryIndication trajectory)
        {
            ArgumentNullException.ThrowIfNull(trajectory);
            string text = string.Join("\n", trajectory.Paths ?? Enumerable.Empty<string>()) + "\n" + (trajectory.RawInput ?? string.Empty).Trim().ToUpperInvariant();
            uint hash = 2166136261;
            foreach (char character in text)
            {
                hash ^= character;
                hash *= 16777619;
            }
            return hash / ((double)uint.MaxValue + 1);
        }

        private static string ExtractExpression(string equation)
        {
            if (equation.Length > 256)
            {
                throw new ArgumentException("Characteristic equations are limited to 256 characters.", nameof(equation));
            }
            int equals = equation.IndexOf('=');
            return (equals >= 0 ? equation[(equals + 1)..] : equation).Trim();
        }

        private static double ToUnitInterval(double value)
        {
            double normalized = value - Math.Floor(value);
            return normalized < 0 ? normalized + 1 : normalized;
        }

        private sealed class EquationParser
        {
            private readonly string _expression;
            private readonly double _x;
            private readonly double _y;
            private readonly double _t;
            private int _position;

            public EquationParser(string expression, double x, double y, double t)
            {
                _expression = expression;
                _x = x;
                _y = y;
                _t = t;
            }

            public double Parse()
            {
                if (string.IsNullOrWhiteSpace(_expression))
                {
                    throw new ArgumentException("The characteristic equation is empty.");
                }
                double result = ParseSum();
                SkipWhitespace();
                if (_position != _expression.Length)
                {
                    throw new ArgumentException("The characteristic equation contains an unsupported token.");
                }
                return result;
            }

            private double ParseSum()
            {
                double value = ParseProduct();
                while (true)
                {
                    if (TryConsume('+')) value += ParseProduct();
                    else if (TryConsume('-')) value -= ParseProduct();
                    else return value;
                }
            }

            private double ParseProduct()
            {
                double value = ParsePower();
                while (true)
                {
                    if (TryConsume('*')) value *= ParsePower();
                    else if (TryConsume('/')) value /= ParsePower();
                    else if (StartsImplicitFactor()) value *= ParsePower();
                    else return value;
                }
            }

            private double ParsePower()
            {
                double value = ParseFactor();
                return TryConsume('^') ? Math.Pow(value, ParsePower()) : value;
            }

            private double ParseFactor()
            {
                if (TryConsume('+')) return ParseFactor();
                if (TryConsume('-')) return -ParseFactor();
                if (TryConsume('('))
                {
                    double value = ParseSum();
                    if (!TryConsume(')')) throw new ArgumentException("The characteristic equation has an unclosed parenthesis.");
                    return value;
                }

                SkipWhitespace();
                if (_position >= _expression.Length) throw new ArgumentException("The characteristic equation ends unexpectedly.");
                char token = char.ToLowerInvariant(_expression[_position]);
                if (token == 'x' || token == 'y' || token == 't')
                {
                    _position++;
                    return token == 'x' ? _x : token == 'y' ? _y : _t;
                }

                int start = _position;
                while (_position < _expression.Length && (char.IsDigit(_expression[_position]) || _expression[_position] == '.')) _position++;
                if (start == _position || !double.TryParse(_expression[start.._position], NumberStyles.Float, CultureInfo.InvariantCulture, out double number))
                {
                    throw new ArgumentException("The characteristic equation contains an invalid number.");
                }
                return number;
            }

            private bool StartsImplicitFactor()
            {
                SkipWhitespace();
                return _position < _expression.Length && (_expression[_position] == '(' || char.ToLowerInvariant(_expression[_position]) is 'x' or 'y' or 't');
            }

            private bool TryConsume(char expected)
            {
                SkipWhitespace();
                if (_position >= _expression.Length || _expression[_position] != expected) return false;
                _position++;
                return true;
            }

            private void SkipWhitespace()
            {
                while (_position < _expression.Length && char.IsWhiteSpace(_expression[_position])) _position++;
            }
        }
    }
}
