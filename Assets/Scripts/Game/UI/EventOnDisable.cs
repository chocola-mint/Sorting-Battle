using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame.UI
{
    /// <summary>
    /// Utility class that fires an event when the object is disabled.
    /// </summary>
    public class EventOnDisable : MonoBehaviour
    {
        public UnityEngine.Events.UnityEvent onDisable;
        private void OnDisable() 
        {
            onDisable.Invoke();    
        }
    }
}
