//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Format;
using Boagaphish.Numeric;

namespace Boagaphish.Core.Neurons
{
    /// <summary>
    /// The base neuron class. Encapsulates common properties suchas the neuron's input, output and weights.
    /// </summary>
    public abstract class Neuron
    {
        /// <summary>
        /// Neuron's inputs count
        /// </summary>
        protected int CountInputs = 0;
        /// <summary>
        /// Nouron's wieghts
        /// </summary>
        protected double[] Weights = null;
        /// <summary>
        /// Neuron's output value
        /// </summary>
        protected double OutputValue = 0;
        /// <summary>
        /// A random number generator using lock to generate more randomness. The generator is used for neuron's weights randomization.
        /// </summary>
        protected static StaticRandom StaticRand = new StaticRandom((int)DateTime.Now.Ticks);
        /// <summary>
        /// Random generator range
        /// </summary>
        /// <remarks>Sets the range of random generator. Affects initial values of neuron's weight.
        /// Default value is [0, 1].</remarks>
        protected static DoubleRange RandRange = new DoubleRange(0.0, 1.0);
        /// <summary>
        /// Random number generator. The property allows to initialize random generator with a custom seed. The generator is used for neuron's weights randomization.
        /// </summary>
        public static StaticRandom StaticRandGenerator
        {
            get { return StaticRand; }
            set
            {
                if (value != null)
                {
                    StaticRand = value;
                }
            }
        }
        /// <summary>
        /// The range of the random generator.
        /// </summary>
        public static DoubleRange RandomRange
        {
            get { return RandRange; }
            set
            {
                if (value != null)
                {
                    RandRange = value;
                }
            }
        }
        /// <summary>
        /// Neuron's inputs count
        /// </summary>
        /// <value>The inputs count.</value>
        public int InputsCount
        {
            get { return CountInputs; }
        }
        /// <summary>
        /// Neuron's output value
        /// </summary>
        /// <value>The output.</value>
        /// <remarks>The calculation way of neuron's output value is determined by its inherited class.</remarks>
        public double Output
        {
            get { return OutputValue; }
        }
        /// <summary>
        /// Neuron's weights accessor
        /// </summary>
        /// <value></value>
        /// <remarks>Allows to access neuron's weights.</remarks>
        public double this[int index]
        {
            get { return Weights[index]; }
            set { Weights[index] = value; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Neuron"/> class
        /// </summary>
        /// <param name="inputs">Neuron's inputs count</param>
        /// <remarks>The new neuron will be randomized (see <see cref="Randomize"/> method)
        /// after it is created.</remarks>
        protected Neuron(int inputs)
        {
            // Allocate weights.
            CountInputs = Math.Max(1, inputs);
            Weights = new double[CountInputs];
            // Randomize the neuron's weights.
            Randomize();
        }
        /// <summary>
        /// Randomize the neuron. Initialize neuron's weights with random values within the range specified by <see cref="RandomRange"/>.
        /// </summary>
        /// <remarks></remarks>
        public virtual void Randomize()
        {
            double d = RandRange.Length;
            // Randomize the weights.
            for (int i = 0; i < CountInputs; i++)
                Weights[i] = StaticRand.NextDouble() * d + RandRange.Min;
        }
        /// <summary>
        /// Computes the output value of a neuron. The actual neuron's output value is determined by inherited class. The output value is also stored in <see cref="Output"/> property.
        /// </summary>
        /// <param name="input">Input vector.</param>
        /// <returns>Returns neuron's output value.</returns>
        public abstract double Compute(double[] input);
    }
}
