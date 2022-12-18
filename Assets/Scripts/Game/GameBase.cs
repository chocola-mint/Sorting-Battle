using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public abstract class GameBase<T> : MonoBehaviour where T : GameState
    {
        protected T gameState;
        protected static readonly WaitForFixedUpdate waitForFixedUpdate = new();
        private Coroutine clockHandle;
        public abstract void StartGame();
        protected void StartTicking(int fixedUpdateToTick) 
        { 
            clockHandle = StartCoroutine(CoroTick(fixedUpdateToTick));
        }
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
