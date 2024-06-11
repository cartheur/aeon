//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Core.Learning
{
    /// <summary>
    /// The supervised learning interface. The interface describes methods, which should be implemented by all supervised learning algorithms. Supervised learning is such a type of learning algorithms, where system's desired output is known at the learning stage. Given a sample of input values and desired outputs, the system should adopt its internal variables to produce a correct (or close to correct) result after the learning step is completed.
    /// </summary>
    public interface ISupervised
    {
        /// <summary>
        /// Runs a learning iteration.
        /// </summary>
        /// <param name="input">The input vector.</param>
        /// <param name="desired">The desired output vector.</param>
        /// <returns>
        /// Returns the learning error.
        /// </returns>
        double Run(double[] input, double[] desired);
        /// <summary>
        /// Runs a learning epoch.
        /// </summary>
        /// <param name="input">An array of input vectors.</param>
        /// <param name="desired">Anarray of output vectors.</param>
        /// <returns>
        /// Returns a sum of learning errors.
        /// </returns>
        double RunEpoch(double[][] input, double[][] desired);
    }
}
