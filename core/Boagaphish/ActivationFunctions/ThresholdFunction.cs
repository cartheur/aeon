//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.ActivationFunctions
{
    /// <summary>
    /// The threshold activation function. 
    /// </summary>
    /// <remarks>The class represents threshold activation function with the next expression:<br />
    /// <code>
    /// f(x) = 1, if x >= 0, otherwise 0
    /// </code>
    /// Output range of the function: <b>[0, 1]</b><br /><br />
    /// Functions graph:<br />
    /// <img src="threshold.bmp" width="242" height="172" />
    /// </remarks>
    public class ThresholdFunction : IActivationFunction
    {
        /// <summary>
        /// Calculates the function value. The method calculates function value at point <b>x</b>
        /// </summary>
        /// <param name="x">Function input value</param>
        /// <returns>Function output value, <i>f(x)</i></returns>
        public double Function(double x)
        {
            return (x >= 0) ? 1 : 0;
        }
        /// <summary>
        /// Not supported. (Why not?)
        /// </summary>
        /// <param name="x">Input value</param>
        /// <returns>Always returns 0</returns>
        /// <remarks>The method is not supported, because it is not possible to
        /// calculate derivative of the function.</remarks>
        public double Derivative(double x)
        {
            double y = Function(x);

            return 0;
        }
        /// <summary>
        /// Not supported. (Why not?)
        /// </summary>
        /// <param name="y">Input value</param>
        /// <returns>Always returns 0</returns>
        /// <remarks>The method is not supported, because it is not possible to
        /// calculate derivative of the function.</remarks>
        public double Derivative2(double y)
        {
            return 0;
        }
    }
}
