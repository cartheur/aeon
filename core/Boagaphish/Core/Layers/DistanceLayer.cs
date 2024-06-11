//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Layers
{
    /// <summary>
    /// The distance layer. Distance layer is a layer of <see cref="DistanceNeuron">distance neurons</see>. The layer is usually a single layer of such networks as Kohonen Self Organizing Map, Elastic Net, Hamming Memory Net, etc.
    /// </summary>
    public class DistanceLayer : Layer
    {
        /// <summary>
        /// The neurons accessor for the layer. Allows to access layer's neurons.
        /// </summary>
        public new DistanceNeuron this[int index]
        {
            get { return (DistanceNeuron)Neurons[index]; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceLayer"/> class. The new layer will be randomized (see <see cref="Neuron.Randomize"/>
        /// method) after it is created.
        /// </summary>
        /// <param name="neuronsCount">The neurons count of the layer.</param>
        /// <param name="inputsCount">The inputs count of the layer.</param>
        public DistanceLayer(int neuronsCount, int inputsCount)
            : base(neuronsCount, inputsCount)
        {
            // Create each neuron.
            for (int i = 0; i < neuronsCount; i++)
                Neurons[i] = new DistanceNeuron(inputsCount);
        }
    }
}
