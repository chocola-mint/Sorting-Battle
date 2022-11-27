using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.MonoTween
{
    public class TMove : MonoTweenBase
    {
        public struct Args
        {
            public Vector3 from;
            public Vector3 to;
            public float duration;
            public AnimationCurve curve;
            public bool useUnscaledTime;
            public bool destroyOnDone;
        }
        private Args args;
        private float startTime = 0;
        private float currentTime => args.useUnscaledTime ? Time.unscaledTime : Time.time;
        public static Promise Create(GameObject ctx, Args args)
        {
            var tween = Create<TMove>(ctx);
            return tween.FromExisting(args);
        }
        public Promise FromExisting(Args args)
        {
            var promise = FromExisting();
            this.args = args;
            return promise;
        }
        protected override void Begin()
        {
            startTime = currentTime;
        }
        private void OnDisable() 
        {
            if(args.destroyOnDone) Destroy(this);    
        }
        // Update is called once per frame
        void Update()
        {
            float t = (currentTime - startTime) / args.duration;
            transform.position = Vector3.Lerp(args.from, args.to, t);
            if(t > 1) Done();
        }
    }
}
