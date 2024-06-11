//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Format;

namespace Boagaphish.Custom
{
    public static class Gaussian
    {
        private static readonly StaticRandom Generator = new StaticRandom();
        const double Epsilon = 1E-6;
        /// <summary>
        /// Gets a random gaussian.
        /// </summary>
        /// <returns></returns>
        public static double RandomGaussian()
        {
            return RandomGaussian(0.0, 1.0);
        }
        /// <summary>
        /// Gets a random gaussian.
        /// </summary>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <returns></returns>
        public static double RandomGaussian(double mean, double standardDeviation)
        {
            double valueOne, valueTwo;
            RandomGaussian(mean, standardDeviation, out valueOne, out valueTwo);
            return valueOne;
        }
        /// <summary>
        /// Gets a random gaussian.
        /// </summary>
        /// <param name="mean">The mean.</param>
        /// <param name="standardDeviation">The standard deviation.</param>
        /// <param name="valueOne">The value one.</param>
        /// <param name="valueTwo">The value two.</param>
        public static void RandomGaussian(double mean, double standardDeviation, out double valueOne,
            out double valueTwo)
        {
            double u, v;
            do
            {
                u = 2 * Generator.NextDouble() - 1;
                v = 2 * Generator.NextDouble() - 1;

            } while (u * u + v * v > 1 || (Math.Abs(u) < Epsilon && Math.Abs(v) < Epsilon));

            var s = u * u + v * v;
            var t = Math.Sqrt((-2.0 * Math.Log(s)) / s);
            valueOne = standardDeviation * u * t + mean;
            valueTwo = standardDeviation * v * t + mean;
        }
    }
}
