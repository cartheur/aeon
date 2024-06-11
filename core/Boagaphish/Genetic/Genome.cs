//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Globalization;

namespace Boagaphish.Genetic
{
    public class Genome
    {
        private static readonly Random Random = new Random();
        private double[] genes;

        public double Fitness
        {
            get;
            set;
        }

        public static double MutationRate
        {
            get;
            set;
        }

        public int Length
        {
            get;
            private set;
        }

        public Genome()
        {
        }

        public Genome(int length)
        {
            Length = length;
            genes = new double[length];
            CreateGenes();
        }

        public Genome(int length, bool createGenes)
        {
            Length = length;
            genes = new double[length];
            if (createGenes)
            {
                CreateGenes();
            }
        }

        public Genome(ref double[] genes)
        {
            Length = genes.GetLength(0);
            this.genes = new double[Length];
            for (int i = 0; i < Length; i++)
            {
                this.genes[i] = genes[i];
            }
        }

        private void CreateGenes()
        {
            DateTime utcNow = DateTime.UtcNow;
            for (int i = 0; i < Length; i++)
            {
                genes[i] = Random.NextDouble();
            }
        }

        public void Crossover(ref Genome genome2, out Genome child1, out Genome child2)
        {
            int num = (int)(Random.NextDouble() * (double)Length);
            child1 = new Genome(Length, false);
            child2 = new Genome(Length, false);
            for (int i = 0; i < Length; i++)
            {
                if (i < num)
                {
                    child1.genes[i] = genes[i];
                    child2.genes[i] = genome2.genes[i];
                }
                else
                {
                    child1.genes[i] = genome2.genes[i];
                    child2.genes[i] = genes[i];
                }
            }
        }

        public void Mutate()
        {
            for (int i = 0; i < Length; i++)
            {
                if (Random.NextDouble() < MutationRate)
                {
                    genes[i] = (genes[i] + Random.NextDouble()) / 2.0;
                }
            }
        }

        public double[] Genes()
        {
            return genes;
        }

        public void Output()
        {
            for (int i = 0; i < Length; i++)
            {
                Logging.WriteLog(genes[i].ToString(CultureInfo.InvariantCulture), Logging.LogType.Information, Logging.LogCaller.DeepLearning);
            }
        }

        public void GetValues(ref double[] values)
        {
            for (int i = 0; i < Length; i++)
            {
                values[i] = genes[i];
            }
        }
    }
}
