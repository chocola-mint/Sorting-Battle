using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace ChocoUtil.Algorithms
{
    /// <summary>
    /// Interpolation utilities.
    /// </summary>
    public static class Interp
    {
        /// <summary>
        /// A generic interpolator iterator that gets every t in each step of the interpolation.
        /// <br></br>
        /// <code>
        /// Example: 
        /// foreach(float t in Interp.GetFrames(1.0f, Ease.Linear, false) { 
        ///     transform.position = Vector3.Lerp(Vector3.zero, Vector3.one, t);
        ///     yield return null;
        /// }
        /// </code>
        /// </summary>
        /// <param name="duration"></param>
        /// <param name="ease"></param>
        /// <param name="useUnscaledTime"></param>
        /// <returns></returns>
        public static IEnumerable<float> GetSteps(float duration, Func<float, float> ease, bool useUnscaledTime = false)
        {
            float startTime = useUnscaledTime ? Time.unscaledTime : Time.time;
            do yield return ease(((useUnscaledTime ? Time.unscaledTime : Time.time) - startTime) / duration); // User expected to wait a frame after getting the current frame.
            while ((useUnscaledTime ? Time.unscaledTime : Time.time) < startTime + duration);
        }
    }   
}
