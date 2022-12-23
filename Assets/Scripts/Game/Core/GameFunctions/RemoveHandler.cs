using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using static System.TupleExtensions;

namespace SortGame.Core.GameFunctions
{
    /// <summary>
    /// An object that handles removal operations on the GameGridState.
    /// Implemented using <see cref="SelectionHandler"></see>.
    /// </summary>
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
        /// and whether they should be removed or not.</returns>
        public (int numberCount, int trashCount, bool shouldRemove) EndSelection(int minimumLength = 0)
        {
            var selection = selectionHandler.EndSelection();
            int numberCount = selection.Count;
            if(selection.Count >= minimumLength)
            {
                ExpandSelectionToIncludeAdjacentGarbage(selection);
                gameGridState.RemoveTiles(selection.ToArray());
                return (numberCount, selection.Count - numberCount, true);
            }
            else return (numberCount, 0, false);
        }
        public bool CanAddToSelection(Vector2Int coord) => selectionHandler.CanAddToSelection(coord);
        private void ExpandSelectionToIncludeAdjacentGarbage(List<Vector2Int> selection)
        {
            HashSet<Vector2Int> garbageTiles = new();
            foreach(var coord in selection)
                foreach(var adj in coord.AdjacentPoints())
                    if(gameGridState.IsOnGrid(adj) && gameGridState.IsTrash(adj))
                        garbageTiles.Add(adj);
            foreach(var garbageTile in garbageTiles)
                selection.Add(garbageTile);
        }
    }
}