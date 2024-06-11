//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;

namespace Boagaphish.Genetic
{
    public sealed class GenomeComparer : IComparer
    {
        private const double Epsilon = 1E-06;

        public int Compare(object x, object y)
        {
            if (!(x is Genome) || !(y is Genome))
            {
                throw new ArgumentException("Not of type Genome.");
            }
            if (((Genome)x).Fitness > ((Genome)y).Fitness)
            {
                return 1;
            }
            if (Math.Abs(((Genome)x).Fitness - ((Genome)y).Fitness) < Epsilon)
            {
                return 0;
            }
            return -1;
        }
    }
}
