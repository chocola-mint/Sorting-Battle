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
        private static readonly WaitForFixedUpdate waitForFixedUpdate = new WaitForFixedUpdate();
        // The "virtual" cursor used by the AI. Used to simplify decision processes, 
        // removing intricacies surrounding the player's mouse.
        private Vector2Int cursor;
        /// <summary>
        /// Utility method.
        /// </summary>
        protected IEnumerator WaitForTicks(int duration)
        {
            for(int i = 0; i < duration; ++i)
                yield return waitForFixedUpdate;
        }
        protected override void Init()
        {
            base.Init();
            // We have to declare the core loop as a separate coroutine rather than just using
            // IEnumerator Start(), to avoid name collision.
            StartCoroutine(AICoreLoop());
        }
        /// <summary>
        /// The core loop of an AI, which just takes an action and waits for awhile.
        /// </summary>
        private IEnumerator AICoreLoop()
        {
            while(gameBoard.state.status == GameBoardState.Status.Active)
            {
                var waitConstraintAfterAction = OnAction();
                yield return waitConstraintAfterAction;
            }
        }
        private void OnDisable() 
        {
            // Much like PlayerController, we have to manually disconnect controller input here.
            selector.EndSelection();
            swapper.EndSwapping();
            StopAllCoroutines();
        }
        /// <summary>
        /// Abstract method (coroutine) that should implement an AI's decision process and execution.
        /// </summary>
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
        /// <br></br>
        /// To simplify things, the AI lets go of the cursor after each swap.
        /// </summary>
        protected void Swap(Vector2Int target)
        {
            swapper.StartSwapping(cursor);
            bool cursorReleased = MoveCursor(target);
            if(!swapper.enabled) 
            {
                swapper.StartSwapping(cursor);
            }
            else 
            {
                swapper.SwapTo(cursor);
                swapper.EndSwapping();
            }
        }
        /// <summary>
        /// Let the AI push a new row.
        /// </summary>
        protected void Push()
        {
            gameBoard.state.PushNewRow(gameBoard.state.gameGridState.columnCount - 1);
        }

    }
}