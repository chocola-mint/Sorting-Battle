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
        public event System.Action<int> onRemove;
        private readonly HashSet<int> columnLocks = new();
        public GameControllerState(GameGridState gameGridState, int minimumSortedLength)
        {
            this.gameGridState = gameGridState;
            this.minimumSortedLength = minimumSortedLength;
            remover = new(gameGridState);
            swapper = new(gameGridState);
        }
        public bool StartSwapping(Vector2Int target)
        {
            columnLocks.Add(target.y);
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
            swapper.EndSwapping();
        }
        public void BeginSelection()
        {
            remover.BeginSelection();
        }
        public bool Select(Vector2Int target)
        {
            columnLocks.Add(target.y);
            return remover.Select(target);
        }
        public void EndSelection()
        {
            columnLocks.Clear();
            var (numberCount, trashCount, shouldRemove) = remover.EndSelection(minimumSortedLength);
            if(shouldRemove) onRemove?.Invoke(numberCount);
            else onRemove?.Invoke(0);
        }
    }
}
