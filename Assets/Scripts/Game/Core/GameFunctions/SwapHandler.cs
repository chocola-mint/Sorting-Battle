using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace SortGame.Core.GameFunctions
{
    /// <summary>
    /// An object that handles swapping operations on the GameGridState.
    /// </summary>
    public class SwapHandler
    {
        private readonly GameGridState gameGridState;
        public SwapHandler(GameGridState gameGridState)
        {
            this.gameGridState = gameGridState;
        }
        public static readonly Vector2Int Null = new(-10, -10);
        public Vector2Int cursor { get; private set; } = Null;
        public bool swappingActive => cursor != Null;
        public bool StartSwapping(Vector2Int target)
        {
            if(!CanStartSwapping(target)) return false;
            // Debug.Log("=== Begin swapping ===");
            cursor = target;
            return true;
        }
        public bool CanStartSwapping(Vector2Int target)
            => gameGridState.IsNumber(target);
        public bool CanSwapTo(Vector2Int target)
            => cursor != Null && AdjacentToCursor(target) && gameGridState.IsNumber(target);
        public bool CanSwap(Vector2Int target)
        {
            if(!swappingActive) return CanStartSwapping(target);
            else return CanSwapTo(target);
        }
        private bool AdjacentToCursor(Vector2Int target)
            => LinAlg.L1Norm(cursor, target) == 1;
        public void SwapTo(Vector2Int target)
        {
            bool success = CanSwapTo(target);
            if(success)
            {
                gameGridState.SwapAndPullDown(cursor, target);
                cursor = target;
            }
        }
        public void EndSwapping()
        {
            // Debug.Log("=== End swapping ===");
            cursor = Null;
        }
    }
}
