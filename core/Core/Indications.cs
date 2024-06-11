//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using Boagaphish.ActivationFunctions;
using Boagaphish.Core.Animals;
using Boagaphish.Core.Networks;
using Boagaphish.Numeric;
using Cartheur.Animals.Utilities;
using SweetPolynomial;

namespace Cartheur.Animals.Core
{
    /// <summary>
    /// The class responsible for engaging the learning algorithm.
    /// </summary>
    /// <remarks>USPTO-204-syntax parse initializes this process.</remarks>
    /// <remarks>204 leverages 302</remarks>
    public class Indication
    {
        /// <summary>
        /// The indication type.
        /// </summary>
        public enum IndicationType
        {
            /// <summary>
            /// The trajectory type.
            /// </summary>
            Trajectory,
            /// <summary>
            /// The emotive type.
            /// </summary>
            Emotive
        }
        // Core objects.
        private bool _needToStop;
        private double[] Data { get; set; }
        private double[,] Solution { get; set; }
        private double[,] Forecast { get; set; }
        private int SolutionSize { get; set; }
        private BackPropagationNetwork BackPropagationNetwork { get; set; }
        private double BackPropagationError { get; set; }
        private double BackPropagationMeanError { get; set; }
        private ActivationNetwork Activation { get; set; }
        private BackPropagation EmotiveNetwork { get; set; }
        private List<double> SentenceWeightTrainingData { get; set; }
        // Configuration-set properties.
        private int SampleSize { get; set; }
        /// <summary>
        /// Gets or sets the iterations.
        /// </summary>
        public int Iterations { get; set; }
        private int TrainingCycles { get; set; }
        /// <summary>
        /// Gets or sets the sigmoid alpha function.
        /// </summary>
        public double SigmoidAlpha { get; set; }
        private double Momentum { get; set; }
        private double LearningRate { get; set; }
        private int NumberOfLayers { get; set; }
        private int[] LayerSize { get; set; }
        private double[][] Bias { get; set; }
        private double[][][] Weight { get; set; }
        /// <summary>
        /// Gets or sets the size of the window.
        /// </summary>
        public int WindowSize { get; set; }
        private int PredictionSize { get; set; }
        private int PredictionSizeTrend { get; set; }
        private double Epoch { get; set; }
        private double LearningError { get; set; }
        private double PredictionError { get; set; }
        private double ForecastError { get; set; }
        // Created parameters.
        private double TrainingRate { get; set; }
        private double ErrorTolerance { get; set; }
        /// <summary>
        /// Gets or sets the transfer function.
        /// </summary>
        /// <value>
        /// The transfer function.
        /// </value>
        public TransferFunction TransferFunction { get; set; }
        /// <summary>
        /// Gets or sets the characteristic equation, which governs aeon's behaviour.
        /// </summary>
        /// <value>
        /// The characteristic equation.
        /// </value>
        public string CharacteristicEquation { get; set; }
        private Polynomial PolynomialExpression { get; set; }
        private double[][] Input { get; set; }
        private double[][] Desired { get; set; }
        private double NetworkCorrection { get; set; }
        private double[] NetworkInput { get; set; }
        private int HiddenLayer { get; set; }
        private int OutputLayer { get; set; }
        /// <summary>
        /// Initializes a new instance of the <see cref="Indication"/> class.
        /// </summary>
        /// <param name="function">The transfer function to initialize on.</param>
        /// <param name="characteristicEquation">The characteristic equation governing the indication. Can be empty if this feature is not desired.</param>
        public Indication(TransferFunction function, string characteristicEquation)
        {
            TransferFunction = function;
            CharacteristicEquation = characteristicEquation;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="Indication"/> class.
        /// </summary>
        /// <param name="function">The transfer function to initialize on.</param>
        /// <param name="polynomalExpression">The polynomial governing the indication.</param>
        public Indication(TransferFunction function, Polynomial polynomalExpression)
        {
            TransferFunction = function;
            PolynomialExpression = polynomalExpression;
        }
        /// <summary>
        /// Trains the network.
        /// </summary>
        /// <param name="outputFileName">Name of the output file.</param>
        /// <param name="saveAsNetworkFile">if set to <c>true</c> [save as network file].</param>
        /// <returns></returns>
        public double TrainNetwork(string outputFileName = "TrajectoryIndicationsTrainingData", bool saveAsNetworkFile = false)
        {
            // Call the test method to create the data objects.
            var dataOperations = new DataOperations();
            dataOperations.CreateTrainingData(CharacteristicEquation);
            SentenceWeightTrainingData = dataOperations.SentenceWeightSingle;
            Data = SentenceWeightTrainingData.ToArray();
            //Logging.WriteLog(@"Based on the data provided, now training the (solver) network.", Logging.LogType.Information, Logging.LogCaller.AgentCore);
            //TrainedNetworkName = outputFileName + XFileType;
            // Create performace measure point.
            // StartedOn = DateTime.Now;
            // Create the network if it does not already exist.
            if (BackPropagationNetwork == null)
            {
                var layerSizes = new[] { WindowSize, WindowSize * 2, 1 };
                var tFuncs = new[] { TransferFunction.None, TransferFunction, TransferFunction.Linear };
                BackPropagationNetwork = new BackPropagationNetwork(layerSizes, tFuncs) { Name = "TrainingIndicationData" };
            }
            // Prepare the number of learning samples. What does this dataset consist?
            SampleSize = Data.Length - PredictionSize - WindowSize;
            Input = new double[SampleSize][];
            Desired = new double[SampleSize][];
            // Loop.
            for (var i = 0; i < SampleSize; i++)
            {
                Input[i] = new double[WindowSize];
                Desired[i] = new double[1];
                // Set the input.
                for (var j = 0; j < WindowSize; j++)
                {
                    Input[i][j] = Data[i + j];
                }
                // Set the desired values.
                Desired[i][0] = Data[i + WindowSize];
            }
            // Train the network.
            var count = 0;
            do
            {
                // Prepare for training epoch.
                count++;
                // Train the network.
                for (var i = 0; i < Input.Length; i++)
                {
                    BackPropagationError += BackPropagationNetwork.Train(ref Input[i], ref Desired[i], TrainingRate, Momentum);
                    //TrainingCycles++;
                }
                BackPropagationMeanError = BackPropagationError / Input.Length;
                //Bias = BackPropagationNetwork.Bias;
                //Weight = BackPropagationNetwork.Weight;
                //Logging.WriteLog(@"Backpropagation error: " + BackPropagationError.ToString(CultureInfo.InvariantCulture), Logging.LogType.Information, Logging.LogCaller.Indications);

                for (var i = 0; i < WindowSize; i++)
                {
                    BackPropagationError += BackPropagationNetwork.Train(ref Input[i], ref Desired[i], TrainingRate, Momentum);
                    //TrainingCycles++;

                }
                BackPropagationMeanError = BackPropagationError / WindowSize;

                //Logging.WriteLog(@"Backpropagation residuals: " + BackPropagationError.ToString(CultureInfo.InvariantCulture), Logging.LogType.Information, Logging.LogCaller.Indications);

            } while (BackPropagationError > ErrorTolerance && count <= TrainingCycles);

            //if (saveAsNetworkFile)
            //BackPropagationNetwork.SaveNetworkXml(Environment.CurrentDirectory + @"\data\networks\" + TrainedNetworkName);
            // Record performance vectors for the operation.
            //SolverTrainingDone = true;
            //Duration = DateTime.Now - StartedOn;
            //Logging.WriteLog("Solver training processing time: " + Duration.Seconds + @"." + Duration.Milliseconds.ToString(CultureInfo.InvariantCulture), Logging.LogType.Statistics, Logging.LogCaller.AgentCore);

            return BackPropagationMeanError;
        }
        /// <summary>
        /// Searches the appropriate solution for the given inputs.
        /// </summary>
        /// <returns></returns>
        public double[,] SearchSolution()
        {
            _needToStop = false;
            //_workerThread = new Thread(SearchSolution) { Name = "SearchSolution Thread" };
            //StartedOn = DateTime.Now;
            // Set the network properties.
            HiddenLayer = WindowSize * 2;
            OutputLayer = 1;
            // Implement data transformation factors for the chart.
            var factor = 1.7 / 0.00405499999999992;
            var yMin = 0.852045;
            NetworkInput = new double[WindowSize];

            for (var i = 0; i < SampleSize; i++)
            {
                for (var j = 0; j < WindowSize; j++)
                {
                    Input[i][j] = (Data[i + j] - yMin) * factor - NetworkCorrection;
                }

                Desired[i][0] = (Data[i + WindowSize] - yMin) * factor - NetworkCorrection;
            }
            // Todo: Add the new code to the Bph library starting at these points below.
            switch (TransferFunction.ToString())
            {
                case "BipolarSigmoid":
                    Activation = new ActivationNetwork(new BipolarSigmoidFunction(SigmoidAlpha), WindowSize, HiddenLayer, OutputLayer);
                    break;
                case "Gaussian":
                    break;
                case "Linear":
                    break;
                case "NormalizedExponent":
                    break;
                case "RationalSigmoid":
                    break;
                case "Sigmoid":
                    Activation = new ActivationNetwork(new SigmoidFunction(SigmoidAlpha), WindowSize, HiddenLayer, OutputLayer);
                    break;
                case "VanderPol":
                    break;
            }
            NumberOfLayers = 2;
            LayerSize = new int[NumberOfLayers];
            LayerSize[0] = HiddenLayer;
            LayerSize[1] = OutputLayer;
            // One set of classes containing activation functions, from the first iteration of this code.
            EmotiveNetwork = new BackPropagation(Activation) { LearningRate = LearningRate, Momentum = Momentum };
            // This is here but have no historical trace. What send an array of zeros to the algorithm?
            Activation.Compute(NetworkInput); // This is an array of zeros. Can we feed the algorithm's correction here?
            LearningError += Math.Abs(Data[1] - ((Activation.Compute(NetworkInput)[0] + NetworkCorrection) / factor + yMin));
            // Iterations.
            var iteration = 1;
            // Solution array.
            SolutionSize = Data.Length - WindowSize;
            Solution = new double[SolutionSize, 2];
            var solution = new double[SolutionSize, 2];
            // Calculate x-values to be used with solution function.
            for (var j = 0; j < SolutionSize; j++)
            {
                Solution[j, 0] = j + WindowSize;
            }
            // Loop.
            while (!_needToStop)
            {
                // Run epoch of learning procedure.
                Epoch = EmotiveNetwork.RunEpoch(Input, Desired) / SampleSize;
                // Introduce data scaling.
                var scaleReduction = 0.1 * Data.Length;// <-- Nofucking way, not here!
                // Calculate solution, learning, and prediction errors by iterating the data.
                for (int i = 0, n = (int)scaleReduction - WindowSize; i < n; i++)
                {
                    // Assign values from current window as network's input.
                    for (var j = 0; j < WindowSize; j++)
                    {
                        NetworkInput[j] = (Data[i + j] - yMin) * factor - NetworkCorrection;
                        // Evaluate the function.
                        // solution[i, 1] = (Activation.Compute(NetworkInput)[i] + NetworkCorrection) / factor + yMin;
                    }
                    //var hold = 0;
                    // Evaluate the function.
                    // solution[i, 1] = (NetworkInput[i] + NetworkCorrection) / (factor + yMin);
                    var sum = Activation.Compute(NetworkInput);
                    solution[i, 1] = (sum[0] + NetworkCorrection);
                    Solution = solution;
                    // Truncate to a pre-chosen set of decimal places.
                    // solution[i, 1] = Math.Round(Solution[i, 1], TrailingDigits);
                    // Compute the prediction and learning error.
                    if (i >= n - PredictionSizeTrend)
                    {
                        PredictionError += Math.Abs(Solution[i, 1] - Data[WindowSize + i]);
                    }
                    else
                    {
                        LearningError += Math.Abs(Solution[i, 1] - Data[WindowSize + i]);
                    }
                }
                // Set the dataset maximum and minimum values.
                //DataMagnitudeUpper = Data.Max();
                //DataMagnitudeLower = Data.Min();
                // Increase the current iteration.
                iteration++;
                // Check if we need to stop.
                if ((Iterations != 0) && (iteration > Iterations))
                {
                    //_workerThread.Abort();
                    _needToStop = true;
                    //break;
                }
            }
            // Record performance vectors for the operation and logfile.
            //Duration = DateTime.Now - StartedOn;
            //Logging.WriteLog("Neural network processed its given task in " + Duration.Seconds + @"." + Duration.Milliseconds + " seconds", Logging.LogType.Statistics, Logging.LogCaller.AgentCore);
            //Logging.WriteLog("Program metrics: At an epoch of " + Epoch + ", " + Iterations + " iterations, " + LearningRate + " learning rate, " + PredictionSizeTrend + " prediction size for the trend computation; a sigmoid alpha value of " + SigmoidAlpha + " and a momentum of " + Momentum + " resulting in a learning error of " + LearningError + " and a prediction error of " + PredictionError, Logging.LogType.Statistics, Logging.LogCaller.AgentCore);
            return Solution;
        }
    }
}
