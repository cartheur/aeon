//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Boagaphish.Core.Layers;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Networks
{
    /// <summary>
    /// Distance network is a neural network of only one <see cref="DistanceLayer">distance layer</see>. The network is a base for such neural networks as SOM, Elastic net, etc.
    /// </summary>
    public class DistanceNetwork : Network
    {
        /// <summary>
        /// The accessor to the distance layers.
        /// </summary>
        public new DistanceLayer this[int index]
        {
            get { return ((DistanceLayer)Layers[index]); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceNetwork"/> class. The new network will be randomized (see <see cref="Neuron.Randomize"/>
        /// method) after it is created.
        /// </summary>
        /// <param name="inputsCount">The inputs count of the network.</param>
        /// <param name="neuronsCount">The neurons count of the network.</param>
        public DistanceNetwork(int inputsCount, int neuronsCount)
            : base(inputsCount, 1)
        {
            // Create the layer.
            Layers[0] = new DistanceLayer(neuronsCount, inputsCount);
        }
        /// <summary>
        /// Get the winner neuron. The method returns index of the neuron, which weights have the minimum distance from network's input.
        /// </summary>
        /// <returns>Index of the winner neuron</returns>
        public int GetWinner()
        {
            // Find the MIN value.
            double min = OutputVector[0];
            int minIndex = 0;

            for (int i = 1, n = OutputVector.Length; i < n; i++)
            {
                if (OutputVector[i] < min)
                {
                    // Found a new MIN value.
                    min = OutputVector[i];
                    minIndex = i;
                }
            }
            return minIndex;
        }
    }
}
