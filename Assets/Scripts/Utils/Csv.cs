using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
namespace SortGame
{
    /// <summary>
    /// Csv utilities to parse values from comma-separated values.
    /// </summary>
    public static class Csv
    {
        public static readonly char[] Delimiter = new []{',', '\n'};
        public static readonly char[] Newline = new []{'\n'};
        /// <summary>
        /// Append a line of comma-separated values to the given StringBuilder.
        /// </summary>
        /// <param name="destination">A StringBuilder to append the results to.</param>
        /// <param name="values">The sequence to write.</param>
        /// <typeparam name="T">The shared type of the values.</typeparam>
        public static void WriteLine<T>(StringBuilder destination, List<T> values)
        {
            if(values.Count == 0) return;
            for(int i = 0; i < values.Count - 1; ++i)
                destination.Append($"{values[i]},");
            destination.Append($"{values[values.Count - 1]}\n");
        }
        /// <summary>
        /// Iterator that iterates over each line in the source string.
        /// </summary>
        /// <param name="source">The source string to parse.</param>
        /// <returns>An iterator. Use foreach statements!</returns>
        public static IEnumerable<string> GetAllLines(string source)
        {
            foreach(var line in source.Split(Newline, StringSplitOptions.RemoveEmptyEntries))
                yield return line;
        }
        /// <summary>
        /// Read a line of comma-separated values as comma-separated strings.
        /// </summary>
        /// <param name="line">Comma-separated values.</param>
        /// <returns>A List containing each string in the sequence.</returns>
        public static List<string> ReadLineAsString(string line)
        {
            return new(line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries));
        }
        /// <summary>
        /// Read a line of comma-separated values as comma-separated integers.
        /// </summary>
        /// <param name="line">Comma-separated values.</param>
        /// <returns>A List containing each integer in the sequence.</returns>
        public static List<int> ReadLineAsInt(string line)
        {
            List<int> result = new();
            foreach(var token in line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries))
                result.Add(int.Parse(token));
            return result;
        }
        /// <summary>
        /// Read a line of comma-separated values as comma-separated floats.
        /// </summary>
        /// <param name="line">Comma-separated values.</param>
        /// <returns>A List containing each float in the sequence.</returns>
        public static List<float> ReadLineAsFloat(string line)
        {
            List<float> result = new();
            foreach(var token in line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries))
                result.Add(float.Parse(token));
            return result;
        }
    }
}
