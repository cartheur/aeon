using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Boagaphish.Numeric;

namespace Boagaphish.Core.Animals
{
    public class BackPropagationNetwork
    {
        private int _layerCount;

        private int _inputSize;

        private int[] _layerSize;

        private double[][] _layerOutput;

        private double[][] _layerInput;

        private double[][] _delta;

        private double[][] _previousBiasDelta;

        private double[][][] _previousWeightDelta;

        private XmlDocument _document;

        public TransferFunction[] TransferFunction;

        public double[][] Bias;

        public double[][][] Weight;

        public double[] ZerothSum;

        public double[] FirstSum;

        public double FirstToSecondLayerResult;

        public double NextToOutputLayerResult;

        public string Name
        {
            get;
            set;
        }

        public BackPropagationNetwork(IList<int> layerSizes, IList<TransferFunction> transferFunctions, string name = "Candles")
        {
            Name = name;
            if (transferFunctions.Count != layerSizes.Count || transferFunctions[0] != 0)
            {
                throw new ArgumentException("Cannot initiate on those parameters.");
            }
            _layerCount = layerSizes.Count - 1;
            _inputSize = layerSizes[0];
            _layerSize = new int[_layerCount];
            for (int i = 0; i < _layerCount; i++)
            {
                _layerSize[i] = layerSizes[i + 1];
            }
            TransferFunction = new TransferFunction[_layerCount];
            for (int j = 0; j < _layerCount; j++)
            {
                TransferFunction[j] = transferFunctions[j + 1];
            }
            Bias = new double[_layerCount][];
            _previousBiasDelta = new double[_layerCount][];
            _delta = new double[_layerCount][];
            _layerOutput = new double[_layerCount][];
            _layerInput = new double[_layerCount][];
            Weight = new double[_layerCount][][];
            _previousWeightDelta = new double[_layerCount][][];
            for (int k = 0; k < _layerCount; k++)
            {
                Bias[k] = new double[_layerSize[k]];
                _previousBiasDelta[k] = new double[_layerSize[k]];
                _delta[k] = new double[_layerSize[k]];
                _layerOutput[k] = new double[_layerSize[k]];
                _layerInput[k] = new double[_layerSize[k]];
                Weight[k] = new double[(k == 0) ? _inputSize : _layerSize[k - 1]][];
                _previousWeightDelta[k] = new double[(k == 0) ? _inputSize : _layerSize[k - 1]][];
                for (int l = 0; l < ((k == 0) ? _inputSize : _layerSize[k - 1]); l++)
                {
                    Weight[k][l] = new double[_layerSize[k]];
                    _previousWeightDelta[k][l] = new double[_layerSize[k]];
                }
            }
            for (int m = 0; m < _layerCount; m++)
            {
                for (int n = 0; n < _layerSize[m]; n++)
                {
                    Bias[m][n] = Gaussian.RandomGaussian();
                    _previousBiasDelta[m][n] = 0.0;
                    _layerOutput[m][n] = 0.0;
                    _layerInput[m][n] = 0.0;
                    _delta[m][n] = 0.0;
                }
                for (int num = 0; num < ((m == 0) ? _inputSize : _layerSize[m - 1]); num++)
                {
                    for (int num2 = 0; num2 < _layerSize[m]; num2++)
                    {
                        Weight[m][num][num2] = Gaussian.RandomGaussian();
                        _previousWeightDelta[m][num][num2] = 0.0;
                    }
                }
            }
        }

        public BackPropagationNetwork(string filePath)
        {
            LoadNetworkXml(filePath);
        }

        protected BackPropagationNetwork()
        {
        }

        public void RunEvaluation(ref double[] input, out double[] output)
        {
            if (input.Length != _inputSize)
            {
                throw new ArgumentException("Input data is not of the correct dimension.");
            }
            output = new double[_layerSize[_layerCount - 1]];
            for (int i = 0; i < _layerCount; i++)
            {
                for (int j = 0; j < _layerSize[i]; j++)
                {
                    double num = 0.0;
                    for (int k = 0; k < ((i == 0) ? _inputSize : _layerSize[i - 1]); k++)
                    {
                        num += Weight[i][k][j] * ((i == 0) ? input[k] : _layerOutput[i - 1][k]);
                    }
                    num += Bias[i][j];
                    _layerInput[i][j] = num;
                    _layerOutput[i][j] = TransferFunctions.Evaluate(TransferFunction[i], num, 0.5);
                }
            }
            for (int l = 0; l < _layerSize[_layerCount - 1]; l++)
            {
                output[l] = _layerOutput[_layerCount - 1][l];
            }
        }

        public double ComputeSoftmax(double[] input, string layer)
        {
            double result = 0.0;
            for (int i = 0; i < _layerCount; i++)
            {
                ZerothSum[i] += input[i] * Weight[0][0][i] + input[i] * Weight[i][0][0];
                ZerothSum[i] += Bias[0][i];
                FirstSum[i] += input[i] * Weight[0][i][0] + input[i] * Weight[i][i][0];
                FirstSum[i] += Bias[i][0];
                FirstToSecondLayerResult = Softmax(ZerothSum[i], "ih");
                NextToOutputLayerResult = Softmax(FirstSum[i], "ho");
                result = 0.5 * Math.Pow(FirstToSecondLayerResult * NextToOutputLayerResult, 2.0);
            }
            return result;
        }

        protected double Softmax(double x, string layer)
        {
            double num = -1.7976931348623157E+308;
            if (layer == "ih")
            {
                num = ((ZerothSum[0] > ZerothSum[1]) ? ZerothSum[0] : ZerothSum[1]);
            }
            else if (layer == "ho")
            {
                num = ((FirstSum[0] > FirstSum[1]) ? FirstSum[0] : FirstSum[1]);
            }
            double num2 = 0.0;
            if (layer == "ih")
            {
                num2 = Math.Exp(ZerothSum[0] - num) + Math.Exp(ZerothSum[1] - num);
            }
            else if (layer == "ho")
            {
                num2 = Math.Exp(FirstSum[0] - num) + Math.Exp(FirstSum[1] - num);
            }
            return Math.Exp(x - num) / num2;
        }

        public double Train(ref double[] input, ref double[] desired, double trainingRate, double momentum)
        {
            if (input.Length != _inputSize)
            {
                throw new ArgumentException("Invalid input parameter", "input");
            }
            if (desired.Length != _layerSize[_layerCount - 1])
            {
                throw new ArgumentException("Invalid input parameter", "desired");
            }
            double num = 0.0;
            double[] array = new double[_layerSize[_layerCount - 1]];
            RunEvaluation(ref input, out array);
            for (int num2 = _layerCount - 1; num2 >= 0; num2--)
            {
                if (num2 == _layerCount - 1)
                {
                    for (int i = 0; i < _layerSize[num2]; i++)
                    {
                        _delta[num2][i] = array[i] - desired[i];
                        num += Math.Pow(_delta[num2][i], 2.0);
                        _delta[num2][i] *= TransferFunctions.EvaluateDerivative(TransferFunction[num2], _layerInput[num2][i], 0.5);
                    }
                }
                else
                {
                    for (int j = 0; j < _layerSize[num2]; j++)
                    {
                        double num3 = 0.0;
                        for (int k = 0; k < _layerSize[num2 + 1]; k++)
                        {
                            num3 += Weight[num2 + 1][j][k] * _delta[num2 + 1][k];
                        }
                        num3 *= TransferFunctions.EvaluateDerivative(TransferFunction[num2], _layerInput[num2][j], 0.5);
                        _delta[num2][j] = num3;
                    }
                }
            }
            for (int l = 0; l < _layerCount; l++)
            {
                for (int m = 0; m < ((l == 0) ? _inputSize : _layerSize[l - 1]); m++)
                {
                    for (int n = 0; n < _layerSize[l]; n++)
                    {
                        double num4 = trainingRate * _delta[l][n] * ((l == 0) ? input[m] : _layerOutput[l - 1][m]) + momentum * _previousWeightDelta[l][m][n];
                        Weight[l][m][n] -= num4;
                        _previousWeightDelta[l][m][n] = num4;
                    }
                }
            }
            for (int num5 = 0; num5 < _layerCount; num5++)
            {
                for (int num6 = 0; num6 < _layerSize[num5]; num6++)
                {
                    double num7 = trainingRate * _delta[num5][num6];
                    Bias[num5][num6] -= num7 + momentum * _previousBiasDelta[num5][num6];
                    _previousBiasDelta[num5][num6] = num7;
                }
            }
            return num;
        }

        public void SaveNetworkXml(string filePath)
        {
            if (filePath != null)
            {
                XmlWriter xmlWriter = XmlWriter.Create(filePath);
                xmlWriter.WriteStartElement("NeuralNetwork");
                xmlWriter.WriteAttributeString("Type", "BackPropagation");
                xmlWriter.WriteStartElement("Parameters");
                xmlWriter.WriteElementString("Name", Name);
                xmlWriter.WriteElementString("inputSize", _inputSize.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteElementString("layerCount", _layerCount.ToString(CultureInfo.InvariantCulture));
                xmlWriter.WriteStartElement("Layers");
                for (int i = 0; i < _layerCount; i++)
                {
                    xmlWriter.WriteStartElement("Layer");
                    xmlWriter.WriteAttributeString("Index", i.ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteAttributeString("Size", _layerSize[i].ToString(CultureInfo.InvariantCulture));
                    xmlWriter.WriteAttributeString("Type", TransferFunction[i].ToString());
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.WriteStartElement("Weights");
                for (int j = 0; j < _layerCount; j++)
                {
                    xmlWriter.WriteStartElement("Layer");
                    xmlWriter.WriteAttributeString("Index", j.ToString(CultureInfo.InvariantCulture));
                    for (int k = 0; k < _layerSize[j]; k++)
                    {
                        xmlWriter.WriteStartElement("Node");
                        xmlWriter.WriteAttributeString("Index", k.ToString(CultureInfo.InvariantCulture));
                        xmlWriter.WriteAttributeString("Bias", Bias[j][k].ToString(CultureInfo.InvariantCulture));
                        for (int l = 0; l < ((j == 0) ? _inputSize : _layerSize[j - 1]); l++)
                        {
                            xmlWriter.WriteStartElement("Axion");
                            xmlWriter.WriteAttributeString("Index", l.ToString(CultureInfo.InvariantCulture));
                            xmlWriter.WriteString(Weight[j][l][k].ToString(CultureInfo.InvariantCulture));
                            xmlWriter.WriteEndElement();
                        }
                        xmlWriter.WriteEndElement();
                    }
                    xmlWriter.WriteEndElement();
                }
                xmlWriter.WriteEndElement();
                xmlWriter.WriteEndElement();
                xmlWriter.Flush();
                xmlWriter.Close();
            }
        }

        public void LoadNetworkXml(string filePath)
        {
            if (filePath != null)
            {
                _document = new XmlDocument();
                _document.Load(filePath);
                string str = "NeuralNetwork/Parameters/";
                if (!(XPathValue("NeuralNetwork/@Type") != "BackPropagation"))
                {
                    Name = XPathValue(str + "Name");
                    int.TryParse(XPathValue(str + "inputSize"), out _inputSize);
                    int.TryParse(XPathValue(str + "layerCount"), out _layerCount);
                    _layerSize = new int[_layerCount];
                    TransferFunction = new TransferFunction[_layerCount];
                    str = "NeuralNetwork/Parameters/Layers/Layer";
                    for (int i = 0; i < _layerCount; i++)
                    {
                        int.TryParse(XPathValue(str + "[@Index='" + i.ToString(CultureInfo.InvariantCulture) + "']/@Size"), out _layerSize[i]);
                        Enum.TryParse(XPathValue(str + "[@Index='" + i.ToString(CultureInfo.InvariantCulture) + "']/@Type"), out TransferFunction[i]);
                    }
                    Bias = new double[_layerCount][];
                    _previousBiasDelta = new double[_layerCount][];
                    _delta = new double[_layerCount][];
                    _layerOutput = new double[_layerCount][];
                    _layerInput = new double[_layerCount][];
                    Weight = new double[_layerCount][][];
                    _previousWeightDelta = new double[_layerCount][][];
                    for (int j = 0; j < _layerCount; j++)
                    {
                        Bias[j] = new double[_layerSize[j]];
                        _previousBiasDelta[j] = new double[_layerSize[j]];
                        _delta[j] = new double[_layerSize[j]];
                        _layerOutput[j] = new double[_layerSize[j]];
                        _layerInput[j] = new double[_layerSize[j]];
                        Weight[j] = new double[(j == 0) ? _inputSize : _layerSize[j - 1]][];
                        _previousWeightDelta[j] = new double[(j == 0) ? _inputSize : _layerSize[j - 1]][];
                        for (int k = 0; k < ((j == 0) ? _inputSize : _layerSize[j - 1]); k++)
                        {
                            Weight[j][k] = new double[_layerSize[j]];
                            _previousWeightDelta[j][k] = new double[_layerSize[j]];
                        }
                    }
                    for (int l = 0; l < _layerCount; l++)
                    {
                        str = "NeuralNetwork/Weights/Layer[@Index='" + l.ToString(CultureInfo.InvariantCulture) + "']/";
                        double num;
                        for (int m = 0; m < _layerSize[l]; m++)
                        {
                            string str2 = "Node[@Index='" + m.ToString(CultureInfo.InvariantCulture) + "']/@Bias";
                            double.TryParse(XPathValue(str + str2), out num);
                            Bias[l][m] = num;
                            _previousBiasDelta[l][m] = 0.0;
                            _layerOutput[l][m] = 0.0;
                            _layerInput[l][m] = 0.0;
                            _delta[l][m] = 0.0;
                        }
                        for (int n = 0; n < ((l == 0) ? _inputSize : _layerSize[l - 1]); n++)
                        {
                            for (int num2 = 0; num2 < _layerSize[l]; num2++)
                            {
                                string str2 = "Node[@Index='" + num2.ToString(CultureInfo.InvariantCulture) + "']/Axion[@Index='" + n.ToString(CultureInfo.InvariantCulture) + "']";
                                double.TryParse(XPathValue(str + str2), out num);
                                Weight[l][n][num2] = num;
                                _previousWeightDelta[l][n][num2] = 0.0;
                            }
                        }
                    }
                    _document = null;
                }
            }
        }

        private string XPathValue(string xPath)
        {
            XmlNode xmlNode = _document.SelectSingleNode(xPath);
            if (xmlNode == null)
            {
                throw new ArgumentException("Cannot find specified node.", xPath);
            }
            return xmlNode.InnerText;
        }

        public double[] Compute(double[] input)
        {
            if (input.Length != _inputSize)
            {
                throw new ArgumentException("Input data is not of the correct dimension.");
            }
            double[] array = new double[_layerSize[_layerCount - 1]];
            for (int i = 0; i < _layerCount; i++)
            {
                for (int j = 0; j < _layerSize[i]; j++)
                {
                    double num = 0.0;
                    for (int k = 0; k < ((i == 0) ? _inputSize : _layerSize[i - 1]); k++)
                    {
                        num += Weight[i][k][j] * ((i == 0) ? input[k] : _layerOutput[i - 1][k]);
                    }
                    num += Bias[i][j];
                    _layerInput[i][j] = num;
                    _layerOutput[i][j] = TransferFunctions.Evaluate(TransferFunction[i], num, 0.5);
                }
            }
            for (int l = 0; l < _layerSize[_layerCount - 1]; l++)
            {
                array[l] = _layerOutput[_layerCount - 1][l];
            }
            return array;
        }
    }
}
