using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SortGame.UI
{
    /// <summary>
    /// Utility component that implements recursive "Return" behaviour in UI hierarchies.
    /// </summary>
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
