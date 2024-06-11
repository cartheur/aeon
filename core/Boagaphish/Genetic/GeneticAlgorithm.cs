//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections;
using System.IO;

namespace Boagaphish.Genetic
{
    public class GeneticAlgorithm
    {
        private double _totalFitness;
        private ArrayList _thisGeneration;
        private ArrayList _nextGeneration;
        private ArrayList _fitnessTable;
        private static readonly Random Random = new Random();
        private static GeneticAlgorithmFunction _getFitness;

        public GeneticAlgorithmFunction FitnessFunction
        {
            get
            {
                return _getFitness;
            }
            set
            {
                _getFitness = value;
            }
        }

        public int PopulationSize
        {
            get;
            set;
        }

        public int Generations
        {
            get;
            set;
        }

        public int GenomeSize
        {
            get;
            set;
        }

        public double CrossoverRate
        {
            get;
            set;
        }

        public double MutationRate
        {
            get;
            set;
        }

        public string FitnessFile
        {
            get;
            set;
        }

        public bool Elitism
        {
            get;
            set;
        }

        public GeneticAlgorithm()
        {
            InitialValues();
            MutationRate = 0.05;
            CrossoverRate = 0.8;
            PopulationSize = 100;
            Generations = 2000;
            FitnessFile = "";
        }

        public GeneticAlgorithm(double crossoverRate, double mutationRate, int populationSize, int generationSize, int genomeSize)
        {
            InitialValues();
            MutationRate = mutationRate;
            CrossoverRate = crossoverRate;
            PopulationSize = populationSize;
            Generations = generationSize;
            GenomeSize = genomeSize;
            FitnessFile = "";
        }

        public GeneticAlgorithm(int genomeSize)
        {
            InitialValues();
            GenomeSize = genomeSize;
        }

        public void InitialValues()
        {
            Elitism = false;
        }

        public void Go()
        {
            if (_getFitness == null)
            {
                throw new ArgumentNullException("Need to supply fitness function.");
            }
            if (GenomeSize == 0)
            {
                throw new IndexOutOfRangeException("Genome size not set.");
            }
            _fitnessTable = new ArrayList();
            _thisGeneration = new ArrayList(Generations);
            _nextGeneration = new ArrayList(Generations);
            Genome.MutationRate = MutationRate;
            CreateGenomes();
            RankPopulation();
            StreamWriter streamWriter = null;
            bool flag = false;
            if (FitnessFile != "")
            {
                flag = true;
                streamWriter = new StreamWriter(FitnessFile);
            }
            for (int i = 0; i < Generations; i++)
            {
                CreateNextGeneration();
                RankPopulation();
                if (flag && streamWriter != null)
                {
                    double fitness = ((Genome)_thisGeneration[PopulationSize - 1]).Fitness;
                    streamWriter.WriteLine("{0},{1}", i, fitness);
                }
            }
            if (streamWriter != null)
            {
                streamWriter.Close();
            }
        }

        private int RouletteSelection()
        {
            double num = Random.NextDouble() * _totalFitness;
            int num2 = -1;
            int num3 = 0;
            int num4 = PopulationSize - 1;
            int num5 = (num4 - num3) / 2;
            while (num2 == -1 && num3 <= num4)
            {
                if (num < (double)_fitnessTable[num5])
                {
                    num4 = num5;
                }
                else if (num > (double)_fitnessTable[num5])
                {
                    num3 = num5;
                }
                num5 = (num3 + num4) / 2;
                if (num4 - num3 == 1)
                {
                    num2 = num4;
                }
            }
            return num2;
        }

        private void RankPopulation()
        {
            _totalFitness = 0.0;
            for (int i = 0; i < PopulationSize; i++)
            {
                Genome genome = (Genome)_thisGeneration[i];
                genome.Fitness = FitnessFunction(genome.Genes());
                _totalFitness += genome.Fitness;
            }
            _thisGeneration.Sort(new GenomeComparer());
            double num = 0.0;
            _fitnessTable.Clear();
            for (int j = 0; j < PopulationSize; j++)
            {
                num += ((Genome)_thisGeneration[j]).Fitness;
                _fitnessTable.Add(num);
            }
        }

        private void CreateGenomes()
        {
            for (int i = 0; i < PopulationSize; i++)
            {
                Genome value = new Genome(GenomeSize);
                _thisGeneration.Add(value);
            }
        }

        private void CreateNextGeneration()
        {
            _nextGeneration.Clear();
            Genome genome = null;
            if (Elitism)
            {
                genome = (Genome)_thisGeneration[PopulationSize - 1];
            }
            for (int i = 0; i < PopulationSize; i += 2)
            {
                int index = RouletteSelection();
                int index2 = RouletteSelection();
                Genome genome2 = (Genome)_thisGeneration[index];
                Genome genome3 = (Genome)_thisGeneration[index2];
                Genome genome4;
                Genome genome5;
                if (Random.NextDouble() < CrossoverRate)
                {
                    genome2.Crossover(ref genome3, out genome4, out genome5);
                }
                else
                {
                    genome4 = genome2;
                    genome5 = genome3;
                }
                genome4.Mutate();
                genome5.Mutate();
                _nextGeneration.Add(genome4);
                _nextGeneration.Add(genome5);
            }
            if (Elitism && genome != null)
            {
                _nextGeneration[0] = genome;
            }
            _thisGeneration.Clear();
            for (int j = 0; j < PopulationSize; j++)
            {
                _thisGeneration.Add(_nextGeneration[j]);
            }
        }

        public void GetBest(out double[] values, out double fitness)
        {
            Genome genome = (Genome)_thisGeneration[PopulationSize - 1];
            values = new double[genome.Length];
            genome.GetValues(ref values);
            fitness = genome.Fitness;
        }

        public void GetWorst(out double[] values, out double fitness)
        {
            GetNthGenome(0, out values, out fitness);
        }

        public void GetNthGenome(int n, out double[] values, out double fitness)
        {
            if (n < 0 || n > PopulationSize - 1)
            {
                throw new ArgumentOutOfRangeException("n too large, or too small");
            }
            Genome genome = (Genome)_thisGeneration[n];
            values = new double[genome.Length];
            genome.GetValues(ref values);
            fitness = genome.Fitness;
        }
    }
}
