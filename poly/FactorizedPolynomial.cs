//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace SweetPolynomial
{
    /// <summary>
    /// Factorized polynomial p := set of polynomials p_1,...,p_k and their corRealsponding powers n_1,...,n_k, such that p = (p_1)^(n_1)*...*(p_k)^(n_k).
    /// </summary>
    public struct FactorizedPolynomial
    {
        /// <summary>
        /// Set of factors the polynomial consists of.
        /// </summary>
        public Polynomial[] Factor;
        /// <summary>
        /// Set of powers, where the real Factor[i] is lifted to Power[i].
        /// </summary>
        public int[] Power;
    }
    /// <summary>
    /// The differential operator structure.
    /// </summary>
    public struct DifferentialOperator
    {
        /// <summary>
        /// Set of operations for the differential.
        /// </summary>
        public Polynomial[] Operator;
        /// <summary>
        /// Set of magnitudes, where the real-valued Operator[i] has Magnitude[i].
        /// </summary>
        public int[] Magnitude;
    }
}
