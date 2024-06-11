//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using Boagaphish.Core.Layers;

namespace Boagaphish.Core.Networks
{
    /// <summary>
    /// This is a base neural netwok class, which represents a collection of neuron's layers.
    /// </summary>
    public abstract class Network
    {
        /// <summary>
        /// Sets the data pointer to between one layer and the other.
        /// </summary>
        public enum CrossLayer
        {
            InputHidden, HiddenOutput
        }
        /// <summary>
        /// The network's inputs count.
        /// </summary>
        protected int CountInputs;
        /// <summary>
        /// The network's layers count.
        /// </summary>
        protected int CountLayers;
        /// <summary>
        /// The layers of the network.
        /// </summary>
        protected Layer[] Layers;
        /// <summary>
        /// The network's output vector.
        /// </summary>
        protected double[] OutputVector;
        /// <summary>
        /// The inputs count for the network.
        /// </summary>
        public int InputsCount
        {
            get { return CountInputs; }
        }
        /// <summary>
        /// The layers count for the network.
        /// </summary>
        public int LayersCount
        {
            get { return CountLayers; }
        }
        /// <summary>
        /// The output vector of the network. The calculation way of network's output vector is determined by an inherited class.
        /// </summary>
        public double[] Output
        {
            get { return OutputVector; }
        }
        /// <summary>
        /// The accessor for the network's layers.
        /// </summary>
        public Layer this[int index]
        {
            get { return Layers[index]; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Network"/> class. Protected constructor, which initializes <see cref="inputsCount"/>, <see cref="layersCount"/> and <see cref="Layers"/> members.
        /// </summary>
        /// <param name="inputsCount">The network's inputs count.</param>
        /// <param name="layersCount">The network's layers count.</param>
        protected Network(int inputsCount, int layersCount)
        {
            CountInputs = Math.Max(1, inputsCount);
            CountLayers = Math.Max(1, layersCount);
            // Create a collection of layers.
            Layers = new Layer[CountLayers];
        }
        /// <summary>
        /// Compute the output vector of the network. The actual network's output vecor is determined by inherited class and it represents an output vector of the last layer of the network. The output vector is also stored in <see cref="Output"/> property.
        /// </summary>
        /// <param name="input">Input vector.</param>
        /// <returns>Returns network's output vector.</returns>
        public virtual double[] Compute(double[] input)
        {
            OutputVector = input;
            // Compute each layer.
            foreach (var layer in Layers)
            {
                OutputVector = layer.Compute(OutputVector);
            }

            return OutputVector;
        }
        /// <summary>
        /// Randomize layers of the network. Randomizes network's layers by calling <see cref="Layer.Randomize"/> method for each layer.
        /// </summary>
        public virtual void Randomize()
        {
            foreach (var layer in Layers)
            {
                layer.Randomize();
            }
        }
    }
}
