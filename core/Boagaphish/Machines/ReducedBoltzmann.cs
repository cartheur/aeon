//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;

namespace Boagaphish.Machines
{
    /// <summary>
    /// An example of a reduced Boltzmann machine.
    /// </summary>
    public class ReducedBoltzmann
    {
        private readonly Random _rnd;

        public int NumberVisible { get; set; }
        public int NumberHidden { get; set; }

        public int[] VisibleNodeValues { get; set; } // visible node values (0, 1)
        public double[] VisualProbs;
        public double[] VisualBiases;

        public int[] HiddenValues;
        public double[] HiddenProbs;
        public double[] HiddenBiases;

        public double[][] VhWeights;

        public ReducedBoltzmann(int numVisible, int numHidden)
        {
            _rnd = new Random(0);

            NumberVisible = numVisible;
            NumberHidden = numHidden;

            // allocate arrays & the weights matrix
            VisibleNodeValues = new int[numVisible];
            VisualProbs = new double[numVisible];
            VisualBiases = new double[numVisible];

            HiddenValues = new int[numHidden];
            HiddenProbs = new double[numHidden];
            HiddenBiases = new double[numHidden];

            VhWeights = new double[numVisible][];  // visible-to-hidden
            for (int i = 0; i < numVisible; ++i)
                VhWeights[i] = new double[numHidden];

            // small random values for initial weights & biases
            const double low = -0.40;
            const double high = +0.40;
            for (int i = 0; i < numVisible; ++i)
                for (int j = 0; j < numHidden; ++j)
                    VhWeights[i][j] = (high - low) * _rnd.NextDouble() + low;

            for (int i = 0; i < numVisible; ++i)
                VisualBiases[i] = (high - low) * _rnd.NextDouble() + low;

            for (int i = 0; i < numHidden; ++i)
                HiddenBiases[i] = (high - low) * _rnd.NextDouble() + low;
        }

        //public void SetWeights(double[] wts)  // for debugging
        //{
        //  // order: weights, vBiases, hBiases
        //  int idx = 0;
        //  for (int i = 0; i < numVisible; ++i)
        //    for (int j = 0; j < numHidden; ++j)
        //      vhWeights[i][j] = wts[idx++];
        //  for (int i = 0; i < numVisible; ++i)
        //    visBiases[i] = wts[idx++];
        //  for (int j = 0; j < numHidden; ++j)
        //    hidBiases[j] = wts[idx++];
        //}

        //public double[] GetWeights()  // for debugging
        //{
        //  int numWts = numVisible * numHidden + numVisible + numHidden;
        //  double[] result = new double[numWts];
        //  int idx = 0;
        //  for (int i = 0; i < numVisible; ++i)
        //    for (int j = 0; j < numHidden; ++j)
        //      result[idx++] = vhWeights[i][j];
        //  for (int i = 0; i < numVisible; ++i)
        //    result[idx++] = visBiases[i];
        //  for (int j = 0; j < numHidden; ++j)
        //    result[idx++] = hidBiases[j];
        //  return result;
        //}

        public void Train(int[][] trainData, double learnRate, int maxEpochs)
        {
            int[] indices = new int[trainData.Length];
            for (int i = 0; i < indices.Length; ++i)
                indices[i] = i;

            int epoch = 0;
            while (epoch < maxEpochs)
            {
                Shuffle(indices);

                for (int idx = 0; idx < indices.Length; ++idx) // each data item
                {
                    int i = indices[idx];  // i points to curr train data

                    // copy visible values from train data into Machine
                    for (int j = 0; j < NumberVisible; ++j)
                        VisibleNodeValues[j] = trainData[i][j];

                    // compute hidden node values ('h' in Wikipedia)
                    for (int h = 0; h < NumberHidden; ++h)
                    {
                        double sum = 0.0;
                        for (int v = 0; v < NumberVisible; ++v)
                            sum += VisibleNodeValues[v] * VhWeights[v][h];

                        sum += HiddenBiases[h]; // add the hidden bias
                        HiddenProbs[h] = LogSign(sum); // compute prob of h activation
                        double pr = _rnd.NextDouble();  // determine 0/1 h node value
                        if (HiddenProbs[h] > pr)
                            HiddenValues[h] = 1;
                        else
                            HiddenValues[h] = 0;
                    }

                    // compute positive gradient =  outer product of v & h
                    int[][] posGrad = OuterProduct(VisibleNodeValues, HiddenValues);

                    // reconstruct visual Nodes as v'
                    int[] vPrime = new int[NumberVisible];  // v' in Wikipedia
                    for (int v = 0; v < NumberVisible; ++v)
                    {
                        double sum = 0.0;
                        for (int h = 0; h < NumberHidden; ++h)
                            sum += HiddenValues[h] * VhWeights[v][h];
                        sum += VisualBiases[v]; // add visible bias
                        double probActiv = LogSign(sum);
                        double pr = _rnd.NextDouble();
                        if (probActiv > pr)
                            vPrime[v] = 1;
                        else
                            vPrime[v] = 0;
                    }

                    // compute new hidden Nodes as h', using v'
                    int[] hPrime = new int[NumberHidden];
                    for (int h = 0; h < NumberHidden; ++h)
                    {
                        double sum = 0.0;
                        for (int v = 0; v < NumberVisible; ++v)
                            sum += vPrime[v] * VhWeights[v][h];
                        sum += HiddenBiases[h]; // add the hidden bias
                        double probActiv = LogSign(sum); // apply activation
                        double pr = _rnd.NextDouble();  // determine 0/1 node value
                        if (probActiv > pr)
                            hPrime[h] = 1;
                        else
                            hPrime[h] = 0;
                    }

                    // compute negative grad using v' and h'
                    int[][] negGrad = OuterProduct(vPrime, hPrime);

                    // update weights
                    for (int row = 0; row < NumberVisible; ++row)
                        for (int col = 0; col < NumberHidden; ++col)
                            VhWeights[row][col] += learnRate * (posGrad[row][col] - negGrad[row][col]);

                    // update visBiases
                    for (int v = 0; v < NumberVisible; ++v)
                        VisualBiases[v] += learnRate * (VisibleNodeValues[v] - vPrime[v]);
                    // update hidBiases
                    for (int h = 0; h < NumberHidden; ++h)
                        HiddenBiases[h] += learnRate * (HiddenValues[h] - hPrime[h]);

                } // for-each train data

                ++epoch;
            }
        }

        public static int[][] OuterProduct(int[] visValues, int[] hidValues)
        {
            int rows = visValues.Length;
            int cols = hidValues.Length;
            int[][] result = new int[rows][];
            for (int i = 0; i < rows; ++i)
                result[i] = new int[cols];

            for (int i = 0; i < rows; ++i)
                for (int j = 0; j < cols; ++j)
                    result[i][j] = visValues[i] * hidValues[j];

            return result;
        }

        public double LogSign(double x)
        {
            if (x < -20.0) return 0.0000000001;
            if (x > 20.0) return 0.9999999999;
            return 1.0 / (1.0 + Math.Exp(-x));
        }

        public void Shuffle(int[] indices)
        {
            for (int i = 0; i < indices.Length; ++i)
            {
                int ri = _rnd.Next(i, indices.Length);
                int tmp = indices[i];
                indices[i] = indices[ri];
                indices[ri] = tmp;
            }
        }

        public int[] HiddenFromVisual(int[] visibles)
        {
            int[] result = new int[NumberHidden];

            for (int h = 0; h < NumberHidden; ++h)
            {
                double sum = 0.0;
                for (int v = 0; v < NumberVisible; ++v)
                    sum += visibles[v] * VhWeights[v][h];

                sum += HiddenBiases[h]; // add the hidden bias
                double probActiv = LogSign(sum); // compute prob of h activation
                                                // Console.WriteLine("Hidden [" + h + "] activation probability = " + probActiv.ToString("F4"));
                double pr = _rnd.NextDouble();  // determine 0/1 h node value
                if (probActiv > pr)
                    result[h] = 1;
                else
                    result[h] = 0;
            }
            return result;
        }

        public int[] VisibleFromHidden(int[] hiddens)
        {
            int[] result = new int[NumberVisible];

            for (int v = 0; v < NumberVisible; ++v)
            {
                double sum = 0.0;
                for (int h = 0; h < NumberHidden; ++h)
                    sum += hiddens[h] * VhWeights[v][h];
                sum += VisualBiases[v]; // add visible bias
                double probActiv = LogSign(sum);
                // Console.WriteLine("Visible [" + v + "] activation probability = " + probActiv.ToString("F4"));
                double pr = _rnd.NextDouble();
                if (probActiv > pr)
                    result[v] = 1;
                else
                    result[v] = 0;
            }
            return result;
        }

        public void Dump(bool showValues, bool showWeights, bool showBiases)
        {
            if (showValues)
            {
                for (int i = 0; i < NumberVisible; ++i)
                {
                    Console.Write("visible node [" + i + "] value = " + VisibleNodeValues[i]);
                    Console.WriteLine("  prob = " + VisualProbs[i].ToString("F4"));
                }
                Console.WriteLine("");

                for (int j = 0; j < NumberHidden; ++j)
                {
                    Console.Write("hidden node [" + j + "] value = " + HiddenValues[j]);
                    Console.WriteLine("  prob = " + HiddenProbs[j].ToString("F4"));
                }
                Console.WriteLine("");
            }

            if (showWeights)
            {
                for (int i = 0; i < NumberVisible; ++i)
                {
                    for (int j = 0; j < NumberHidden; ++j)
                    {
                        double x = VhWeights[i][j];
                        if (x >= 0.0)
                            Console.Write(" ");
                        Console.Write(VhWeights[i][j].ToString("F4") + "  ");
                    }
                    Console.WriteLine("");
                }
                Console.WriteLine("");
            }

            if (showBiases)
            {
                for (int i = 0; i < NumberVisible; ++i)
                    Console.WriteLine("visible bias [" + i + "] value = " +
                      VisualBiases[i].ToString("F4"));
                Console.WriteLine("");

                for (int j = 0; j < NumberHidden; ++j)
                    Console.WriteLine("hidden bias [" + j + "] value = " +
                      HiddenBiases[j].ToString("F4"));
                Console.WriteLine("");
            }
        }
    }
}
