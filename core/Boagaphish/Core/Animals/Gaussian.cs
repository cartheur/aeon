//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Format;

namespace Boagaphish.Core.Animals
{
    public static class Gaussian
    {
        private static readonly StaticRandom Generator = new StaticRandom();
        private const double Epsilon = 1E-06;

        public static double RandomGaussian()
        {
            return RandomGaussian(0.0, 1.0);
        }

        public static double RandomGaussian(double mean, double standardDeviation)
        {
            double result;
            double num;
            RandomGaussian(mean, standardDeviation, out result, out num);
            return result;
        }

        public static void RandomGaussian(double mean, double standardDeviation, out double valueOne, out double valueTwo)
        {
            double num;
            double num2;
            do
            {
                num = 2.0 * Generator.NextDouble() - 1.0;
                num2 = 2.0 * Generator.NextDouble() - 1.0;
            }
            while (num * num + num2 * num2 > 1.0 || (Math.Abs(num) < Epsilon && Math.Abs(num2) < Epsilon));
            var num3 = num * num + num2 * num2;
            var num4 = Math.Sqrt(-2.0 * Math.Log(num3) / num3);
            valueOne = standardDeviation * num * num4 + mean;
            valueTwo = standardDeviation * num2 * num4 + mean;
        }
    }
}
