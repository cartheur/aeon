//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.ActivationFunctions;
using Boagaphish.Core.Layers;
using Boagaphish.Core.Networks;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Learning
{
    /// <summary>
    /// Perceptron learning algorithm
    /// </summary>
    /// <remarks>This learning algorithm is used to train one layer neural network of <see cref="ActivationNeuron">Activation Neurons</see> with the <see cref="ThresholdFunction">Threshold</see> activation function.</remarks>
    public class Perceptron : ISupervised
    {
        // network to teach
        private ActivationNetwork network;
        /// <summary>
        /// The tolerance value for network minimum value comparisions.
        /// </summary>
        public double Epsilon = 1E-6;
        // learning rate
        private double _learningRate = 0.1;
        /// <summary>
        /// Learning rate
        /// </summary>
        /// <value>The learning rate.</value>
        /// <remarks>The value determines speed of learning in the range of [0, 1].
        /// Default value equals to 0.1.</remarks>
        public double LearningRate
        {
            get { return _learningRate; }
            set
            {
                _learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Perceptron"/> class
        /// </summary>
        /// <param name="network">Network to teach</param>
        public Perceptron(ActivationNetwork network)
        {
            // check layers count
            if (network.LayersCount != 1)
            {
                throw new ArgumentException("Invalid nuaral network. It should have one layer only.");
            }

            this.network = network;
        }
        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// <param name="input">input vector</param>
        /// <param name="desired">desired output vector</param>
        /// <returns>
        /// Returns absolute error - difference between real output and
        /// desired output
        /// </returns>
        /// <remarks>Runs one learning iteration and updates neuron's
        /// weights in case if neuron's output does not equal to the
        /// desired output.</remarks>
        public double Run(double[] input, double[] desired)
        {
            // compute output of network
            double[] networkOutput = network.Compute(input);

            // get the only layer of the network
            ActivationLayer layer = network[0];

            // summary network absolute error
            double error = 0.0;

            // check output of each neuron and update weights
            for (int j = 0, k = layer.NeuronsCount; j < k; j++)
            {
                double e = desired[j] - networkOutput[j];

                if (Math.Abs(e) > Epsilon)
                {
                    ActivationNeuron perceptron = layer[j];

                    // update weights
                    for (int i = 0, n = perceptron.InputsCount; i < n; i++)
                    {
                        perceptron[i] += _learningRate * e * input[i];
                    }

                    // update threshold value
                    perceptron.Threshold += _learningRate * e;

                    // make error to be absolute
                    error += Math.Abs(e);
                }
            }

            return error;
        }
        /// <summary>
        /// Runs learning epoch
        /// </summary>
        /// <param name="input">array of input vectors</param>
        /// <param name="desired">array of output vectors</param>
        /// <returns>Returns sum of absolute errors</returns>
        /// <remarks>Runs series of learning iterations - one iteration
        /// for each input sample. Updates neuron's weights each time,
        /// when neuron's output does not equal to the desired output.</remarks>
        public double RunEpoch(double[][] input, double[][] desired)
        {
            double error = 0.0;

            // run learning procedure for all samples
            for (int i = 0, n = input.Length; i < n; i++)
            {
                error += Run(input[i], desired[i]);
            }

            // return summary error
            return error;
        }
    }
}
