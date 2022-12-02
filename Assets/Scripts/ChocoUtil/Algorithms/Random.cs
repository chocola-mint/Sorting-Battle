using System;
using System.Collections.Generic;
using System.Text;

namespace ChocoUtil.Algorithms
{
    /// <summary>
    /// Algorithms involving randomness. Uses UnityEngine's built-in random state.
    /// </summary>
    public static class RandLib
    {
        /// <summary>
        /// Get the total weight of an array of weighted values.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="choices"></param>
        /// <returns>The total weight.</returns>
        public static float TotalWeight<T>(this WeightedValue<T>[] choices)
        {
            float totalWeight = 0;
            for (int i = 0; i < choices.Length; i++)
                totalWeight += choices[i].weight;
            return totalWeight;
        }
        /// <summary>
        /// Normalize all weights into [0, 1].
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="choices"></param>
        public static void NormalizeWeight<T>(this WeightedValue<T>[] choices)
        {
            NormalizeWeight(choices, TotalWeight(choices));
        }
        /// <summary>
        /// Divide all weights by totalWeight to normalize them.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="choices"></param>
        /// <param name="totalWeight"></param>
        public static void NormalizeWeight<T>(this WeightedValue<T>[] choices, float totalWeight)
        {
            for (int i = 0; i < choices.Length; i++)
                choices[i] = new(choices[i].weight / totalWeight, choices[i].value);
        }
        /// <summary>
        /// Select from an array of weighted values.
        /// <br></br>
        /// <br></br>
        /// Time complexity: O(n), where n is the length of the input array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="choices"></param>
        /// <returns>The value selected, randomly.</returns>
        public static T Select<T>(this WeightedValue<T>[] choices)
        {
            float totalWeight = TotalWeight(choices);
            float weightKey = UnityEngine.Random.Range(0.0f, totalWeight);
            float currentWeight = 0.0f;
            for(int i = 0; i < choices.Length; i++)
            {
                float nextWeight = currentWeight + choices[i].weight;
                if (nextWeight >= weightKey)
                    return choices[i].value;
                else currentWeight = nextWeight;
            }
            return choices[^1].value;
        }
        /// <summary>
        /// Performs the Fisher-Yates shuffle on the input array.
        /// <br></br><br></br>
        /// Time complexity: O(n), where n is the length of the input array.
        /// <br></br><br></br>
        /// Based on:
        /// <br></br>
        /// https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle#The_modern_algorithm
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        public static void InplaceShuffle<T>(this T[] items)
        {
            int n = items.Length;
            for (int i = n - 1; i >= 1; i--) 
            { 
                // Note: Using int version of Random.Range
                int j = UnityEngine.Random.Range(0, i);
                // Swap i and j
                (items[j], items[i]) = (items[i], items[j]);
            }
        }
        public static int[] RandomIntegerSequence(int start=0, int end=0, int increment=1)
        {
            int[] s = new int[(end - start) / increment];
            for(int i = start, j = 0; i < end; i += increment, j++)
                s[j] = i;
            s.InplaceShuffle();
            return s;
        }
        /// <summary>
        /// The immutable version of <see cref="InplaceShuffle{T}(T[])"/>. 
        /// <br></br>
        /// Returns a shuffled copy of the input array.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <returns></returns>
        public static T[] Shuffle<T>(this T[] items)
        {
            // Make a copy, then shuffle the copy.
            T[] copy = new T[items.Length];
            Array.Copy(items, 0, copy, 0, items.Length);
            InplaceShuffle(copy);
            return copy;
        }
    }
}
