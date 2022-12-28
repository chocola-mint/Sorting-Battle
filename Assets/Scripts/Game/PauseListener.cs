using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace SortGame
{
    public class PauseListener : MonoBehaviour
    {
        [SerializeField] private PauseManager pauseManager;
        [SerializeField] private InputActionReference pauseAction;
        private void OnPauseAction(InputAction.CallbackContext ctx)
        {
            if(pauseManager.isPaused) 
            {
                Debug.Log("Unpause");
                pauseManager.Unpause();
            }
            else 
            {
                Debug.Log("Pause");
                pauseManager.Pause();
            }
        }
        private void OnEnable() 
        {
            pauseAction.EnableAndConnectOnPerform(OnPauseAction);
        }
        private void OnDisable() 
        {
            pauseAction.DisableAndDisconnectOnPerform(OnPauseAction);
        }
    }
}
