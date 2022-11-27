using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.MonoTween
{
    public class Promise
    {
        private Queue<System.Action<Promise>> callbacks = new();
        /// <summary>
        /// Alias for <see cref="Pass"></see>
        /// </summary>
        public void Start()
        {
            Pass();
        }
        public void Pass()
        {
            if(callbacks.TryDequeue(out var callback))
                callback(this);
        }
        public Promise(System.Action<Promise> initialCallback)
        {
            callbacks.Enqueue(initialCallback);
        }
        
        public Promise Then(System.Action<Promise> callback)
        {
            callbacks.Enqueue(callback);
            return this;
        }
    }
}
