using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.Modifiers
{
    public class PulledFollow : MonoBehaviour
    {
        public Vector3 pullSource;
        public float pullRadius;
        public float factor;
        public Vector3 followTarget;
        private void Awake() 
        {
            enabled = false;
        }
        void Update() 
        {
            Vector3 actualFollow = pullSource + Vector3.ClampMagnitude(followTarget - pullSource, pullRadius);
            Vector3 followMoveTo = Vector3.Lerp(transform.position, actualFollow, factor * Time.smoothDeltaTime);
            transform.position = followMoveTo;
        }
    }
}
