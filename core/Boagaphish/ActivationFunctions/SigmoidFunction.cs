//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.ActivationFunctions
{
    /// <summary>
    /// The sigmoid activation function. 
    /// </summary>
    /// <remarks>The class represents sigmoid activation function with the next expression:<br />
    /// <code>
    ///                1
    /// f(x) = ------------------
    ///        1 + exp(-alpha * x)
    ///
    ///           alpha * exp(-alpha * x )
    /// f'(x) = ---------------------------- = alpha * f(x) * (1 - f(x))
    ///           (1 + exp(-alpha * x))^2
    /// </code>
    /// Output range of the function: <b>[0, 1]</b><br /><br />
    /// Functions graph:<br />
    /// <img src="sigmoid.bmp" width="242" height="172" />
    /// </remarks>
    public class SigmoidFunction : IActivationFunction
    {
        // sigmoid's alpha value
        private double _alpha = 2;
        /// <summary>
        /// Sigmoid's alpha value
        /// </summary>
        /// <value>The alpha.</value>
        /// <remarks>The value determines steepness of the function. Default value: <b>2</b>.
        /// </remarks>
        public double Alpha { get { return _alpha; } set { _alpha = value; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="SigmoidFunction"/> class
        /// </summary>
        public SigmoidFunction() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="SigmoidFunction"/> class
        /// </summary>
        /// <param name="alpha">Sigmoid's alpha value</param>
        public SigmoidFunction(double alpha)
        {
            _alpha = alpha;
        }
        /// <summary>
        /// Calculates function value
        /// </summary>
        /// <param name="x">Function input value</param>
        /// <returns>Function output value, <i>f(x)</i></returns>
        /// <remarks>The method calculates function value at point <b>x</b>.</remarks>
        public double Function(double x)
        {
            return (1 / (1 + Math.Exp(-_alpha * x)));
        }
        /// <summary>
        /// Calculates the function derivative. The method calculates function derivative at point <b>x</b>.
        /// </summary>
        /// <param name="x">Function input value</param>
        /// <returns>Function derivative, <i>f'(x)</i></returns>
        public double Derivative(double x)
        {
            double y = Function(x);

            return (_alpha * y * (1 - y));
        }
        /// <summary>
        /// Calculates the derivative of a function.
        /// </summary>
        /// <param name="y">Function output value - the value, which was obtained with the help of <see cref="Function"/> method.</param>
        /// <returns>Function derivative, <i>f'(x)</i></returns>
        /// <remarks>The method calculates the same derivative value as the <see cref="Derivative"/> method, but it takes not the input <b>x</b> value itself, but the function value, which was calculated previously with the help of <see cref="Function"/> method. <i>(Some applications require as function value, as derivative value, so they can seve the amount of calculations using this method to calculate derivative)</i></remarks>
        public double Derivative2(double y)
        {
            return (_alpha * y * (1 - y));
        }
    }
}
