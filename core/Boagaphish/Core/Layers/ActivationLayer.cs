//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using Boagaphish.ActivationFunctions;
using Boagaphish.Core.Neurons;

namespace Boagaphish.Core.Layers
{
    /// <summary>
    /// The activation layer. Activation layer is a layer of <see cref="ActivationNeuron">activation neurons</see>. The layer is usually used in multi-layer neural networks.
    /// </summary>
    /// <remarks></remarks>
    public class ActivationLayer : Layer
    {
        /// <summary>
        /// The layer's neurons accessor. Allows to access layer's neurons.
        /// </summary>
        public new ActivationNeuron this[int index]
        {
            get { return (ActivationNeuron)Neurons[index]; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="ActivationLayer"/> class. The new layer will be randomized (see <see ref="ActivationNeuron.Randomize"/> method) after it is created.
        /// </summary>
        /// <param name="neuronsCount">The neurons count of the layer.</param>
        /// <param name="inputsCount">The inputs count of the layer.</param>
        /// <param name="function">The activation function of neurons of the layer.</param>
        public ActivationLayer(int neuronsCount, int inputsCount, IActivationFunction function)
            : base(neuronsCount, inputsCount)
        {
            // Create each neuron.
            for (int i = 0; i < neuronsCount; i++)
                Neurons[i] = new ActivationNeuron(inputsCount, function);
        }
    }
}
