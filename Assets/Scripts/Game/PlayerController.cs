using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
namespace SortGame
{

    [RequireComponent(typeof(GameBoard))]
    public class PlayerController : MonoBehaviour
    {
        private GameBoard gameBoard;
        [System.Serializable]
        private struct Inputs
        {
            public InputActionReference select;
        }
        [SerializeField] private Inputs inputs;

        // Start is called before the first frame update
        void Start()
        {
            gameBoard = GetComponent<GameBoard>();
        }
        private void OnEnable() 
        {
            inputs.select.EnableAndConnect(OnPointerDrag);
        }
        private void OnDisable() 
        {
            inputs.select.DisableAndDisconnect(OnPointerDrag);
        }
        private void OnPointerDrag(InputAction.CallbackContext ctx)
        {
            if(ctx.started) gameBoard.BeginSelection();
            else if(ctx.canceled) gameBoard.EndSelection();
            gameBoard.Select(ctx.ReadValue<Vector2>());
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

}