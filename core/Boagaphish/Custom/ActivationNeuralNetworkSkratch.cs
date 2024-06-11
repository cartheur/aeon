//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.Custom
{
    public class ActivationNeuralNetworkSkratch
    {
        public class DummyNeuralNetwork
        {
            public double[] inputs;

            public double ihWeight00;
            public double ihWeight01;
            public double ihWeight10;
            public double ihWeight11;
            public double ihBias0;
            public double ihBias1;

            public double ihSum0;
            public double ihSum1;
            public double ihResult0;
            public double ihResult1;

            public double hoWeight00;
            public double hoWeight01;
            public double hoWeight10;
            public double hoWeight11;
            public double hoBias0;
            public double hoBias1;

            public double hoSum0;
            public double hoSum1;
            public double hoResult0;
            public double hoResult1;

            public double[] outputs;

            public DummyNeuralNetwork()
            {
                inputs = new double[2];
                outputs = new double[2];
            }

            public void SetInputs(double[] inputs)
            {
                inputs.CopyTo(this.inputs, 0);
            }

            public void SetWeights(double[] weightsAndBiases)
            {
                int k = 0;
                ihWeight00 = weightsAndBiases[k++];
                ihWeight01 = weightsAndBiases[k++];
                ihWeight10 = weightsAndBiases[k++];
                ihWeight11 = weightsAndBiases[k++];
                ihBias0 = weightsAndBiases[k++];
                ihBias1 = weightsAndBiases[k++];

                hoWeight00 = weightsAndBiases[k++];
                hoWeight01 = weightsAndBiases[k++];
                hoWeight10 = weightsAndBiases[k++];
                hoWeight11 = weightsAndBiases[k++];
                hoBias0 = weightsAndBiases[k++];
                hoBias1 = weightsAndBiases[k++];
            }

            public void ComputeOutputs(string activationType)
            {
                // assumes this.inputs[] have values
                ihSum0 = (inputs[0] * ihWeight00) + (inputs[1] * ihWeight10);
                ihSum0 += ihBias0;
                ihSum1 = (inputs[0] * ihWeight01) + (inputs[1] * ihWeight11);
                ihSum1 += ihBias1;
                ihResult0 = Evaluation(ihSum0, activationType, "ih");
                ihResult1 = Evaluation(ihSum1, activationType, "ih");

                //Console.WriteLine("ihSum0 = " + ihSum0);
                //Console.WriteLine("ihResult0 = " + ihResult0);
                //Console.WriteLine("ihSum1 = " + ihSum1);
                //Console.WriteLine("ihResult1 = " + ihResult1);

                hoSum0 = (ihResult0 * hoWeight00) + (ihResult1 * hoWeight10);
                hoSum0 += hoBias0;
                hoSum1 = (ihResult0 * hoWeight01) + (ihResult1 * hoWeight11);
                hoSum1 += hoBias1;
                hoResult0 = Evaluation(hoSum0, activationType, "ho");
                hoResult1 = Evaluation(hoSum1, activationType, "ho");

                //Console.WriteLine("hoSum0 = " + hoSum0);
                //Console.WriteLine("hoResult0 = " + hoResult0);
                //Console.WriteLine("hoSum1 = " + hoSum1);
                //Console.WriteLine("hoResult1 = " + hoResult1);

                outputs[0] = hoResult0;
                outputs[1] = hoResult1;
            }

            public double Evaluation(double x, string activationType, string layer)
            {
                activationType = activationType.ToLower().Trim();
                if (activationType == "logsigmoid")
                    return LogSigmoid(x);
                if (activationType == "hyperbolictangent")
                    return HyperbolicTangtent(x);
                if (activationType == "softmax")
                    return NormalizedExponential(x, layer);
                throw new Exception("Not implemented");
            }

            public double LogSigmoid(double x)
            {
                if (x < -45.0) return 0.0;
                if (x > 45.0) return 1.0;
                return 1.0 / (1.0 + Math.Exp(-x));
            }

            public double HyperbolicTangtent(double x)
            {
                if (x < -45.0) return -1.0;
                if (x > 45.0) return 1.0;
                return Math.Tanh(x);
            }

            public double NormalizedExponential(double x, string layer)
            {
                // naive version 
                double scale = 0.0;
                if (layer == "ih")
                    scale = Math.Exp(ihSum0) + Math.Exp(ihSum1);
                else if (layer == "ho")
                    scale = Math.Exp(hoSum0) + Math.Exp(hoSum1);
                else
                    throw new Exception("Unknown layer");

                return Math.Exp(x) / scale;
            }

            public double Softmax(double x, string layer)
            {
                // Determine the maximum value.
                double max = double.MinValue;
                if (layer == "ih")
                    max = (ihSum0 > ihSum1) ? ihSum0 : ihSum1;
                else if (layer == "ho")
                    max = (hoSum0 > hoSum1) ? hoSum0 : hoSum1;
                // Compute the scale.
                double scale = 0.0;
                if (layer == "ih")
                    scale = Math.Exp(ihSum0 - max) + Math.Exp(ihSum1 - max);
                else if (layer == "ho")
                    scale = Math.Exp(hoSum0 - max) + Math.Exp(hoSum1 - max);

                return Math.Exp(x - max) / scale;
            }

        }

    }
}
