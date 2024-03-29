using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.Core.GameFunctions;

namespace SortGame.Core
{
    /// <summary>
    /// A class that represents a player's controller's states, and what a player
    /// is allowed to do.
    /// </summary>
    public class GameControllerState
    {
        private readonly GameGridState gameGridState;
        public readonly int minimumSortedLength;
        private readonly RemoveHandler remover;
        private readonly SwapHandler swapper;
        public event System.Action<int> onRemove;
        public event System.Action onStartSwapping, onEndSwapping, onBeginSelection, onEndSelection;
        private readonly HashSet<int> columnLocks = new();
        public GameControllerState(GameGridState gameGridState, int minimumSortedLength = 3, bool useInterrupts = true)
        {
            this.gameGridState = gameGridState;
            this.minimumSortedLength = minimumSortedLength;
            remover = new(gameGridState);
            swapper = new(gameGridState);
            // ! If useInterrupts is set to true, make swapping and selecting interrupt each other.
            if(useInterrupts)
            {
                onStartSwapping += EndSelection;
                onBeginSelection += EndSwapping;
            }
        }
        public bool StartSwapping(Vector2Int target)
        {
            columnLocks.Add(target.y);
            onStartSwapping?.Invoke();
            return swapper.StartSwapping(target);
        }
        public void SwapTo(Vector2Int target)
        {
            columnLocks.Remove(swapper.cursor.y);
            columnLocks.Add(target.y);
            swapper.SwapTo(target);
        }
        public void EndSwapping()
        {
            columnLocks.Clear();
            onEndSwapping?.Invoke();
            swapper.EndSwapping();
        }
        public void BeginSelection()
        {
            onBeginSelection?.Invoke();
            remover.BeginSelection();
        }
        public bool Select(Vector2Int target)
        {
            columnLocks.Add(target.y);
            return remover.Select(target);
        }
        public bool CanAddToSelection(Vector2Int target) => remover.CanAddToSelection(target);
        public bool CanSwap(Vector2Int target) => swapper.CanSwap(target);
        public void EndSelection()
        {
            columnLocks.Clear();
            onEndSelection?.Invoke();
            var (numberCount, trashCount, shouldRemove) = remover.EndSelection(minimumSortedLength);
            if(shouldRemove) onRemove?.Invoke(numberCount);
            else onRemove?.Invoke(0);
        }
    }
}
