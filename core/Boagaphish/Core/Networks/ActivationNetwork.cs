//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Boagaphish.ActivationFunctions;
using Boagaphish.Core.Layers;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Networks
{
    /// <summary>
    /// The activation network is a base for multi-layer neural network with activation functions. It consists of <see cref="ActivationLayer"> activation layers</see>.
    /// </summary>
    public class ActivationNetwork : Network
    {
        /// <summary>
        /// The accessor for the network's layers.
        /// </summary>
        public new ActivationLayer this[int index]
        {
            get { return ((ActivationLayer)Layers[index]); }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationNetwork"/> class. The new network will be randomized (see <see cref="ActivationNeuron.Randomize"/> method) after it is created.
        /// </summary>
        /// <param name="function">Activation function of neurons of the network</param>
        /// <param name="inputsCount">Network's inputs count</param>
        /// <param name="neuronsCount">Array, which specifies the amount of neurons in each layer of the neural network</param>
        /// <example>The following sample illustrates the usage of <c>ActivationNetwork</c> class:
        /// <code>
        /// Create a new activation network:
        /// ActivationNetwork network = new ActivationNetwork(new SigmoidFunction( ), // sigmoid activation function
        /// 3,                      // 3 inputs
        /// 4, 1 );                 // 2 layers: 4 neurons in the first layer, 1 neuron in the second layer.
        /// </code>
        /// </example>
        public ActivationNetwork(IActivationFunction function, int inputsCount, params int[] neuronsCount)
            : base(inputsCount, neuronsCount.Length)
        {
            // Create each layer.
            for (var i = 0; i < LayersCount; i++)
            {
                Layers[i] = new ActivationLayer(
                    // The neurons count in the layer.
                    neuronsCount[i],
                    // The inputs count of the layer.
                    (i == 0) ? inputsCount : neuronsCount[i - 1],
                    // The activation function of the layer.
                    function);
            }
        }
    }
}
