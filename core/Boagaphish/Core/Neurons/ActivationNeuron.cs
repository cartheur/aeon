//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.ActivationFunctions;

namespace Boagaphish.Core.Neurons
{
    /// <summary>
    /// The activation neuron. The activation neuron computes weighted sum of its inputs, adds threshold value and then applies activation function. The neuron is usually used in multi-layer neural networks.
    /// </summary>
    public class ActivationNeuron : Neuron
    {
        /// <summary>
        /// The threshold value. The value is added to inputs weighted sum.
        /// </summary>
        protected double ThresholdValue = 0.0f;
        /// <summary>
        /// The activation function. The function is applied to inputs weighted sum plus the threshold value.
        /// </summary>
        protected IActivationFunction Function = null;
        /// <summary>
        /// The threshold value. The value is added to inputs weighted sum.
        /// </summary>
        public double Threshold
        {
            get { return ThresholdValue; }
            set { ThresholdValue = value; }
        }
        /// <summary>
        /// The activation function of the neuron.
        /// </summary>
        public IActivationFunction ActivationFunction
        {
            get { return Function; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationNeuron"/> class
        /// </summary>
        /// <param name="inputs">The inputs count of the neuron.</param>
        /// <param name="function">The activation function of the neuron.</param>
        public ActivationNeuron(int inputs, IActivationFunction function)
            : base(inputs)
        {
            Function = function;
        }
        /// <summary>
        /// Randomize the neuron. Calls base class <see cref="Neuron.Randomize">Randomize</see> method to randomize neuron's weights and then randomize threshold's value.
        /// </summary>
        public override void Randomize()
        {
            // Randomize the weights.
            base.Randomize();
            // Randomize the threshold.
            ThresholdValue = StaticRand.NextDouble() * (RandRange.Length) + RandRange.Min;
        }
        /// <summary>
        /// Computes the output value of neuron. The output value of activation neuron is equal to value of nueron's activation function, which parameter is weighted sum of its inputs plus threshold value. The output value is also stored in <see cref="Neuron.Output">Output</see> property.
        /// </summary>
        /// <param name="input">The input vector.</param>
        /// <returns>Returns the neuron's output value.</returns>
        public override double Compute(double[] input)
        {
            // Check for corrent input vector.
            if (input.Length != CountInputs)
                throw new ArgumentException();
            // The initial summation value.
            double sum = 0.0;
            // Compute the weighted sum of inputs.
            for (int i = 0; i < CountInputs; i++)
            {
                sum += Weights[i] * input[i];
            }
            sum += ThresholdValue;

            return (OutputValue = Function.Function(sum));
        }
    }
}
