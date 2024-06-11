//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.ActivationFunctions
{
    /// <summary>
    /// The class represents bipolar sigmoid activation function with the next expression:<br />
    /// 
    ///                2
    /// f(x) = ------------------ - 1
    ///        1 + exp(-alpha * x)
    ///
    ///           2 * alpha * exp(-alpha * x )
    /// f'(x) = -------------------------------- = alpha * (1 - f(x)^2) / 2
    ///           (1 + exp(-alpha * x))^2
    /// 
    /// Output range of the function is [-1, 1].
    /// </summary>
    public class BipolarSigmoidFunction : IActivationFunction
    {
        private double _alpha = 2;
        /// <summary>
        /// Sigmoid's alpha value
        /// </summary>
        /// <value>The alpha.</value>
        /// <remarks>The value determines steepness of the function. Default value: <b>2</b>.
        /// </remarks>
        public double Alpha { get { return _alpha; } set { _alpha = value; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="BipolarSigmoidFunction"/> class. Output range of the function is [-1, 1].
        /// </summary>
        public BipolarSigmoidFunction() { }
        /// <summary>
        /// Initializes a new instance of the <see cref="BipolarSigmoidFunction"/> class. Output range of the function is [-1, 1].
        /// </summary>
        /// <param name="alpha">The sigmoid alpha value.</param>
        public BipolarSigmoidFunction(double alpha)
        {
            _alpha = alpha;
        }
        /// <summary>
        /// Calculates the function's value. The method calculates function value at point <b>x</b>.
        /// </summary>
        /// <param name="x">Function input value</param>
        /// <returns>Function output value, <i>f(x)</i>.</returns>
        public double Function(double x)
        {
            return ((2 / (1 + Math.Exp(-_alpha * x))) - 1);
        }
        /// <summary>
        /// Calculates the function's derivative. The method calculates function derivative at point <b>x</b>.
        /// </summary>
        /// <param name="x">Function input value</param>
        /// <returns>Function derivative, <i>f'(x)</i>.</returns>
        public double Derivative(double x)
        {
            var y = Function(x);

            return (_alpha * (1 - y * y) / 2);
        }
        /// <summary>
        /// Calculates the function's derivative. The method calculates the same derivative value as the <see cref="Derivative"/> method, but it takes not the input <b>x</b> value itself, but the function value, which was calculated previously with the help of <see cref="Function"/> method. <i>(Some applications require as function value, as derivative value, so they can seve the amount of calculations using this method to calculate derivative)</i>.
        /// </summary>
        /// <param name="y">Function output value - the value, which was obtained with the help of <see cref="Function"/> method.</param>
        /// <returns>Function derivative, <i>f'(x)</i>.</returns>
        public double Derivative2(double y)
        {
            return (_alpha * (1 - y * y) / 2);
        }
    }
}
