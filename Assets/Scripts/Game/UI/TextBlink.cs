using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SortGame.UI
{
    /// <summary>
    /// Class that implements adjustable blinking and vanishing behaviour on TMP text meshes.
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    public class TextBlink : MonoBehaviour
    {
        [SerializeField]
        private AnimationCurve blinkCurve, vanishCurve;
        [SerializeField][Min(0.01f)]
        private float blinkPeriod = 1.0f;
        private float blinkStart = 0;
        [SerializeField, Min(0)]
        private float vanishDuration = 1.0f;
        private float vanishStart = 0;
        private bool isVanishing = false;
        [SerializeField]
        private bool isBlinking = false;
        private TMP_Text textMesh;
        void Awake() {
            textMesh = GetComponent<TMP_Text>();
            textMesh.alpha = 0;
        }
        private void OnEnable() 
        {
            isVanishing = false;
            if(isBlinking) blinkStart = Time.time;    
        }
        void Update() 
        {
            if(isVanishing)
            {
                float progress = (Time.time - vanishStart) / vanishDuration;
                textMesh.alpha = vanishCurve.Evaluate(progress);
                if(progress >= 1) enabled = false;
            }
            else if(isBlinking)
            {
                textMesh.alpha = blinkCurve.Evaluate((Time.time - blinkStart) % blinkPeriod);
            }
        }
        public void StartBlinking()
        {
            isBlinking = true;
            blinkStart = Time.time;
        }
        public void StartVanishing()
        {
            isVanishing = true;
            vanishStart = Time.time;
        }
    }
}
