using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Linear Algebra functions.
    /// </summary>
    public static class LinAlg
    {
        public static int L1Norm(Vector2Int lhs, Vector2Int rhs) 
            => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y);
        public static float L1Norm(Vector2 lhs, Vector2 rhs) 
            => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y);

    }
}
