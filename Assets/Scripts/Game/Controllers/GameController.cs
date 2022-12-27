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
        protected GameBoardPusher pusher { get; private set; }
        protected event System.Action onEnable, onDisable;
        private bool initInvoked = false;
        private void OnEnable() 
        {
            if(!initInvoked) Init();
            onEnable?.Invoke();
        }
        private void OnDisable() 
        {
            onDisable?.Invoke();
        }
        private void Start() 
        {
            if(!initInvoked) Init();
        }        
        protected virtual void Init() 
        {
            initInvoked = true;
            selector = gameBoard.GetComponent<GameGridSelector>();
            swapper = gameBoard.GetComponent<GameGridSwapper>();
            pusher = gameBoard.GetComponent<GameBoardPusher>();
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
