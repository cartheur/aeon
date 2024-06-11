//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.Collections.Generic;
using Boagaphish.Core.Networks;
using Boagaphish.Numeric;
using BackPropagationNetwork = Boagaphish.Core.Animals.BackPropagationNetwork;

namespace Boagaphish.Core
{
    public static class Extensions
    {
        static ArrayList Input { get; set; }
        static ArrayList FirstInputWeights { get; set; }
        static double FirstSum { get; set; }
        static double FirstResult { get; set; }
        static ArrayList SecondInputWeights { get; set; }
        static double SecondSum { get; set; }
        static double SecondResult { get; set; }
        static double ThirdSum { get; set; }
        static double Summy { get; set; }
        static double AverageSummy { get; set; }
        static double X { get; set; }
        static double Softmax { get; set; }

        public static double ComputeSoftmax(this BackPropagationNetwork network, Network.CrossLayer crossLayer, double[] input)
        {
            //var output = 0.0;

            Input = new ArrayList();
            FirstInputWeights = new ArrayList();
            SecondInputWeights = new ArrayList();
            // Create the array list.
            for (int i = 0; i < input.Length; i++)
            {
                Input.Add(input[i]);
            }
            // Process the inputs and sums for the first set.
            for (int i = 0; i < input.Length; i++)
            {
                if (i != i % 2)
                {
                    for (int j = 0; j < network.Weight[0][i].Length; j++)
                    {
                        FirstInputWeights.Add(network.Weight[0][i][j]);
                    }
                    FirstSum += Convert.ToDouble(Input[i]) * Convert.ToDouble(FirstInputWeights[i]);
                    FirstSum += Convert.ToDouble(network.Bias[0][i]);
                    FirstInputWeights = new ArrayList();
                }
                if (i == i % 2)
                {
                    for (int j = 0; j < network.Weight[0][i].Length; j++)
                    {
                        SecondInputWeights.Add(network.Weight[0][i][j]);
                    }
                    SecondSum += Convert.ToDouble(Input[i]) * Convert.ToDouble(SecondInputWeights[i]);
                    SecondSum += Convert.ToDouble(network.Bias[0][i]);
                    SecondInputWeights = new ArrayList();
                }
            }
            // Run an evaluation on the sums based on the chosen governing ODE.
            FirstResult = FirstSum.Evaluate(network.TransferFunction[0]);
            SecondResult = SecondSum.Evaluate(network.TransferFunction[0]);
            // Process the next set of sums.
            for (int k = 0; k < network.Weight[0][1].Length; k++)
            {
                Summy += Convert.ToDouble(network.Weight[1][k][0]);
                AverageSummy = Summy / network.Weight[0][1].Length;
                ThirdSum += Convert.ToDouble(network.Bias[0][k]);
            }
            ThirdSum += (FirstResult * AverageSummy) + (SecondResult * AverageSummy);
            // Finally, compute the softmax result. Determine the maximum value.
            double max = Double.MinValue;
            if (crossLayer == Network.CrossLayer.InputHidden)
                max = (FirstSum > SecondSum) ? FirstSum : SecondSum;
            else if (crossLayer == Network.CrossLayer.HiddenOutput)
                max = (ThirdSum > ThirdSum) ? ThirdSum : ThirdSum;
            // Compute the scale.
            double scale = 0.0;
            if (crossLayer == Network.CrossLayer.InputHidden)
                scale = Math.Exp(FirstSum - max) + Math.Exp(SecondSum - max);
            else if (crossLayer == Network.CrossLayer.HiddenOutput)
                scale = Math.Exp(ThirdSum - max) + Math.Exp(ThirdSum - max);
            X = 0.5 * (FirstResult + SecondResult);
            // Return to Softmax.
            Softmax = Math.Exp(X - max) / scale;

            return Softmax;
        }
        public static double Evaluate(this double value, TransferFunction transferFunction)
        {
            return TransferFunctions.Evaluate(transferFunction, value);
        }

        public static bool IsGreaterThan(this double value, double comparisonValue)
        {
            if (value > comparisonValue)
                return true;
            return false;
        }
        public static bool IsLessThan(this double value, double comparisonValue)
        {
            if (value < comparisonValue)
                return true;
            return false;
        }
        public static bool IsGreaterThanPrevious(this double[] input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value > previousValue)
                return true;
            return false;
        }
        public static bool IsLessThanPrevious(this double[] input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value < previousValue)
                return true;
            return false;
        }
        /// <summary>
        /// Determines whether the specified value is between a minimum-maximum range (inclusive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns></returns>
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        public static bool IsLessThanPrevious(this List<double> input)
        {
            var previousValue = 0.0;
            var value = 0.0;
            for (var i = 0; i < 1; i++)
            {
                value = input[0];
                previousValue = input[1];
            }
            if (value < previousValue)
                return true;
            return false;
        }
    }
}
