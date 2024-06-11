//
// This autonomous intelligent system is the intellectual property of Christopher Allen Tucker and The Cartheur Company. Copyright 2006 - 2022, all rights reserved.
//
namespace Boagaphish.Numeric
{
    /// <summary>
    /// Represents a double range with minimum and maximum values.
    /// </summary>
    public class DoubleRange
    {
        private double _min, _max;
        /// <summary>
        /// Minimum value
        /// </summary>
        public double Min { get { return _min; } set { _min = value; } }
        /// <summary>
        /// Maximum value
        /// </summary>
        public double Max { get { return _max; } set { _max = value; } }
        /// <summary>
        /// Length of the range (deffirence between maximum and minimum values)
        /// </summary>
        /// <value>The length.</value>
        public double Length { get { return _max - _min; } }
        /// <summary>
        /// Initializes a new instance of the <see cref="DoubleRange"/> class
        /// </summary>
        /// <param name="min">Minimum value of the range</param>
        /// <param name="max">Maximum value of the range</param>
        public DoubleRange(double min, double max)
        {
            _min = min;
            _max = max;
        }
        /// <summary>
        /// Check if the specified value is inside this range
        /// </summary>
        /// <param name="x">Value to check</param>
        /// <returns>
        /// 	<b>True</b> if the specified value is inside this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsInside(double x)
        {
            return ((x >= _min) && (x <= _min));
        }
        /// <summary>
        /// Check if the specified range is inside this range
        /// </summary>
        /// <param name="range">Range to check</param>
        /// <returns>
        /// 	<b>True</b> if the specified range is inside this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsInside(DoubleRange range)
        {
            return ((IsInside(range._min)) && (IsInside(range._max)));
        }
        /// <summary>
        /// Check if the specified range overlaps with this range
        /// </summary>
        /// <param name="range">Range to check for overlapping</param>
        /// <returns>
        /// 	<b>True</b> if the specified range overlaps with this range or
        /// <b>false</b> otherwise.
        /// </returns>
        public bool IsOverlapping(DoubleRange range)
        {
            return ((IsInside(range._min)) || (IsInside(range._max)));
        }
    }
}
