using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Text;
namespace SortGame
{
    public static class Csv
    {
        public static readonly char[] Delimiter = new []{',', '\n'};
        public static readonly char[] Newline = new []{'\n'};
        public static void WriteLine<T>(StringBuilder destination, List<T> values)
        {
            if(values.Count == 0) return;
            for(int i = 0; i < values.Count - 1; ++i)
                destination.Append($"{values[i]},");
            destination.Append($"{values[values.Count - 1]}\n");
        }
        public static IEnumerable<string> GetAllLines(string source)
        {
            foreach(var line in source.Split(Newline, StringSplitOptions.RemoveEmptyEntries))
                yield return line;
        }
        public static List<string> ReadLineAsString(string line)
        {
            return new(line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries));
        }
        public static List<int> ReadLineAsInt(string line)
        {
            List<int> result = new();
            foreach(var token in line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries))
                result.Add(int.Parse(token));
            return result;
        }
        public static List<float> ReadLineAsFloat(string line)
        {
            List<float> result = new();
            foreach(var token in line.Split(Delimiter, StringSplitOptions.RemoveEmptyEntries))
                result.Add(float.Parse(token));
            return result;
        }
    }
}
