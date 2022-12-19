using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Base class for all game modes based on GameState implementations.
    /// </summary>
    public abstract class GameBase<T> : MonoBehaviour where T : GameState
    {
        protected T gameState;
        protected static readonly WaitForFixedUpdate waitForFixedUpdate = new();
        private Coroutine clockHandle;
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
            clockHandle = StartCoroutine(CoroTick(fixedUpdateToTick));
        }
        /// <summary>
        /// Stop the ticking of the game's timer.
        /// </summary>
        protected void StopTicking()
        {
            StopCoroutine(clockHandle);
        }
        private IEnumerator CoroTick(int fixedUpdateToTick)
        {
            if(fixedUpdateToTick <= 0) 
                throw new System.ArgumentException(
                    $"Argument {nameof(fixedUpdateToTick)} must be greater than 0.");
            while(true)
            {
                gameState.Tick();
                for(int i = 0; i < fixedUpdateToTick; ++i)
                    yield return waitForFixedUpdate;
            }
        }
    }
}
