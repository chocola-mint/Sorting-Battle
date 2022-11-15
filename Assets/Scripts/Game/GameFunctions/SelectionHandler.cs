using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace SortGame.GameFunctions
{
    public class SelectionHandler
    {
        private readonly GameGridState gameGridState;
        private List<Vector2Int> currentSelection = new();
        public SelectionHandler(GameGridState gameGridState)
        {
            this.gameGridState = gameGridState;
        }
        private bool SelectionOnSameRowAs(Vector2Int target)
        {
            foreach(var coord in currentSelection)
                if(coord.x != target.x) return false;
            return true;
        }
        private bool SelectionOnSameColumnAs(Vector2Int target)
        {
            foreach(var coord in currentSelection)
                if(coord.y != target.y) return false;
            return true;
        }
        private List<int> GetCurrentSelectionNumbers()
        {
            List<int> numbers = new();
            foreach(var coord in currentSelection)
                numbers.Add(gameGridState.Get(coord));
            return numbers;
        }
        private bool SelectionIsSortedWithRespectTo(Vector2Int target)
        {
            if(currentSelection.Count == 0) return true;
            List<int> numbers = GetCurrentSelectionNumbers();
            numbers.Add(gameGridState.Get(target));
            return numbers.IsMonotonic();
        }
        private bool LastSelectionIsAdjacentTo(Vector2Int target)
        {
            return currentSelection.Count == 0 
            || LinAlg.L1Norm(target, currentSelection[^1]) <= 1;
        }
        private bool CanAddToSelection(Vector2Int target)
        {
            if(currentSelection.Contains(target)
            || !LastSelectionIsAdjacentTo(target)
            || !SelectionIsSortedWithRespectTo(target))
            {
                return false;
            }
            return SelectionOnSameRowAs(target)
            || SelectionOnSameColumnAs(target);
        }
        private bool TryAddToSelection(Vector2Int target)
        {
            bool canAdd = CanAddToSelection(target);
            if(canAdd) currentSelection.Add(target);
            return canAdd;
        }
        public void Select(Vector2Int tileCoord)
        {
            if(TryAddToSelection(tileCoord))
            {
                Debug.Log($"Added {gameGridState.Get(tileCoord)}");
            }
        }
        public void BeginSelection()
        {
            Debug.Log("=== Begin selection ===");
            currentSelection.Clear();
        }
        public void EndSelection()
        {
            Debug.Log("=== End selection ===");
            currentSelection.Clear();
        }
    }

}