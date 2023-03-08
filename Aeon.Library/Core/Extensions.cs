//
// This AGI is the intellectual property of Dr. Christopher A. Tucker. Copyright 2023, all rights reserved. No rights are explicitly granted to persons who have obtained this source code.
//
using System.Reflection;
using System.Xml.Linq;

namespace Aeon.Library
{
    /// <summary>
    /// The class containing the extensions for the application.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Determines if the specified value has occurred previously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lastValue">The previous value.</param>
        /// <returns></returns>
        public static bool HasAppearedBefore(this string value, string lastValue)
        {
            return value.Contains(lastValue);
        }
        /// <summary>
        /// Determines if the specified value has occurred previously.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="lastValue">The previous value.</param>
        /// <returns></returns>
        public static bool HasAppearedBefore(this int value, int lastValue)
        {
            return value.Equals(lastValue);
        }
        /// <summary>
        /// Determines whether the specified value is between a minimum-maximum range (inclusive).
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="minimum">The minimum.</param>
        /// <param name="maximum">The maximum.</param>
        /// <returns>If the value is between a minimum and maximum threshold.</returns>
        public static bool IsBetween(this int value, int minimum, int maximum)
        {
            return value >= minimum && value <= maximum;
        }
        /// <summary>
        /// Returns the properties of the given object as XElements. Properties with null values are still returned, but as empty elements. Underscores in property names are replaces with hyphens.
        /// </summary>
        public static IEnumerable<XElement> AsXElements(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XElement(prop.Name.Replace("_", "-"), value);
            }
        }
        /// <summary>
        /// Returns the properties of the given object as XElements.Properties with null values are returned as empty attributes. Underscores in property names are replaces with hyphens.
        /// </summary>
        public static IEnumerable<XAttribute> AsXAttributes(this object source)
        {
            foreach (PropertyInfo prop in source.GetType().GetProperties())
            {
                object value = prop.GetValue(source, null);
                yield return new XAttribute(prop.Name.Replace("_", "-"), value ?? "");
            }
        }
    }
}
