using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public static class AnimatorExtensions
    {
        public static IEnumerator WaitUntilCurrentStateIsDone(this Animator animator)
        {
            yield return null;
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
        }
        public static IEnumerator WaitUntilCurrentStateIsDone(this Animator animator, System.Action callback)
        {
            yield return WaitUntilCurrentStateIsDone(animator);
            callback();
        }
    }
}
