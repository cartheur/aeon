//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.Core.Neurons
{
    /// <summary>
    /// The distance neuron. Distance neuron computes its output as distance between its weights and inputs. The neuron is usually used in Kohonen Self Organizing Map.
    /// </summary>
    public class DistanceNeuron : Neuron
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNeuron"/> class. The new neuron will be randomized (see <see cref="Neuron.Randomize"/> method) after it is created.
        /// </summary>
        /// <param name="inputs">Neuron's inputs count</param>
        public DistanceNeuron(int inputs) : base(inputs) { }
        /// <summary>
        /// Computes the output value of neuron. The output value of distance neuron is equal to distance between its weights and inputs - sum of absolute differences. The output value is also stored in <see cref="Neuron.Output">Output</see> property. The actual neuron's output value is determined by inherited class. The output value is also stored in <see cref="Neuron.Output"/> property.
        /// </summary>
        /// <param name="input">The input vector.</param>
        public override double Compute(double[] input)
        {
            OutputValue = 0.0;

            // compute distance between inputs and weights
            for (int i = 0; i < CountInputs; i++)
            {
                OutputValue += Math.Abs(Weights[i] - input[i]);
            }
            return OutputValue;
        }
    }
}
