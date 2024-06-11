//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.Numeric
{
    public enum TransferFunction
    {
        None,
        BipolarSigmoid,
        Gaussian,
        Linear,
        NormalizedExponent,
        RationalSigmoid,
        Sigmoid,
        VanderPol
    }
    public static class TransferFunctions
    {
        public static double Alpha { get; set; }
        public static double Mu { get; set; }
        /// <summary>
        /// Evaluates a specified transfer function.
        /// </summary>
        /// <param name="tFunc">The transfer function.</param>
        /// <param name="input">The input.</param>
        /// <param name="mu">Parameter for use in the van der Pol oscillator function.</param>
        /// <returns></returns>
        public static double Evaluate(TransferFunction tFunc, double input, double mu = 0.5)
        {
            Mu = mu;
            switch (tFunc)
            {
                case TransferFunction.BipolarSigmoid:
                    return BipolarSigmoid(input);
                case TransferFunction.Gaussian:
                    return Gaussian(input);
                case TransferFunction.Linear:
                    return Linear(input);
                case TransferFunction.RationalSigmoid:
                    return RationalSigmoid(input);
                case TransferFunction.Sigmoid:
                    return Sigmoid(input);
                case TransferFunction.VanderPol:
                    return VanderPol(input);
                default:
                    return 0.0;
            }
        }
        /// <summary>
        /// Evaluates the derivative.
        /// </summary>
        /// <param name="tFunc">The transfer function.</param>
        /// <param name="input">The input.</param>
        /// <param name="mu">Parameter for use in the van der Pol oscillator function.</param>
        /// <returns></returns>
        public static double EvaluateDerivative(TransferFunction tFunc, double input, double mu = 0.5)
        {
            Mu = mu;
            switch (tFunc)
            {
                case TransferFunction.BipolarSigmoid:
                    return BipolarSigmoidDerivative(input);
                case TransferFunction.Gaussian:
                    return GaussianDerivative(input);
                case TransferFunction.Linear:
                    return LinearDerivative(input);
                case TransferFunction.RationalSigmoid:
                    return RationalSigmoidDerivative(input);
                case TransferFunction.Sigmoid:
                    return SigmoidDerivative(input);
                case TransferFunction.VanderPol:
                    return VanderPolDerivative(input);
                default:
                    return 0.0;
            }
        }
        // Implementation of the different functions.
        static double BipolarSigmoid(double x)
        {
            return ((2 / (1 + Math.Exp(-Alpha * x))) - 1);
        }
        static double BipolarSigmoidDerivative(double x)
        {
            var y = BipolarSigmoid(x);
            return (Alpha * (1 - y * y) / 2);
        }
        static double BipolarSigmoidDerivative2(double y)
        {
            return (Alpha * (1 - y * y) / 2);
        }
        static double Gaussian(double x)
        {
            return Math.Exp(-Math.Pow(x, 2));
        }
        static double GaussianDerivative(double x)
        {
            return -2.0 * x * Gaussian(x);
        }
        static double Linear(double x)
        {
            return x;
        }
        static double LinearDerivative(double x)
        {
            return 1.0;
        }
        static double RationalSigmoid(double x)
        {
            return x / (1.0 + Math.Sqrt(1.0 + x * x));
        }
        static double RationalSigmoidDerivative(double x)
        {
            var value = Math.Sqrt(1.0 + x * x);
            return 1.0 / (value * (1 + value));
        }
        static double Sigmoid(double x)
        {
            return 1.0 / (1.0 + Math.Exp(-x));
        }
        static double SigmoidDerivative(double x)
        {
            return Sigmoid(x) * (1 - Sigmoid(x));
        }
        static double VanderPol(double x)
        {
            return (1 / Mu) * x;
        }
        static double VanderPolDerivative(double x)
        {
            var y = VanderPol(x);
            return Math.Abs(Mu * (x - (0.3333 * Math.Pow(x, 3)) - y));
        }
    }
}
