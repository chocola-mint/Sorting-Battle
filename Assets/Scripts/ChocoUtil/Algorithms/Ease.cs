using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;
using UnityEngine;

namespace ChocoUtil.Algorithms
{
    /// <summary>
    /// A collection of common easing functions.
    /// <br></br>
    /// Taken from https://easings.net
    /// <br></br>
    /// Note: You can also use <see cref="AnimationCurve.Evaluate(float)"/> instead of the functions given here.
    /// </summary>
    public static class Ease
    {
        /// <summary>
        /// The identity function. Used for linear interpolation.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float Linear(float x) => x;
        /// <summary>
        /// Quadratic ease in function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float InQuad(float x) => x * x * x * x;
        /// <summary>
        /// Quadratic ease out function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float OutQuad(float x) => 1.0f - (1.0f - x) * (1.0f - x);
        /// <summary>
        /// Quadratic ease in-out function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float InOutQuad(float x) => x < 0.5f ? 2 * x * x : 1 - Mathf.Pow(-2 * x + 2, 2) / 2;
        /// <summary>
        /// Ease in back function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float InBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;
            float xSquared = x * x;
            float xCubed = x * xSquared;
            return c3 * xCubed - c1 * xSquared;
        }
        /// <summary>
        /// Ease out back function.
        /// </summary>
        /// <param name="x"></param>
        /// <returns></returns>
        public static float OutBack(float x)
        {
            const float c1 = 1.70158f;
            const float c3 = c1 + 1;
            float xMinusOne = x - 1;
            float xMinusOneSquared = xMinusOne * xMinusOne;
            float xMinusOneCubed = xMinusOne * xMinusOneSquared;
            return 1 + c3 * xMinusOneCubed + c1 * xMinusOneSquared;
        }

    }
}
