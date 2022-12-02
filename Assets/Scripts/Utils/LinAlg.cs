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
        /// <summary>
        /// Compute the L1-norm (Manhattan distance) of two Vector2Ints.
        /// </summary>
        public static int L1Norm(Vector2Int lhs, Vector2Int rhs) 
            => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y);
        /// <summary>
        /// Compute the L1-norm (Manhattan distance) of two Vector2s.
        /// </summary>
        public static float L1Norm(Vector2 lhs, Vector2 rhs) 
            => Mathf.Abs(lhs.x - rhs.x) + Mathf.Abs(lhs.y - rhs.y);
        /// <summary>
        /// Project given point to the plane "Z = z"
        /// </summary>
        public static Vector3 MapToZPlane(this Vector3 point, float z)
        {
            return new(point.x, point.y, z);
        }
        /// <summary>
        /// Project given point to the plane "Z = z"
        /// </summary>
        public static Vector3 MapToZPlane(this Vector2 point, float z)
        {
            return new(point.x, point.y, z);
        }
        public static IEnumerable<Vector2Int> AdjacentPoints(this Vector2Int point)
        {
            yield return point + Vector2Int.up;
            yield return point + Vector2Int.right;
            yield return point + Vector2Int.down;
            yield return point + Vector2Int.left;
        }
    }
}
