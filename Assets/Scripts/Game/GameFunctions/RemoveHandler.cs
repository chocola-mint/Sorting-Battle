using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static System.TupleExtensions;

namespace SortGame.GameFunctions
{
    public class RemoveHandler
    {
        private readonly GameGridState gameGridState;
        private readonly SelectionHandler selectionHandler;
        public int GetCurrentSelectionCount() => selectionHandler.GetCurrentSelectionCount();
        public RemoveHandler(GameGridState gameGridState, SelectionHandler selectionHandler)
        {
            this.selectionHandler = selectionHandler;
            this.gameGridState = gameGridState;
        }
        public RemoveHandler(GameGridState gameGridState)
        {
            this.selectionHandler = new(gameGridState);
            this.gameGridState = gameGridState;
        }
        public void BeginSelection() => selectionHandler.BeginSelection();
        public bool Select(Vector2Int tileCoord) => selectionHandler.Select(tileCoord);
        /// <summary>
        /// Try to remove the currently-selected tiles.
        /// </summary>
        /// <returns>A tuple, containing the removed blocks 
        /// and the SwapOps that come from dropping the blocks above.</returns>
        public (List<Vector2Int>, bool shouldRemove) EndSelection(int minimumLength = 0)
        {
            var selection = selectionHandler.EndSelection();
            if(selection.Count >= minimumLength)
            {
                ExpandSelectionToIncludeAdjacentGarbage(selection);
                gameGridState.RemoveTiles(selection.ToArray());
                return (selection, true);
            }
            else return (selection, false);
        }
        private void ExpandSelectionToIncludeAdjacentGarbage(List<Vector2Int> selection)
        {
            HashSet<Vector2Int> garbageTiles = new();
            foreach(var coord in selection)
                foreach(var adj in coord.AdjacentPoints())
                    if(gameGridState.IsOnGrid(adj) && gameGridState.IsGarbage(adj))
                        garbageTiles.Add(adj);
            foreach(var garbageTile in garbageTiles)
                selection.Add(garbageTile);
        }
    }
}