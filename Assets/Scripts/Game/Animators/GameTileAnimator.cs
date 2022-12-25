using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    [RequireComponent(typeof(Animator))]
    public class GameTileAnimator : MonoBehaviour, IOnSelectReceiver, IOnDeselectReceiver
    {
        private Animator animator;
        private static class Params
        {
            public static int BeginHighlight = Animator.StringToHash(nameof(BeginHighlight));
            public static int EndHighlight = Animator.StringToHash(nameof(EndHighlight));
        }
        private void Awake() {
            animator = GetComponent<Animator>();
        }
        
        public void OnSelect()
        {
            // Debug.Log($"Set trigger {nameof(Params.BeginHighlight)}");
            animator.SetTrigger(Params.BeginHighlight);
        }

        public void OnDeselect()
        {
            animator.SetTrigger(Params.EndHighlight);
        }
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
