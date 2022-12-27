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
        private System.Lazy<GameBoardPusher> pusherCache;
        protected GameGridSelector selector { get; private set; }
        protected GameGridSwapper swapper { get; private set; }
        protected GameBoardPusher pusher => pusherCache.Value;
        private void Awake() 
        {
            pusherCache = new(() => gameBoard.GetComponent<GameBoardPusher>(), false);
        }
        private void Start() 
        {
            Init();
        }        
        protected virtual void Init() 
        {
            selector = gameBoard.GetComponent<GameGridSelector>();
            swapper = gameBoard.GetComponent<GameGridSwapper>();
        }
        public static void DisableAll()
        {
            foreach(var controller in FindObjectsOfType<GameController>())
                controller.enabled = false;
        }
        public static void EnableAll()
        {
            foreach(var controller in FindObjectsOfType<GameController>())
                controller.enabled = true;
        }
    }
}
