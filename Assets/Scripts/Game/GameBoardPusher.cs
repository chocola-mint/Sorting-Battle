using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace SortGame
{
    [RequireComponent(typeof(GameBoard))]
    public class GameBoardPusher : MonoBehaviour
    {
        [SerializeField] private Button pushButton;
        private Graphic[] buttonGraphics;
        private GameBoard gameBoard;
        private void Awake() 
        {
            gameBoard = GetComponent<GameBoard>();
            buttonGraphics = pushButton.GetComponentsInChildren<Graphic>();
            enabled = false;
        }
        void OnEnable()
        {
            pushButton.onClick.AddListener(Push);
            foreach(var graphic in buttonGraphics)
                graphic.raycastTarget = true;
        }
        private void OnDisable() 
        {
            pushButton.onClick.RemoveListener(Push);
            foreach(var graphic in buttonGraphics)
                graphic.raycastTarget = false;
        }
        public void Push()
        {
            gameBoard.state.PushNewRow(gameBoard.state.gameGridState.columnCount - 1, triggerEvent: true);
        }
    }
}
