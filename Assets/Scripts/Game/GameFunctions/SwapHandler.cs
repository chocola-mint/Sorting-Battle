using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
namespace SortGame.GameFunctions
{
    using SwapOp = GameGridState.SwapOp;
    public class SwapHandler
    {
        public struct Commands
        {
            public bool success;
            public bool failed => !success;
            public SwapOp swap;
            public List<SwapOp> drops;
            public bool shouldFall => drops.Count > 0;
        }
        private readonly GameGridState gameGridState;
        public SwapHandler(GameGridState gameGridState)
        {
            this.gameGridState = gameGridState;
        }
        private static readonly Vector2Int Null = new(-10, -10);
        private Vector2Int cursor = Null;
        public bool swappingActive => cursor != Null;
        public bool StartSwapping(Vector2Int select)
        {
            if(gameGridState.IsEmpty(select)) return false;
            Debug.Log("=== Begin swapping ===");
            cursor = select;
            return true;
        }
        private bool AdjacentToCursor(Vector2Int target)
            => LinAlg.L1Norm(cursor, target) <= 1;
        public Commands SwapTo(Vector2Int target)
        {
            Debug.Assert(cursor != Null);
            Commands cmds = new();
            cmds.success = AdjacentToCursor(target);
            if(cmds.success)
            {
                cmds.swap = new(){a = cursor, b = target};
                cmds.drops = gameGridState.SwapAndPullDown(cursor, target);
                cursor = target;
                if(cmds.drops.Any(x => x.a == target)) EndSwapping();
            }
            return cmds;
        }
        public void EndSwapping()
        {
            Debug.Log("=== End swapping ===");
            cursor = Null;
        }
    }
}
