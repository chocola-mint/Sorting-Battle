using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace SortGame
{
    /// <summary>
    /// Extension methods containing common Input System boilerplates.
    /// </summary>
    public static class InputSystemExtensions
    {
        public static void EnableAndConnect(
            this InputActionReference inputActionReference, 
            System.Action<InputAction.CallbackContext> callback)
        {
            inputActionReference.action.Enable();
            inputActionReference.action.started += callback;
            inputActionReference.action.performed += callback;
            inputActionReference.action.canceled += callback;
        }
        public static void DisableAndDisconnect(
            this InputActionReference inputActionReference, 
            System.Action<InputAction.CallbackContext> callback)
        {
            inputActionReference.action.Disable();
            inputActionReference.action.started -= callback;
            inputActionReference.action.performed -= callback;
            inputActionReference.action.canceled -= callback;
        }
        public static void EnableAndConnectOnPerform(
            this InputActionReference inputActionReference, 
            System.Action<InputAction.CallbackContext> callback)
        {
            inputActionReference.action.Enable();
            inputActionReference.action.performed += callback;
        }
        public static void DisableAndDisconnectOnPerform(
            this InputActionReference inputActionReference, 
            System.Action<InputAction.CallbackContext> callback)
        {
            inputActionReference.action.Disable();
            inputActionReference.action.performed -= callback;
        }

    }

}
