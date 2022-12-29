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
            public InputActionReference push;
        }
        [SerializeField] private Inputs inputs;
        private void Awake() 
        {
            onEnable += () => {
                if(pusher) pusher.enabled = true;
                inputs.select.EnableAndConnect(OnSelect);
                inputs.swap.EnableAndConnect(OnSwap);
                inputs.push.EnableAndConnect(OnPush);
            };
            onDisable += () => {
                if(pusher) pusher.enabled = false;
                inputs.select.DisableAndDisconnect(OnSelect);
                inputs.swap.DisableAndDisconnect(OnSwap);
                inputs.push.DisableAndDisconnect(OnPush);
            };
        }
        protected override void Init()
        {
            base.Init();
        }
        private void OnSelect(InputAction.CallbackContext ctx)
        {
            if(!selector) return;

            if(ctx.started) selector.BeginSelection();
            else if(ctx.canceled) selector.EndSelection();
            selector.Select(ctx.ReadValue<Vector2>());
        }
        private void OnSwap(InputAction.CallbackContext ctx)
        {
            if(!swapper) return;

            if(ctx.started) swapper.StartSwapping(ctx.ReadValue<Vector2>());
            else if(ctx.canceled) swapper.EndSwapping();
            else swapper.SwapTo(ctx.ReadValue<Vector2>());
        }
        private void OnPush(InputAction.CallbackContext ctx)
        {
            if(!pusher) return;

            if(ctx.performed) 
                pusher.Push();
        }
    }

}