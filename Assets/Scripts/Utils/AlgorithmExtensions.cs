using System.Collections;
using System.Collections.Generic;

namespace SortGame
{
    /// <summary>
    /// Extension methods that implement useful algorithms.
    /// </summary>
    public static class AlgorithmExtensions
    {
        /// <summary>
        /// Check if the sequence is monotonically increasing.
        /// <br></br>
        /// Example: 0 1 1 2 2 3 is a monotonically increasing sequence.
        /// </summary>
        /// <param name="values">The sequence to check.</param>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <returns>Yes or no.</returns>
        public static bool IsMonotoneIncreasing<T>(this List<T> values) where T : System.IComparable
        {
            for(int i = 0; i < values.Count - 1; ++i)
            {
                T cur = values[i], next = values[i + 1];
                if(cur.CompareTo(next) > 0) return false;
            }
            return true;
        }
        /// <summary>
        /// Check if the sequence is monotonically decreasing.
        /// <br></br>
        /// Example: 3 2 2 1 1 0 is a monotonically decreasing sequence.
        /// </summary>
        /// <param name="values">The sequence to check.</param>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <returns>Yes or no.</returns>
        public static bool IsMonotoneDecreasing<T>(this List<T> values) where T : System.IComparable
        {
            for(int i = 0; i < values.Count - 1; ++i)
            {
                T cur = values[i], next = values[i + 1];
                if(cur.CompareTo(next) < 0) return false;
            }
            return true;
        }
        /// <summary>
        /// Check if the sequence is monotonic. That is, if it's either monotonically increasing
        /// or decreasing.
        /// </summary>
        /// <param name="values">The sequence to check.</param>
        /// <typeparam name="T">The type of the sequence's elements</typeparam>
        /// <returns>Yes or no.</returns>
        public static bool IsMonotonic<T>(this List<T> values) where T : System.IComparable
        {
            return IsMonotoneIncreasing(values) || IsMonotoneDecreasing(values);
        }
    }


}