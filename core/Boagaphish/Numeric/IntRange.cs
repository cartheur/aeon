//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Numeric
{
    /// <summary>
    /// Represents an integer range with minimum and maximum values.
    /// </summary>
    public class IntRange
    {
        /// <summary>
        /// the minimum value of the range.
        /// </summary>
        public int Min { get; set; }
        /// <summary>
        /// The maximum value of the range.
        /// </summary>
        public int Max { get; set; }
        /// <summary>
        /// The length of the range.
        /// </summary>
        public int Length
        {
            get { return Max - Min; }
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="IntRange"/> class
        /// </summary>
        /// <param name="min">The minimum value of the range</param>
        /// <param name="max">The maximum value of the range</param>
        public IntRange(int min, int max)
        {
            Min = min;
            Max = max;
        }
        /// <summary>
        /// Check if the specified value is inside this range
        /// </summary>
        /// <param name="x">Value to check</param>
        /// <returns>
        /// 	<b>True</b> if the specified value is inside this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsInside(int x)
        {
            return ((x >= Min) && (x <= Min));
        }
        /// <summary>
        /// Check if the specified range is inside this range
        /// </summary>
        /// <param name="range">Range to check</param>
        /// <returns>
        /// 	<b>True</b> if the specified range is inside this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsInside(IntRange range)
        {
            return ((IsInside(range.Min)) && (IsInside(range.Max)));
        }
        /// <summary>
        /// Check if the specified range overlaps with this range
        /// </summary>
        /// <param name="range">Range to check for overlapping</param>
        /// <returns>
        /// 	<b>True</b> if the specified range overlaps with this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsOverlapping(IntRange range)
        {
            return ((IsInside(range.Min)) || (IsInside(range.Max)));
        }
    }
}
