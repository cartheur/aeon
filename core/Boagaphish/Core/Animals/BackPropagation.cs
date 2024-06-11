//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using Boagaphish.ActivationFunctions;
using Boagaphish.Core.Layers;
using Boagaphish.Core.Learning;
using Boagaphish.Core.Networks;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Animals
{
    public class BackPropagation : ISupervised
    {
        private readonly ActivationNetwork _network;
        private double _learningRate;
        private double _momentum;
        private readonly double[][] _neuronErrors;
        private readonly double[][][] _weightsUpdates;
        private readonly double[][] _thresholdUpdates;

        public double LearningRate
        {
            get
            {
                return _learningRate;
            }
            set
            {
                _learningRate = Math.Max(0.0, Math.Min(1.0, value));
            }
        }

        public double Momentum
        {
            get
            {
                return _momentum;
            }
            set
            {
                _momentum = Math.Max(0.0, Math.Min(1.0, value));
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BackPropagation"/> class.
        /// </summary>
        /// <param name="network">The network.</param>
        /// <param name="learningRate">The learning rate.</param>
        public BackPropagation(ActivationNetwork network, double learningRate = 0.1)
        {
            _learningRate = learningRate;
            _network = network;
            _neuronErrors = new double[network.LayersCount][];
            _weightsUpdates = new double[network.LayersCount][][];
            _thresholdUpdates = new double[network.LayersCount][];
            int i = 0;
            for (int layersCount = network.LayersCount; i < layersCount; i++)
            {
                Layer layer = network[i];
                _neuronErrors[i] = new double[layer.NeuronsCount];
                _weightsUpdates[i] = new double[layer.NeuronsCount][];
                _thresholdUpdates[i] = new double[layer.NeuronsCount];
                for (int j = 0; j < layer.NeuronsCount; j++)
                {
                    _weightsUpdates[i][j] = new double[layer.InputsCount];
                }
            }
        }

        public double Run(double[] input, double[] desired)
        {
            _network.Compute(input);
            double result = CalculateError(desired);
            CalculateUpdates(input);
            UpdateNetwork();
            return result;
        }

        public double RunEpoch(double[][] input, double[][] desired)
        {
            double num = 0.0;
            int i = 0;
            for (int num2 = input.Length; i < num2; i++)
            {
                num += Run(input[i], desired[i]);
            }
            return num;
        }

        private double CalculateError(IList<double> desired)
        {
            double num = 0.0;
            int layersCount = _network.LayersCount;
            IActivationFunction activationFunction = _network[0][0].ActivationFunction;
            ActivationLayer activationLayer = _network[layersCount - 1];
            double[] array = _neuronErrors[layersCount - 1];
            int i = 0;
            for (int neuronsCount = activationLayer.NeuronsCount; i < neuronsCount; i++)
            {
                double output = activationLayer[i].Output;
                double num2 = desired[i] - output;
                array[i] = num2 * activationFunction.Derivative2(output);
                num += num2 * num2;
            }
            for (int num3 = layersCount - 2; num3 >= 0; num3--)
            {
                activationLayer = _network[num3];
                ActivationLayer activationLayer2 = _network[num3 + 1];
                array = _neuronErrors[num3];
                double[] array2 = _neuronErrors[num3 + 1];
                int j = 0;
                for (int neuronsCount2 = activationLayer.NeuronsCount; j < neuronsCount2; j++)
                {
                    double num4 = 0.0;
                    int k = 0;
                    for (int neuronsCount3 = activationLayer2.NeuronsCount; k < neuronsCount3; k++)
                    {
                        num4 += array2[k] * ((Neuron)activationLayer2[k])[j];
                    }
                    array[j] = num4 * activationFunction.Derivative2(activationLayer[j].Output);
                }
            }
            return num / 2.0;
        }

        private void CalculateUpdates(double[] input)
        {
            ActivationLayer activationLayer = _network[0];
            double[] array = _neuronErrors[0];
            double[][] array2 = _weightsUpdates[0];
            double[] array3 = _thresholdUpdates[0];
            int i = 0;
            for (int neuronsCount = activationLayer.NeuronsCount; i < neuronsCount; i++)
            {
                ActivationNeuron activationNeuron = activationLayer[i];
                double num = array[i];
                double[] array4 = array2[i];
                int j = 0;
                for (int inputsCount = activationNeuron.InputsCount; j < inputsCount; j++)
                {
                    array4[j] = _learningRate * (_momentum * array4[j] + (1.0 - _momentum) * num * input[j]);
                }
                array3[i] = _learningRate * (_momentum * array3[i] + (1.0 - _momentum) * num);
            }
            int k = 1;
            for (int layersCount = _network.LayersCount; k < layersCount; k++)
            {
                ActivationLayer activationLayer2 = _network[k - 1];
                activationLayer = _network[k];
                array = _neuronErrors[k];
                array2 = _weightsUpdates[k];
                array3 = _thresholdUpdates[k];
                int l = 0;
                for (int neuronsCount2 = activationLayer.NeuronsCount; l < neuronsCount2; l++)
                {
                    ActivationNeuron activationNeuron = activationLayer[l];
                    double num = array[l];
                    double[] array4 = array2[l];
                    int m = 0;
                    for (int inputsCount2 = activationNeuron.InputsCount; m < inputsCount2; m++)
                    {
                        array4[m] = _learningRate * (_momentum * array4[m] + (1.0 - _momentum) * num * activationLayer2[m].Output);
                    }
                    array3[l] = _learningRate * (_momentum * array3[l] + (1.0 - _momentum) * num);
                }
            }
        }

        private void UpdateNetwork()
        {
            int i = 0;
            for (int layersCount = _network.LayersCount; i < layersCount; i++)
            {
                ActivationLayer activationLayer = _network[i];
                double[][] array = _weightsUpdates[i];
                double[] array2 = _thresholdUpdates[i];
                int j = 0;
                for (int neuronsCount = activationLayer.NeuronsCount; j < neuronsCount; j++)
                {
                    ActivationNeuron activationNeuron = activationLayer[j];
                    double[] array3 = array[j];
                    int k = 0;
                    for (int inputsCount = activationNeuron.InputsCount; k < inputsCount; k++)
                    {
                        ActivationNeuron activationNeuron2 = activationNeuron;
                        int index = k;
                        ((Neuron)activationNeuron2)[index] += array3[k];
                    }
                    activationNeuron.Threshold += array2[j];
                }
            }
        }
    }
}
