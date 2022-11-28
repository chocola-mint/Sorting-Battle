using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using UnityEngine;

namespace ChocoUtil.Coroutines
{
    /// <summary>
    /// Extension methods (mostly syntax sugar) for IEnumerator-based coroutines.
    /// </summary>
    public static class CoroutineExtensions
    {
        /// <summary>
        /// Chain two coroutines together.
        /// </summary>
        /// <param name="doThisFirst"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        public static IEnumerator Then(this IEnumerator doThisFirst, IEnumerator then)
        {
            yield return doThisFirst;
            yield return then;
        }
        /// <summary>
        /// Chain a coroutine with an Action.
        /// </summary>
        /// <param name="doThisFirst"></param>
        /// <param name="then"></param>
        /// <returns></returns>
        public static IEnumerator Then(this IEnumerator doThisFirst, Action then)
        {
            yield return doThisFirst;
            then();
        }
        public static IEnumerator All(this MonoBehaviour context, params IEnumerator[] coroutines)
        {
            Coroutine[] handles = new Coroutine[coroutines.Length];
            for(int i = 0; i < coroutines.Length; ++i)
                handles[i] = context.StartCoroutine(coroutines[i]);
            yield return context.All(handles);
        }
        public static IEnumerator All(this MonoBehaviour context, params Coroutine[] handles)
        {
            for(int i = 0; i < handles.Length; ++i)
                yield return handles[i];
        }
    }
}
