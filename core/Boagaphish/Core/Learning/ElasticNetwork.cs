//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Core.Layers;
using Boagaphish.Core.Networks;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Learning
{
    /// <summary>
    /// The elastic network learning algorithm.
    /// </summary>
    /// <remarks>This class implements elastic network's learning algorithm and allows to train <see cref="DistanceNetwork">Distance Networks</see>.</remarks>
    public class ElasticNetwork : IUnsupervised
    {
        // neural network to train
        private readonly DistanceNetwork _network;
        // array of distances between neurons
        private readonly double[] _distance;
        // learning rate
        private double _learningRate = 0.1;
        // learning radius
        private double _learningRadius = 0.5;
        // squared learning radius multiplied by 2 (precalculated value to speed up computations)
        private double _squaredRadius2 = 2 * 7 * 7;
        /// <summary>
        /// Learning rate
        /// </summary>
        /// <value>The learning rate.</value>
        /// <remarks>Determines speed of learning. Value range is [0, 1].
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
        /// Learning radius
        /// </summary>
        /// <value>The learning radius.</value>
        /// <remarks>Determines the amount of neurons to be updated around
        /// winner neuron. Neurons, which are in the circle of specified radius,
        /// are updated during the learning procedure. Neurons, which are closer
        /// to the winner neuron, get more update.<br/><br/>
        /// Default value equals to 0.5.</remarks>
        public double LearningRadius
        {
            get { return _learningRadius; }
            set
            {
                _learningRadius = Math.Max(0, Math.Min(1.0, value));
                _squaredRadius2 = 2 * _learningRadius * _learningRadius;
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ElasticNetwork"/> class
        /// </summary>
        /// <param name="network">Neural network to train</param>
        public ElasticNetwork(DistanceNetwork network)
        {
            _network = network;

            // precalculate distances array
            int neurons = network[0].NeuronsCount;
            double deltaAlpha = Math.PI * 2.0 / neurons;
            double alpha = deltaAlpha;

            _distance = new double[neurons];
            _distance[0] = 0.0;

            // calculate all distance values
            for (int i = 1; i < neurons; i++)
            {
                double dx = 0.5 * Math.Cos(alpha) - 0.5;
                double dy = 0.5 * Math.Sin(alpha);

                _distance[i] = dx * dx + dy * dy;

                alpha += deltaAlpha;
            }
        }
        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// <param name="input">input vector</param>
        /// <returns>
        /// Returns learning error - summary absolute difference between updated
        /// weights and according inputs. The difference is measured according to the neurons
        /// distance to the winner neuron.
        /// </returns>
        public double Run(double[] input)
        {
            double error = 0.0;

            // compute the network
            _network.Compute(input);
            int winner = _network.GetWinner();

            // get layer of the network
            Layer layer = _network[0];

            // walk through all neurons of the layer
            for (int j = 0, m = layer.NeuronsCount; j < m; j++)
            {
                Neuron neuron = layer[j];

                // update factor
                double factor = Math.Exp(-_distance[Math.Abs(j - winner)] / _squaredRadius2);

                // update weight of the neuron
                for (int i = 0, n = neuron.InputsCount; i < n; i++)
                {
                    // calculate the error
                    double e = (input[i] - neuron[i]) * factor;
                    error += Math.Abs(e);
                    // update weight
                    neuron[i] += e * _learningRate;
                }
            }
            return error;
        }
        /// <summary>
        /// Runs learning epoch
        /// </summary>
        /// <param name="input">array of input vectors</param>
        /// <returns>
        /// Returns summary learning error for the epoch. See <see cref="Run"/>
        /// method for details about learning error calculation.
        /// </returns>
        public double RunEpoch(double[][] input)
        {
            double error = 0.0;

            // walk through all training samples
            foreach (double[] sample in input)
            {
                error += Run(sample);
            }

            // return summary error
            return error;
        }
    }
}
