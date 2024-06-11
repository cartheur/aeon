//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Cartheur.Animals.Core;

namespace Cartheur.Animals.Utilities
{
    /// <summary>
    /// A two tuple.
    /// </summary>
    /// <typeparam name="T1">The type of one.</typeparam>
    /// <typeparam name="T2">The type of two.</typeparam>
    public class TupleTwo<T1, T2> : List<Tuple<T1, T2>>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="item2">The item2.</param>
        public void Add(T1 item, T2 item2)
        {
            Add(new Tuple<T1, T2>(item, item2));
        }
    }
    /// <summary>
    /// A three tuple.
    /// </summary>
    /// <typeparam name="T1">The type of one.</typeparam>
    /// <typeparam name="T2">The type of two.</typeparam>
    /// <typeparam name="T3">The type of three.</typeparam>
    public class TupleTwee<T1, T2, T3> : List<Tuple<T1, T2, T3>>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="item3">The item3.</param>
        public void Add(T1 item, T2 item2, T3 item3)
        {
            Add(new Tuple<T1, T2, T3>(item, item2, item3));
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T1">The type of one.</typeparam>
    /// <typeparam name="T2">The type of two.</typeparam>
    /// <typeparam name="T3">The type of three.</typeparam>
    /// <typeparam name="T4">The type of four.</typeparam>
    public class TupleQuad<T1, T2, T3, T4> : List<Tuple<T1, T2, T3, T4>>
    {
        /// <summary>
        /// Adds the specified item.
        /// </summary>
        /// <param name="item">The item.</param>
        /// <param name="item2">The item2.</param>
        /// <param name="item3">The item3.</param>
        /// <param name="item4">The item4.</param>
        public void Add(T1 item, T2 item2, T3 item3, T4 item4)
        {
            Add(new Tuple<T1, T2, T3, T4>(item, item2, item3, item4));
        }
    }

    /// <summary>
    /// The class containing the data operations.
    /// </summary>
    public class DataOperations
    {
        /// <summary>
        /// Gets or sets this aeon the operations apply to.
        /// </summary>
        protected Aeon ThisAeon { get; set; }
        protected LoaderPaths Configuration;
        /// <summary>
        /// Gets or sets the freqeuncy word dictionary.
        /// </summary>
        protected Dictionary<string, int> FreqeuncyWordDictionary { get; set; }
        /// <summary>
        /// Gets or sets the relationship list.
        /// </summary>
        protected List<string> RelationshipList { get; set; }
        /// <summary>
        /// Gets or sets the item means sentence.
        /// </summary>
        public TupleTwo<int, string> ItemMeansSentence { get; set; }
        /// <summary>
        /// Gets or sets the sentence weight single.
        /// </summary>
        public List<double> SentenceWeightSingle { get; set; }
        /// <summary>
        /// Gets or sets the sentence weight duo.
        /// </summary>
        public TupleTwo<double, int> SentenceWeightDuo { get; set; }
        /// <summary>
        /// Gets or sets the sentence weight triple.
        /// </summary>
        public TupleTwee<double, int, int> SentenceWeightTriple { get; set; }
        /// <summary>
        /// Gets or sets the sentence weight four.
        /// </summary>
        public TupleQuad<double, int, int, TupleTwo<int, string>> SentenceWeightFour { get; set; }
        /// <summary>
        /// Compiles the training set.
        /// </summary>
        /// <param name="file">The file.</param>
        public static void CompileTrainingSet(StringBuilder file)
        {
            
        }
        /// <summary>
        /// Stores the training set.
        /// </summary>
        /// <param name="thisAeon">The this aeon.</param>
        /// <returns></returns>
        public static bool StoreTrainingSet(Aeon thisAeon)
        {
            // Write to file for training set.
            var fileOutput = new StringBuilder();

            try
            {
                var streamWriter = new StreamWriter(Environment.CurrentDirectory + thisAeon.GlobalSettings.GrabSetting("learningdataset"), true);
                streamWriter.WriteLine(fileOutput);
                streamWriter.Close();
            }
            catch (Exception ex)
            {
                Logging.WriteLog(ex.Message, Logging.LogType.Error, Logging.LogCaller.Me);
                return false;
            }
            return true;
        }
        /// <summary>
        /// Creates the training data.
        /// </summary>
        public void CreateTrainingData(string characteristicEquation)
        {
            Configuration = new LoaderPaths("Debug");
            ThisAeon = new Aeon(characteristicEquation);
            ThisAeon.LoadSettings(Configuration.PathToSettings);
            FreqeuncyWordDictionary = new Dictionary<string, int>();
            ItemMeansSentence = new TupleTwo<int, string>();
            SentenceWeightSingle = new List<double>();
            SentenceWeightDuo = new TupleTwo<double, int>();
            SentenceWeightTriple = new TupleTwee<double, int, int>();
            SentenceWeightFour = new TupleQuad<double, int, int, TupleTwo<int, string>>();
            // Step 1: Create a dictionary object from the seed *.csv file.
            using (var seedReader = new StreamReader(Environment.CurrentDirectory + ThisAeon.GlobalSettings.GrabSetting("learningdatasetseed")))
            {
                while (!seedReader.EndOfStream)
                {
                    var line = seedReader.ReadLine();
                    var values = line.Split(',');
                    if (!FreqeuncyWordDictionary.ContainsKey(values[0]))
                    {
                        FreqeuncyWordDictionary.Add(values[0], Convert.ToInt32(values[1]));
                    }
                }
            }
            // Step 2: Read the relationship-type file into a list.
            var relationshipFile = File.ReadAllLines(Environment.CurrentDirectory + ThisAeon.GlobalSettings.GrabSetting("relationshipfile"));
            RelationshipList = new List<string>(relationshipFile).Distinct().ToList();
            // Step 3: Assigned a value to each sentence from the relationship list from a lookup in the dictionary.
            // a. Split the sentences and lookup each word from the dictionary.
            // b. Compute the mean value of each sentence value.
            // c. Store the value along with the word count of each sentence into a tuple.
            foreach (var sentence in RelationshipList)
            {
                var words = sentence.Split(' ');
                var itemMean = new List<int>();
                foreach (var s in words)
                {
                    if (FreqeuncyWordDictionary.ContainsKey(s))
                    {
                        var itemValue = FreqeuncyWordDictionary[s];
                        ItemMeansSentence.Add(itemValue, sentence);
                        itemMean.Add(itemValue);
                    }
                }
                var meanValueZipf = itemMean.Count > 0 ? itemMean.Average() : 0.0;
                SentenceWeightSingle.Add(meanValueZipf);
                SentenceWeightDuo.Add(meanValueZipf, words.Count());
                SentenceWeightTriple.Add(meanValueZipf, words.Count(), sentence.Length);
                SentenceWeightFour.Add(meanValueZipf, words.Count(), sentence.Length, ItemMeansSentence);
            }
            // Step 4: Congratulations, you now have the compiled training list with a tuple containing the index to prevent entropy and aid in training the algorithm as I think one needs the answer to be able to determine if it is functioning correctly. Not exactly sure of the object format as it is a little heavy now. Later.
            // Or how about persisting the dataobjects for use in the training data, which by the way would be the better option?!
        }
    }
    /// <summary>
    /// The extension class for probability utilities.
    /// </summary>
    public static class ProbabilityUtilities
    {
        // Was Zipf from the MathNet library.
    }
}
