using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

namespace SortGame
{
    public class PressAnyButtonToStart : MonoBehaviour
    {
        [SerializeField] InputActionReference anyButton;
        public UnityEvent onStart;
        private void OnEnable() 
        {
            anyButton.EnableAndConnectOnPerform(OnAnyButton);
        }
        private void OnDisable() 
        {
            anyButton.DisableAndDisconnectOnPerform(OnAnyButton);    
        }
        private void OnAnyButton(InputAction.CallbackContext ctx)
        {
            onStart?.Invoke();
            enabled = false; // Disable here because you probably wouldn't reuse this.
        }
    }
}
