//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Core.Decision
{
    /// <summary>
    /// Collect all data before sending to the area where a decision is contemplated.
    /// </summary>
    public class Decider
    {
        public Decider(bool analyze)
        {
            if (analyze)
            {
                
            }
        }
        public static bool DecideNegative { get; set; }
        public static bool DecidePositive { get; set; }

        public static void MakeDecisionBasedOnTrend(double[,] inputs)
        {
            // Does the solution suggest an upward or downward trend?
            if (inputs[0,0] > 1)
            {
                DecideNegative = false;
                DecidePositive = true;
            }
            if (inputs[0,0] < 1)
            {
                DecideNegative = true;
                DecidePositive = false;
            }
        }
    }
}
