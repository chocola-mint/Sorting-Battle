using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SortGame
{
    public static class TransformExtensions
    {
        public static void DestroyAllChildren(this Transform transform)
        {
            while (transform.childCount > 0) 
            {
                GameObject.DestroyImmediate(transform.GetChild(0).gameObject);
            }
        }
    }
}