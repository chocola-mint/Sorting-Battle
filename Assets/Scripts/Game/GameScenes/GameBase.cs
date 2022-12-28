using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using SortGame.Core;

namespace SortGame
{
    /// <summary>
    /// Base class for all game scenes based on GameState implementations.
    /// Game scenes represent GameStates in Unity.
    /// </summary>
    [RequireComponent(typeof(GameEventDelegate))]
    public abstract class GameBase<T> : MonoBehaviour where T : GameState
    {
        [SerializeField] private PauseManager pauseManager;
        protected T gameState;
        private int fixedUpdatesUntilTick = 0;
        private int fixedUpdateToTick = -1;
        /// <summary>
        /// Invoke to start the game.
        /// </summary>
        public abstract void StartGame();
        /// <summary>
        /// Invoke to start the game's timer, controlled by FixedUpdates.
        /// </summary>
        /// <param name="fixedUpdateToTick">FixedUpdates per tick.</param>
        protected void StartTicking(int fixedUpdateToTick) 
        { 
            this.fixedUpdateToTick = fixedUpdateToTick;
        }
        /// <summary>
        /// Stop the ticking of the game's timer.
        /// </summary>
        protected void StopTicking()
        {
            fixedUpdateToTick = -1;
        }
        private void OnEnable() 
        {
            if(pauseManager)
            {
                pauseManager.onPause += OnPause;
                pauseManager.onUnpause += OnUnpause;
            }
        }
        private void OnDisable()
        {
            if(pauseManager)
            {
                pauseManager.onPause -= OnPause;
                pauseManager.onUnpause -= OnUnpause;
            }
        }
        private void OnPause() 
        {
            Time.timeScale = 0;
            GameController.DisableAll();
        }
        private void OnUnpause() 
        {
            Time.timeScale = 1;
            GameController.EnableAll();
        }
        private void FixedUpdate() 
        {
            if(fixedUpdateToTick < 0 
            || (pauseManager != null && pauseManager.isPaused)) return;
            if(fixedUpdatesUntilTick == 0)
            {
                gameState.Tick();
                fixedUpdatesUntilTick = fixedUpdateToTick;
            }
            else {
                --fixedUpdatesUntilTick;
            }
        }
    }
}
