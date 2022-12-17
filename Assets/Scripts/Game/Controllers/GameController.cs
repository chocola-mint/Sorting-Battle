using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Base class for all controllers (agents).
    /// </summary>
    public abstract class GameController : MonoBehaviour
    {
        public GameBoard gameBoardComponent;
        protected GameBoard gameBoard => gameBoardComponent;
        protected GameGridSelector selector { get; private set; }
        protected GameGridSwapper swapper { get; private set; }
        private void Start() 
        {
            selector = gameBoard.GetComponent<GameGridSelector>();
            swapper = gameBoard.GetComponent<GameGridSwapper>();
            Init();
        }        
        protected virtual void Init() {}
        public static void DisableAll()
        {
            foreach(var controller in FindObjectsOfType<GameController>())
                controller.enabled = false;
        }

    }
}
