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
        public GameControllerState(GameGridState gameGridState, int minimumSortedLength)
        {
            this.gameGridState = gameGridState;
            this.minimumSortedLength = minimumSortedLength;
            remover = new(gameGridState);
            swapper = new(gameGridState);
        }
        public bool StartSwapping(Vector2Int target)
        {
            return swapper.StartSwapping(target);
        }
        public SwapHandler.Commands SwapTo(Vector2Int target)
        {
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
            var (selection, swaps, shouldRemove) = remover.EndSelection(minimumSortedLength);
            if(shouldRemove) onRemove?.Invoke(selection.Count);
            else onRemove?.Invoke(0);
            return (selection, swaps, shouldRemove);
        }
    }
}
