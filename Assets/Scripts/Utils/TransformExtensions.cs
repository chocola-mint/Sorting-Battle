using System.Collections.Generic;
using UnityEngine;
namespace SortGame
{
    /// <summary>
    ///  Extension methods for Unity's Transform component.
    /// </summary>
    public static class TransformExtensions
    {
        /// <summary>
        /// Immediately destroy all children of the given Transform.
        /// </summary>
        public static void DestroyAllChildren(this Transform transform)
        {
            while (transform.childCount > 0) 
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}