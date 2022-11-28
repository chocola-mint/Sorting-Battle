using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using ChocoUtil.Algorithms;

namespace ChocoUtil.Coroutines
{
    /// <summary>
    /// A collection of common tweening applications.
    /// </summary>
    public static partial class Tween
    {
        /// <summary>
        /// Fade an image between two colors smoothly.
        /// </summary>
        /// <param name="image"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="useUnscaledTime"></param>
        /// <returns></returns>
        public static IEnumerator Fade(this Image image, Color from, Color to, float duration, Func<float, float> ease, bool useUnscaledTime = false)
        {
            yield return null;
            foreach(float t in Interp.GetSteps(duration, ease, useUnscaledTime))
            {
                image.color = Color.LerpUnclamped(from, to, t);
                yield return null;
            }
            image.color = to;
        }
        /// <summary>
        /// Move a transform in world space smoothly.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="useUnscaledTime"></param>
        /// <returns></returns>
        public static IEnumerator MoveWorld(this Transform transform, Vector3 from, Vector3 to, float duration, Func<float, float> ease, bool useUnscaledTime = false)
        {
            yield return null;
            foreach (float t in Interp.GetSteps(duration, ease, useUnscaledTime))
            {
                transform.position = Vector3.LerpUnclamped(from, to, t);
                yield return null;
            }
            transform.position = to;
        }
        /// <summary>
        /// Move a transform in local space smoothly.
        /// </summary>
        /// <param name="transform"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="useUnscaledTime"></param>
        /// <returns></returns>
        public static IEnumerator MoveLocal(this Transform transform, Vector3 from, Vector3 to, float duration, Func<float, float> ease, bool useUnscaledTime = false)
        {
            yield return null;
            foreach (float t in Interp.GetSteps(duration, ease, useUnscaledTime))
            {
                transform.localPosition = Vector3.LerpUnclamped(from, to, t);
                yield return null;
            }
            transform.localPosition = to;
        }
    }
}
