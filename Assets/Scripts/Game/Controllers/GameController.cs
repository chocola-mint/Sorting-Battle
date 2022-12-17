using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Base class for all controllers (agents).
    /// </summary>
    [RequireComponent(typeof(GameBoard))]
    [RequireComponent(typeof(GameGridSelector))]
    [RequireComponent(typeof(GameGridSwapper))]
    public abstract class GameController : MonoBehaviour
    {
        protected GameBoard gameBoard { get; private set; }
        protected GameGridSelector selector { get; private set; }
        protected GameGridSwapper swapper { get; private set; }
        private void Awake() 
        {
            selector = GetComponent<GameGridSelector>();
            swapper = GetComponent<GameGridSwapper>();
            gameBoard = GetComponent<GameBoard>();
        }
        public static void DisableAll()
        {
            foreach(var controller in FindObjectsOfType<GameController>())
                controller.enabled = false;
        }

    }
}
