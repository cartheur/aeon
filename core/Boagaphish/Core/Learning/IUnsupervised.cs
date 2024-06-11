//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Core.Learning
{
    /// <summary>
    /// The unsupervised learning interface.
    /// </summary>
    /// <remarks>The interface describes methods, which should be implemented by all unsupervised learning algorithms. Unsupervised learning is such a type of learning algorithms, where system's desired output is not known on the learning stage. Given sample input values, it is expected, that system will organize itself in the way to find similarities betweed provided samples.</remarks>
	public interface IUnsupervised
	{
        /// <summary>
        /// Runs learning iteration
        /// </summary>
        /// <param name="input">input vector</param>
        /// <returns>Returns learning error</returns>
		double Run( double[] input );
        /// <summary>
        /// Runs learning epoch
        /// </summary>
        /// <param name="input">array of input vectors</param>
        /// <returns>Returns sum of learning errors</returns>
		double RunEpoch( double[][] input );
	}
}
