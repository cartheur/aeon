//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Layers
{
    /// <summary>
    /// This is a base neural layer class, which represents a collection of neurons.
    /// </summary>
    public abstract class Layer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Layer"/> class
        /// </summary>
        /// <param name="neuronsCount">Layer's neurons count</param>
        /// <param name="inputsCount">Layer's inputs count</param>
        /// <remarks>Protected contructor, which initializes <see cref="CountInputs"/>,
        /// <see cref="CountNeurons"/>, <see cref="Neurons"/> and <see cref="OutputVector"/>
        /// members.</remarks>
        protected Layer(int neuronsCount, int inputsCount)
        {
            CountInputs = Math.Max(1, inputsCount);
            CountNeurons = Math.Max(1, neuronsCount);
            // create collection of neurons
            Neurons = new Neuron[CountNeurons];
            // allocate output array
            OutputVector = new double[CountNeurons];
        }
        /// <summary>
        /// The layer's inputs count.
        /// </summary>
        protected int CountInputs = 0;
        /// <summary>
        /// Layer's neurons count
        /// </summary>
        protected int CountNeurons = 0;
        /// <summary>
        /// Layer's neurons
        /// </summary>
        protected Neuron[] Neurons;
        /// <summary>
        /// The layer's output vector.
        /// </summary>
        protected double[] OutputVector;
        /// <summary>
        /// Layer's inputs count
        /// </summary>
        /// <value>The inputs count.</value>
        public int InputsCount
        {
            get { return CountInputs; }
        }
        /// <summary>
        /// The layer's neurons count.
        /// </summary>
        public int NeuronsCount
        {
            get { return CountNeurons; }
        }
        /// <summary>
        /// The layer's output vector. The calculation way of layer's output vector is determined by an inherited class.
        /// </summary>
        public double[] Output
        {
            get { return OutputVector; }
        }
        /// <summary>
        /// The layer's neurons accessor. Allows to access layer's neurons.
        /// </summary>
        public Neuron this[int index]
        {
            get { return Neurons[index]; }
        }
        /// <summary>
        /// Computes the output vector of the layer. The actual layer's output vector is determined by inherited class and it consists of output values of layer's neurons. The output vector is also stored in <see cref="Output"/> property.
        /// </summary>
        /// <param name="input">Input vector</param>
        /// <returns>Returns layer's output vector</returns>
        public virtual double[] Compute(double[] input)
        {
            // Compute each neuron.
            for (var i = 0; i < CountNeurons; i++)
                OutputVector[i] = Neurons[i].Compute(input);

            return OutputVector;
        }
        /// <summary>
        /// Randomize the neurons of the layer. Randomizes layer's neurons by calling <see cref="Neuron.Randomize"/> method of each neuron.
        /// </summary>
        public virtual void Randomize()
        {
            foreach (var neuron in Neurons)
                neuron.Randomize();
        }
    }
}
