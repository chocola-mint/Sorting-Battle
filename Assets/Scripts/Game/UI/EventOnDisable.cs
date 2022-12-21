using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class EventOnDisable : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent onDisable;
        private void OnDisable() 
        {
            onDisable.Invoke();    
        }
    }
}
