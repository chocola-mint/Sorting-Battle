using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    /// <summary>
    /// Controller base class for AI controllers.
    /// </summary>
    public abstract class AIController : GameController
    {
        [SerializeField] private int decisionPeriod = 10;
        private static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        private Vector2Int cursor;
        protected IEnumerator WaitForTicks(int duration)
        {
            for(int i = 0; i < duration; ++i)
                yield return waitForFixedUpdate;
        }
        protected override void Init()
        {
            base.Init();
            StartCoroutine(AICoreLoop());
        }
        private IEnumerator AICoreLoop()
        {
            while(gameBoard.state.status == GameBoardState.Status.Active)
            {
                var waitConstraintAfterAction = OnAction();
                if(waitConstraintAfterAction != null) 
                    yield return waitConstraintAfterAction;
                yield return WaitForTicks(decisionPeriod);
            }
        }
        protected abstract IEnumerator OnAction();
        /// <summary>
        /// Move the AI's virtual cursor on the grid.
        /// </summary>
        /// <returns>True if the cursor would have to be released to move to the destination.</returns>
        private bool MoveCursor(Vector2Int to)
        {
            bool cursorReleased = LinAlg.L1Norm(cursor, to) > 1;
            cursor = to;
            return cursorReleased;
        }
        /// <summary>
        /// Let the AI select a tile. Pressing/releasing the mouse buttons is simulated accordingly.
        /// </summary>
        protected void Select(Vector2Int target)
        {
            bool cursorReleased = MoveCursor(target);
            if(cursorReleased) selector.EndSelection();
            if(!selector.enabled) selector.BeginSelection();
            selector.Select(cursor);
        }
        /// <summary>
        /// Let the AI swap on a tile. Pressing/releasing the mouse buttons is simulated accordingly.
        /// </summary>
        protected void Swap(Vector2Int target)
        {
            bool cursorReleased = MoveCursor(target);
            if(!swapper.enabled) swapper.StartSwapping(cursor);
            else 
            {
                if(cursorReleased) swapper.EndSwapping();
                swapper.SwapTo(cursor);
            }

        }

    }
}
