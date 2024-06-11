//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Boagaphish.Numeric;

namespace Boagaphish.Custom
{
    /// <summary>
    /// A personalized back-propagation network biased on my ideas.
    /// </summary>
    public class BackPropagationNetwork
    {
        // Network parameters
        private int _layerCount;
        private int _inputSize;
        private int[] _layerSize;
        private double[][] _layerOutput;
        private double[][] _layerInput;
        private double[][] _delta;
        private double[][] _previousBiasDelta;
        private double[][][] _previousWeightDelta;
        private XmlDocument _document;
        // Network variables
        public TransferFunction[] TransferFunction;
        public double[][] Bias;
        public double[][][] Weight;
        // Softmax/scale variables
        public double[] ZerothSum;
        public double[] FirstSum;
        public double FirstToSecondLayerResult;
        public double NextToOutputLayerResult;
        /// <summary>
        /// The name of the network.
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="BackPropagationNetwork" /> class.
        /// </summary>
        /// <param name="layerSizes">The layer sizes.</param>
        /// <param name="transferFunctions">The transfer functions.</param>
        /// <param name="name">The name of the network.</param>
        /// <exception cref="System.ArgumentException">(Cannot initiate on those parameters.)</exception>
        public BackPropagationNetwork(IList<int> layerSizes, IList<TransferFunction> transferFunctions, string name = "Candles")
        {
            // Tag the network.
            Name = name;
            // Validate the input data.
            if (transferFunctions.Count != layerSizes.Count || transferFunctions[0] != Numeric.TransferFunction.None)
                throw new ArgumentException(("Cannot initiate on those parameters."));

            // Initialize layers.
            _layerCount = layerSizes.Count - 1;
            _inputSize = layerSizes[0];
            _layerSize = new int[_layerCount];

            for (int i = 0; i < _layerCount; i++)
                _layerSize[i] = layerSizes[i + 1];

            TransferFunction = new TransferFunction[_layerCount];
            for (int i = 0; i < _layerCount; i++)
                TransferFunction[i] = transferFunctions[i + 1];

            // Start dimensioning arrays.
            Bias = new double[_layerCount][];
            _previousBiasDelta = new double[_layerCount][];
            _delta = new double[_layerCount][];
            _layerOutput = new double[_layerCount][];
            _layerInput = new double[_layerCount][];
            Weight = new double[_layerCount][][];
            _previousWeightDelta = new double[_layerCount][][];
            // Fill in 2-dimensional arrays.
            for (int l = 0; l < _layerCount; l++)
            {
                Bias[l] = new double[_layerSize[l]];
                _previousBiasDelta[l] = new double[_layerSize[l]];
                _delta[l] = new double[_layerSize[l]];
                _layerOutput[l] = new double[_layerSize[l]];
                _layerInput[l] = new double[_layerSize[l]];

                Weight[l] = new double[l == 0 ? _inputSize : _layerSize[l - 1]][];
                _previousWeightDelta[l] = new double[l == 0 ? _inputSize : _layerSize[l - 1]][];

                for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                {
                    Weight[l][i] = new double[_layerSize[l]];
                    _previousWeightDelta[l][i] = new double[_layerSize[l]];
                }
            }
            // Intiailze the weights.
            for (int l = 0; l < _layerCount; l++)
            {
                for (int j = 0; j < _layerSize[l]; j++)
                {
                    Bias[l][j] = Gaussian.RandomGaussian();
                    _previousBiasDelta[l][j] = 0.0;
                    _layerOutput[l][j] = 0.0;
                    _layerInput[l][j] = 0.0;
                    _delta[l][j] = 0.0;
                }

                for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                {
                    for (int j = 0; j < _layerSize[l]; j++)
                    {
                        Weight[l][i][j] = Gaussian.RandomGaussian();
                        _previousWeightDelta[l][i][j] = 0.0;
                    }
                }
            }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="BackPropagationNetwork"/> class.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public BackPropagationNetwork(string filePath)
        {
            LoadNetworkXml(filePath);
        }
        protected BackPropagationNetwork()
        {
            // Here for the CustomNetwork class.
        }
        /// <summary>
        /// Sums weights and biases in the layers for evaluation against the transfer function at the output.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="output">The output.</param>
        /// <exception cref="System.ArgumentException">Input data is not of the correct dimension.</exception>
        public void RunEvaluation(ref double[] input, out double[] output)
        {
            if (input.Length != _inputSize)
                throw new ArgumentException("Input data is not of the correct dimension.");
            // Dimension.
            output = new double[_layerSize[_layerCount - 1]];
            // Run the network.
            for (var l = 0; l < _layerCount; l++)
            {
                for (var j = 0; j < _layerSize[l]; j++)
                {
                    var sum = 0.0;
                    for (var i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                        sum += Weight[l][i][j] * (l == 0 ? input[i] : _layerOutput[l - 1][i]);

                    sum += Bias[l][j];
                    _layerInput[l][j] = sum;
                    _layerOutput[l][j] = TransferFunctions.Evaluate(TransferFunction[l], sum);
                }
            }
            // Copy the output to the output array.
            for (var i = 0; i < _layerSize[_layerCount - 1]; i++)
                output[i] = _layerOutput[_layerCount - 1][i];

        }
        /// <summary>
        /// Computes the softmax. (Unfinished)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public double ComputeSoftmax(double[] input, string layer)
        {
            var result = 0.0;
            for (var i = 0; i < _layerCount; i++)
            {
                ZerothSum[i] += (input[i] * Weight[0][0][i] /*ihWeight00*/) + (input[i] * Weight[i][0][0] /*ihWeight10*/);
                ZerothSum[i] += Bias[0][i];// ihBias0;
                FirstSum[i] += (input[i] * Weight[0][i][0] /*ihWeight01*/) + (input[i] * Weight[i][i][0] /*ihWeight11*/);
                FirstSum[i] += Bias[i][0];// ihBias1;
                FirstToSecondLayerResult = Softmax(ZerothSum[i], "ih");
                NextToOutputLayerResult = Softmax(FirstSum[i], "ho");
                result = 0.5 * Math.Pow(FirstToSecondLayerResult * NextToOutputLayerResult, 2);
            }
            return result;
        }
        protected double Softmax(double x, string layer)
        {
            // Determine the maximum value.
            double max = double.MinValue;
            if (layer == "ih")
                max = (ZerothSum[0] > ZerothSum[1]) ? ZerothSum[0] : ZerothSum[1];
            else if (layer == "ho")
                max = (FirstSum[0] > FirstSum[1]) ? FirstSum[0] : FirstSum[1];
            // Compute the scale.
            double scale = 0.0;
            if (layer == "ih")
                scale = Math.Exp(ZerothSum[0] - max) + Math.Exp(ZerothSum[1] - max);
            else if (layer == "ho")
                scale = Math.Exp(FirstSum[0] - max) + Math.Exp(FirstSum[1] - max);

            return Math.Exp(x - max) / scale;
        }
        /// <summary>
        /// Trains the network using the specified input.
        /// </summary>
        /// <param name="input">The input data.</param>
        /// <param name="desired">The desired output.</param>
        /// <param name="trainingRate">The training rate.</param>
        /// <param name="momentum">The momentum.</param>
        /// <returns>Training error.</returns>
        /// <exception cref="System.ArgumentException">
        /// Invalid input parameter;input
        /// or
        /// Invalid input parameter;desired
        /// </exception>
        public double Train(ref double[] input, ref double[] desired, double trainingRate, double momentum)
        {
            // Parameter validation.
            if (input.Length != _inputSize)
                throw new ArgumentException("Invalid input parameter", "input");
            if (desired.Length != _layerSize[_layerCount - 1])
                throw new ArgumentException("Invalid input parameter", "desired");
            // Local variables.
            var error = 0.0;
            var output = new double[_layerSize[_layerCount - 1]];
            // Run the network.
            RunEvaluation(ref input, out output);
            // Back-propagate the error.
            for (var l = _layerCount - 1; l >= 0; l--)
            {
                // Output layer
                if (l == _layerCount - 1)
                {
                    for (var k = 0; k < _layerSize[l]; k++)
                    {
                        _delta[l][k] = output[k] - desired[k];
                        error += Math.Pow(_delta[l][k], 2);
                        _delta[l][k] *= TransferFunctions.EvaluateDerivative(TransferFunction[l], _layerInput[l][k]);
                    }
                }
                else // Hidden layer
                {
                    for (var i = 0; i < _layerSize[l]; i++)
                    {
                        double sum = 0.0;
                        for (int j = 0; j < _layerSize[l + 1]; j++)
                        {
                            sum += Weight[l + 1][i][j] * _delta[l + 1][j];
                        }
                        sum *= TransferFunctions.EvaluateDerivative(TransferFunction[l], _layerInput[l][i]);
                        _delta[l][i] = sum;

                    }
                }
            }
            // Update the weights and biases.
            for (int l = 0; l < _layerCount; l++)
                for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                    for (int j = 0; j < _layerSize[l]; j++)
                    {
                        double weightDelta = trainingRate * _delta[l][j] * (l == 0 ? input[i] : _layerOutput[l - 1][i])
                                             + momentum * _previousWeightDelta[l][i][j];
                        Weight[l][i][j] -= weightDelta;

                        _previousWeightDelta[l][i][j] = weightDelta;
                    }

            for (int l = 0; l < _layerCount; l++)
                for (int i = 0; i < _layerSize[l]; i++)
                {
                    double biasDelta = trainingRate * _delta[l][i];
                    Bias[l][i] -= biasDelta + momentum * _previousBiasDelta[l][i];

                    _previousBiasDelta[l][i] = biasDelta;
                }
            return error;
        }
        /// <summary>
        /// Saves the network as xml file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void SaveNetworkXml(string filePath)
        {
            if (filePath == null)
                return;
            XmlWriter writer = XmlWriter.Create(filePath);
            // Begin document
            writer.WriteStartElement("NeuralNetwork");
            writer.WriteAttributeString("Type", "BackPropagation");
            // Parameters element
            writer.WriteStartElement("Parameters");
            writer.WriteElementString("Name", Name);
            writer.WriteElementString("inputSize", _inputSize.ToString(CultureInfo.InvariantCulture));
            writer.WriteElementString("layerCount", _layerCount.ToString(CultureInfo.InvariantCulture));
            // Layer sizes
            writer.WriteStartElement("Layers");
            for (int l = 0; l < _layerCount; l++)
            {
                writer.WriteStartElement("Layer");
                writer.WriteAttributeString("Index", l.ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Size", _layerSize[l].ToString(CultureInfo.InvariantCulture));
                writer.WriteAttributeString("Type", TransferFunction[l].ToString());
                writer.WriteEndElement();// Layer
            }
            writer.WriteEndElement();//Layers
            writer.WriteEndElement();//Parameters
            // Weights and biases
            writer.WriteStartElement("Weights");
            for (int l = 0; l < _layerCount; l++)
            {
                writer.WriteStartElement("Layer");
                writer.WriteAttributeString("Index", l.ToString(CultureInfo.InvariantCulture));
                for (int j = 0; j < _layerSize[l]; j++)
                {
                    writer.WriteStartElement("Node");
                    writer.WriteAttributeString("Index", j.ToString(CultureInfo.InvariantCulture));
                    writer.WriteAttributeString("Bias", Bias[l][j].ToString(CultureInfo.InvariantCulture));
                    for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                    {
                        writer.WriteStartElement("Axion");
                        writer.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
                        writer.WriteString(Weight[l][i][j].ToString(CultureInfo.InvariantCulture));
                        writer.WriteEndElement();// Axion
                    }
                    writer.WriteEndElement();// Node
                }
                writer.WriteEndElement();// Layer
            }
            writer.WriteEndElement();// Weights
            writer.WriteEndElement();//NeuralNetwork
            writer.Flush();
            writer.Close();
        }
        /// <summary>
        /// Loads the network from xml file.
        /// </summary>
        /// <param name="filePath">The file path.</param>
        public void LoadNetworkXml(string filePath)
        {
            if (filePath == null)
                return;
            _document = new XmlDocument();
            _document.Load(filePath);
            // Load from xml
            string basePath = "NeuralNetwork/Parameters/";
            if (XPathValue("NeuralNetwork/@Type") != "BackPropagation")
                return;
            Name = XPathValue(basePath + "Name");
            int.TryParse(XPathValue(basePath + "inputSize"), out _inputSize);
            int.TryParse(XPathValue(basePath + "layerCount"), out _layerCount);
            _layerSize = new int[_layerCount];
            TransferFunction = new TransferFunction[_layerCount];
            basePath = "NeuralNetwork/Parameters/Layers/Layer";

            for (int l = 0; l < _layerCount; l++)
            {
                int.TryParse(XPathValue(basePath + "[@Index='" + l.ToString(CultureInfo.InvariantCulture) + "']/@Size"), out _layerSize[l]);
                Enum.TryParse(XPathValue(basePath + "[@Index='" + l.ToString(CultureInfo.InvariantCulture) + "']/@Type"), out TransferFunction[l]);
            }
            // Parse the weights element, start dimensioning arrays.
            Bias = new double[_layerCount][];
            _previousBiasDelta = new double[_layerCount][];
            _delta = new double[_layerCount][];
            _layerOutput = new double[_layerCount][];
            _layerInput = new double[_layerCount][];
            Weight = new double[_layerCount][][];
            _previousWeightDelta = new double[_layerCount][][];
            // Fill in 2-dimensional arrays.
            for (int l = 0; l < _layerCount; l++)
            {
                Bias[l] = new double[_layerSize[l]];
                _previousBiasDelta[l] = new double[_layerSize[l]];
                _delta[l] = new double[_layerSize[l]];
                _layerOutput[l] = new double[_layerSize[l]];
                _layerInput[l] = new double[_layerSize[l]];
                Weight[l] = new double[l == 0 ? _inputSize : _layerSize[l - 1]][];
                _previousWeightDelta[l] = new double[l == 0 ? _inputSize : _layerSize[l - 1]][];

                for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                {
                    Weight[l][i] = new double[_layerSize[l]];
                    _previousWeightDelta[l][i] = new double[_layerSize[l]];
                }
            }
            // Intiailze the weights.
            for (int l = 0; l < _layerCount; l++)
            {
                basePath = "NeuralNetwork/Weights/Layer[@Index='" + l.ToString(CultureInfo.InvariantCulture) + "']/";
                string nodePath;
                double value;
                for (int j = 0; j < _layerSize[l]; j++)
                {
                    nodePath = "Node[@Index='" + j.ToString(CultureInfo.InvariantCulture) + "']/@Bias";
                    double.TryParse(XPathValue(basePath + nodePath), out value);
                    Bias[l][j] = value;
                    _previousBiasDelta[l][j] = 0.0;
                    _layerOutput[l][j] = 0.0;
                    _layerInput[l][j] = 0.0;
                    _delta[l][j] = 0.0;
                }
                for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                {
                    for (int j = 0; j < _layerSize[l]; j++)
                    {
                        nodePath = "Node[@Index='" + j.ToString(CultureInfo.InvariantCulture) + "']/Axion[@Index='" + i.ToString(CultureInfo.InvariantCulture) + "']";
                        double.TryParse(XPathValue(basePath + nodePath), out value);

                        Weight[l][i][j] = value;
                        _previousWeightDelta[l][i][j] = 0.0;
                    }
                }
            }
            // Release
            _document = null;
        }
        /// <summary>
        /// Returns the Xpath value.
        /// </summary>
        /// <param name="xPath">The X path.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Cannot find specified node</exception>
        private string XPathValue(string xPath)
        {
            var node = _document.SelectSingleNode((xPath));
            if (node == null)
                throw new ArgumentException("Cannot find specified node.", xPath);
            return node.InnerText;
        }

        #region Really needed?
        /// <summary>
        /// Computes against the network with the specified input. (This is the same as RunEvaluation)
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Input data is not of the correct dimension.</exception>
        public double[] Compute(double[] input)
        {
            // Make sure enough data.
            if (input.Length != _inputSize)
                throw new ArgumentException("Input data is not of the correct dimension.");
            // Dimension.
            var output = new double[_layerSize[_layerCount - 1]];
            // Run the network.
            for (int l = 0; l < _layerCount; l++)
            {
                for (int j = 0; j < _layerSize[l]; j++)
                {
                    double sum = 0.0;
                    for (int i = 0; i < (l == 0 ? _inputSize : _layerSize[l - 1]); i++)
                        sum += Weight[l][i][j] * (l == 0 ? input[i] : _layerOutput[l - 1][i]);

                    sum += Bias[l][j];
                    _layerInput[l][j] = sum;
                    _layerOutput[l][j] = TransferFunctions.Evaluate(TransferFunction[l], sum);
                }
            }
            // Copy the output to the output array.
            for (int i = 0; i < _layerSize[_layerCount - 1]; i++)
                output[i] = _layerOutput[_layerCount - 1][i];
            return output;

        }
        #endregion
    }
}
