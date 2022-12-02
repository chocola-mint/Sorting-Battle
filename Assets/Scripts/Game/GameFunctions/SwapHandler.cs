using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SortGame.GameFunctions
{
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
            if(gameGridState.IsEmpty(target)) return false;
            Debug.Log("=== Begin swapping ===");
            cursor = target;
            return true;
        }
        private bool AdjacentToCursor(Vector2Int target)
            => LinAlg.L1Norm(cursor, target) == 1;
        public void SwapTo(Vector2Int target)
        {
            bool success = cursor != Null && AdjacentToCursor(target) && gameGridState.IsNumber(target);
            if(success)
            {
                gameGridState.SwapAndPullDown(cursor, target);
                cursor = target;
            }
        }
        public void EndSwapping()
        {
            Debug.Log("=== End swapping ===");
            cursor = Null;
        }
    }
}
