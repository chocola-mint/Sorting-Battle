using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace SortGame.UI
{
    [RequireComponent(typeof(TMP_Text))]
    public class EventOnTextAlpha : MonoBehaviour
    {
        public enum Operator
        {
            Greater,
            GreaterOrEqual,
            Less,
            LessOrEqual,
        }
        private bool Comparison(Operator op, float lhs, float rhs)
        {
            switch(op)
            {
                case Operator.Greater:
                    return lhs > rhs;
                case Operator.GreaterOrEqual:
                    return lhs >= rhs;
                case Operator.Less:
                    return lhs < rhs;
                case Operator.LessOrEqual:
                default:
                    return lhs <= rhs;
            }
        }
        [SerializeField] [Range(0, 1)] private float compareAlpha = 0.5f;
        [SerializeField] private Operator opType;
        private TMP_Text textMesh;
        public UnityEvent onCompareTrue;
        private void Awake() 
        {
            textMesh = GetComponent<TMP_Text>();    
        }
        void LateUpdate()
        {
            if(Comparison(opType, textMesh.alpha, compareAlpha))
            {
                onCompareTrue.Invoke();
                Destroy(this);
            }
        }
    }
}
