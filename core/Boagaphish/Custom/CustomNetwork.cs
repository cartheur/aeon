//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.Custom
{
    /// <summary>
    /// My own specific implementation of a back-propagation network.
    /// </summary>
    public class CustomNetwork : BackPropagationNetwork
    {
        private readonly double[] _inputs;
        // Parts for the input-to-hidden.
        private double _ihWeight00;
        private double _ihWeight01;
        private double _ihWeight10;
        private double _ihWeight11;
        private double _ihBias0;
        private double _ihBias1;
        private double _ihSum0;
        private double _ihSum1;
        private double _ihResult0;
        private double _ihResult1;
        // Parts for the hidden-to-output.
        private double _hoWeight00;
        private double _hoWeight01;
        private double _hoWeight10;
        private double _hoWeight11;
        private double _hoBias0;
        private double _hoBias1;
        private double _hoSum0;
        private double _hoSum1;
        private double _hoResult0;
        private double _hoResult1;
        /// <summary>
        /// The output of the network.
        /// </summary>
        public double[] NetworkOutput;
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomNetwork"/> class.
        /// </summary>
        public CustomNetwork()
        {
            _inputs = new double[2];
            NetworkOutput = new double[2];
        }
        /// <summary>
        /// Sets the inputs.
        /// </summary>
        /// <param name="inputs">The inputs.</param>
        public void SetInputs(double[] inputs)
        {
            inputs.CopyTo(_inputs, 0);
        }
        /// <summary>
        /// Sets the weights. Not good as this should be computed for not simply assigned from the application.
        /// </summary>
        /// <param name="weightsAndBiases">The weights and biases.</param>
        public void SetWeights(double[] weightsAndBiases)
        {
            int k = 0;
            _ihWeight00 = weightsAndBiases[k++];
            _ihWeight01 = weightsAndBiases[k++];
            _ihWeight10 = weightsAndBiases[k++];
            _ihWeight11 = weightsAndBiases[k++];
            _ihBias0 = weightsAndBiases[k++];
            _ihBias1 = weightsAndBiases[k++];

            _hoWeight00 = weightsAndBiases[k++];
            _hoWeight01 = weightsAndBiases[k++];
            _hoWeight10 = weightsAndBiases[k++];
            _hoWeight11 = weightsAndBiases[k++];
            _hoBias0 = weightsAndBiases[k++];
            _hoBias1 = weightsAndBiases[k];
        }
        /// <summary>
        /// Computes the outputs.
        /// </summary>
        /// <param name="activationType">Type of the activation.</param>
        public void ComputeOutputs(string activationType)
        {
            // assumes this.inputs[] have values
            _ihSum0 = (_inputs[0] * _ihWeight00) + (_inputs[1] * _ihWeight10);
            _ihSum0 += _ihBias0;
            _ihSum1 = (_inputs[0] * _ihWeight01) + (_inputs[1] * _ihWeight11);
            _ihSum1 += _ihBias1;
            _ihResult0 = Evaluation(_ihSum0, activationType, "ih");
            _ihResult1 = Evaluation(_ihSum1, activationType, "ih");

            //Console.WriteLine("ihSum0 = " + ihSum0);
            //Console.WriteLine("ihResult0 = " + ihResult0);
            //Console.WriteLine("ihSum1 = " + ihSum1);
            //Console.WriteLine("ihResult1 = " + ihResult1);

            _hoSum0 = (_ihResult0 * _hoWeight00) + (_ihResult1 * _hoWeight10);
            _hoSum0 += _hoBias0;
            _hoSum1 = (_ihResult0 * _hoWeight01) + (_ihResult1 * _hoWeight11);
            _hoSum1 += _hoBias1;
            //
            //
            _hoResult0 = Evaluation(_hoSum0, activationType, "ho");
            _hoResult1 = Evaluation(_hoSum1, activationType, "ho");

            //Console.WriteLine("hoSum0 = " + hoSum0);
            //Console.WriteLine("hoResult0 = " + hoResult0);
            //Console.WriteLine("hoSum1 = " + hoSum1);
            //Console.WriteLine("hoResult1 = " + hoResult1);

            NetworkOutput[0] = _hoResult0;
            NetworkOutput[1] = _hoResult1;
        }
        /// <summary>
        /// Computes softmax the specified value and layer.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        public double ComputeSoftmax(double x, string layer)
        {
            // Determine the maximum value.
            double max = double.MinValue;
            if (layer == "ih")
                max = (_ihSum0 > _ihSum1) ? _ihSum0 : _ihSum1;
            else if (layer == "ho")
                max = (_hoSum0 > _hoSum1) ? _hoSum0 : _hoSum1;
            // Compute the scale.
            double scale = 0.0;
            if (layer == "ih")
                scale = Math.Exp(_ihSum0 - max) + Math.Exp(_ihSum1 - max);
            else if (layer == "ho")
                scale = Math.Exp(_hoSum0 - max) + Math.Exp(_hoSum1 - max);

            return Math.Exp(x - max) / scale;
        }
        /// <summary>
        /// An evaluation of a value, an activation type, and a layer.
        /// </summary>
        /// <param name="x">The x.</param>
        /// <param name="activationType">Type of the activation.</param>
        /// <param name="layer">The layer.</param>
        /// <returns></returns>
        /// <exception cref="System.Exception">Not implemented</exception>
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
        private static double LogSigmoid(double x)
        {
            if (x < -45.0) return 0.0;
            if (x > 45.0) return 1.0;
            return 1.0 / (1.0 + Math.Exp(-x));
        }
        private static double HyperbolicTangtent(double x)
        {
            if (x < -45.0) return -1.0;
            if (x > 45.0) return 1.0;
            return Math.Tanh(x);
        }
        private double NormalizedExponential(double x, string layer)
        {
            // naive version 
            double scale = 0.0;
            if (layer == "ih")
                scale = Math.Exp(_ihSum0) + Math.Exp(_ihSum1);
            else if (layer == "ho")
                scale = Math.Exp(_hoSum0) + Math.Exp(_hoSum1);
            else
                throw new Exception("Unknown layer");

            return Math.Exp(x) / scale;
        }

    }
}
