using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public static class AlgorithmExtensions
    {
        public static bool IsMonotoneIncreasing<T>(this List<T> values) where T : System.IComparable
        {
            for(int i = 0; i < values.Count - 1; ++i)
            {
                T cur = values[i], next = values[i + 1];
                if(cur.CompareTo(next) > 0) return false;
            }
            return true;
        }
        public static bool IsMonotoneDecreasing<T>(this List<T> values) where T : System.IComparable
        {
            for(int i = 0; i < values.Count - 1; ++i)
            {
                T cur = values[i], next = values[i + 1];
                if(cur.CompareTo(next) < 0) return false;
            }
            return true;
        }
        public static bool IsMonotonic<T>(this List<T> values) where T : System.IComparable
        {
            return IsMonotoneIncreasing(values) || IsMonotoneDecreasing(values);
        }
        public static int L1Norm(this Vector2Int lhs, Vector2Int rhs) => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y);
    }


}