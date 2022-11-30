using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;

namespace SortGame
{
    public class GameControllerState
    {
        private readonly GameGridState gameGridState;
        public readonly int minimumSortedLength;
        private readonly RemoveHandler remover;
        private readonly SwapHandler swapper;
        /// <summary>
        /// Whether the simulated game environment is paused or not.
        /// While paused, write operations are locked (like swaps), but read operations
        /// (like selects) are not.
        /// See <see cref="Block"></see> and <see cref="Unblock"></see>.
        /// </summary>
        public bool writeLocked { get; private set; }
        public GameControllerState(GameGridState gameGridState, int minimumSortedLength)
        {
            this.gameGridState = gameGridState;
            this.minimumSortedLength = minimumSortedLength;
            remover = new(gameGridState);
            swapper = new(gameGridState);
        }
        /// <summary>
        /// Pauses the simulated game environment, ignoring most inputs.
        /// This can be used to process "view" operations, like playing animations.
        /// </summary>
        public void Block()
        {
            writeLocked = true;
        }
        /// <summary>
        /// Unpauses the simulated game environment. See <see cref="Block"></see>.
        /// </summary>
        public void Unblock()
        {
            writeLocked = false;
        }
        public bool StartSwapping(Vector2Int target)
        {
            if(writeLocked)
            {
                Debug.LogWarning("Attempting swap while write locked");
                return false;
            }
            return swapper.StartSwapping(target);
        }
        public SwapHandler.Commands SwapTo(Vector2Int target)
        {
            if(writeLocked)
            {
                Debug.LogWarning("Attempting swap while write locked");
                return default;
            }
            return swapper.SwapTo(target);
        }
        public void EndSwapping()
        {
            swapper.EndSwapping();
        }
        public void BeginSelection()
        {
            remover.BeginSelection();
        }
        public bool Select(Vector2Int target)
        {
            return remover.Select(target);
        }
        public (List<Vector2Int>, List<GameGridState.SwapOp>, bool) EndSelection()
        {
            bool shouldRemove = remover.GetCurrentSelectionCount() >= minimumSortedLength;
            if(shouldRemove && writeLocked)
            {
                Debug.LogWarning("Attempting removal while write locked");
                var (_selection, _drops) = remover.EndSelection(int.MaxValue);
                return (_selection, _drops, false);
            }
            
            var (selection, drops) = remover.EndSelection(minimumSortedLength);
            return (selection, drops, shouldRemove);
        }
    }
}
