using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace SortGame
{
    public class PlayerController : GameController
    {
        [System.Serializable]
        private struct Inputs
        {
            public InputActionReference select;
            public InputActionReference swap;
        }
        [SerializeField] private Inputs inputs;        
        private void OnEnable() 
        {
            inputs.select.EnableAndConnect(OnSelect);
            inputs.swap.EnableAndConnect(OnSwap);
        }
        private void OnDisable() 
        {
            inputs.select.DisableAndDisconnect(OnSelect);
            inputs.swap.DisableAndDisconnect(OnSwap);
        }
        private void OnSelect(InputAction.CallbackContext ctx)
        {
            if(ctx.started) selector.BeginSelection();
            else if(ctx.canceled) selector.EndSelection();
            selector.Select(ctx.ReadValue<Vector2>());
        }
        private void OnSwap(InputAction.CallbackContext ctx)
        {
            if(ctx.started) swapper.StartSwapping(ctx.ReadValue<Vector2>());
            else if(ctx.canceled) swapper.EndSwapping();
            else swapper.SwapTo(ctx.ReadValue<Vector2>());
        }
    }

}