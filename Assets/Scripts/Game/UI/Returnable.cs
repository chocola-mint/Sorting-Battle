using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SortGame
{
    public class Returnable : MonoBehaviour
    {
        public UnityEvent onReturn;
        private System.Lazy<Returnable[]> childReturnables;
        private void Awake() 
        {
            childReturnables = new(GetComponentsInChildren<Returnable>, false);
        }
        public void Return() {
            onReturn.Invoke();
            foreach(var returnable in childReturnables.Value)
                if(returnable != this)
                    returnable.Return();
        }
    }
}
