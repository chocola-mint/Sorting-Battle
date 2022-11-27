using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.MonoTween
{
    public abstract class MonoTweenBase : MonoBehaviour
    {
        protected Promise promise;
        protected static T Create<T>(GameObject ctx) where T : MonoTweenBase
        {
            var tween = ctx.AddComponent<T>();
            tween.enabled = false;
            return tween;
        }
        protected Promise FromExisting()
        {
            return new Promise(Init);
        }
        private void Init(Promise promise)
        {
            this.promise = promise;
            enabled = true;
            Begin();
        }
        protected abstract void Begin();
        protected void Done()
        {
            enabled = false;
            promise.Pass();
        }
        private void Awake() 
        {
            enabled = false;
        }
    }
}
