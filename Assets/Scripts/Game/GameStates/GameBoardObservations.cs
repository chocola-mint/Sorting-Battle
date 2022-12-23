using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SortGame.Core
{
    /// <summary>
    /// Static class for common observation data AI could use.
    /// </summary>
    public static class GameBoardObservations
    {
        /// <summary>
        /// Get all tiles in row-major order.
        /// This can be cached safely after the game starts.
        /// </summary>
        public static List<Vector2Int> GetAllTiles(this GameBoardState gameBoardState)
        {
            List<Vector2Int> result = new();
            for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
                for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
                result.Add(new(i, j));
            return result;
        }
        /// <summary>
        /// Get all selectable tiles in row-major order.
        /// This becomes invalid after each ingame tick, so you'll have to invoke this repeatedly.
        /// </summary>
        public static List<Vector2Int> GetAllSelectableTiles(this GameBoardState gameBoardState)
        {
            List<Vector2Int> result = new();
            for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
                for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
                {
                    Vector2Int coord = new(i, j);
                    if(gameBoardState.gameControllerState.CanAddToSelection(coord)) 
                        result.Add(coord);
                }
            return result;
        }
        /// <summary>
        /// Get all sorted lines. Horizontal lines come before vertical lines.
        /// This becomes invalid after each ingame tick, so you'll have to invoke this repeatedly.
        /// </summary>
        public static List<List<Vector2Int>> GetAllSortedLines(
            this GameBoardState gameBoardState, 
            int minimumSortedLength = 3,
            bool requireSelectable = true,
            bool tryMaximizeLength = true)
        {
            List<List<Vector2Int>> result = new();
            LinkedList<Vector2Int> currentSubsequence = new();
            System.Action registerSubsequenceIfValid = () => {
                if(currentSubsequence.Count < minimumSortedLength) return;
                result.Add(currentSubsequence.ToList());
            };
            System.Action<Vector2Int> onNextNumberInSequence = coord => {
                if(currentSubsequence.Count == 0)
                {
                    currentSubsequence.AddLast(coord);
                }
                else if(
                    // We require the coord to map to a number.
                    gameBoardState.gameGridState.IsNumber(coord)
                    // Check for monotone increasing here.
                    && (gameBoardState.gameGridState.Get(currentSubsequence.Last.Value) <= gameBoardState.gameGridState.Get(coord)) 
                    // If we require the coord to be selectable, do the check here.
                    && (!requireSelectable || gameBoardState.gameControllerState.CanAddToSelection(coord)))
                {
                    currentSubsequence.AddLast(coord);
                    if(!tryMaximizeLength)
                        registerSubsequenceIfValid();
                }
                else
                {
                    registerSubsequenceIfValid();
                    currentSubsequence.Clear();
                }
            };
            // Get sorted rows first.
            for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
            {
                for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
                    onNextNumberInSequence(new(i, j));
                registerSubsequenceIfValid(); // Retrieve the last valid subsequence.
                currentSubsequence.Clear();
            }
            // Then sorted columns.
            for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
            {
                for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
                    onNextNumberInSequence(new(i, j));
                registerSubsequenceIfValid(); // Retrieve the last valid subsequence.
                currentSubsequence.Clear();
            }
            return result;
        }
        public static List<Vector2Int> GetAllSwappableTiles(
            this GameBoardState gameBoardState)
        {
            List<Vector2Int> result = new();
            for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
                for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
                {
                    Vector2Int coord = new(i, j);
                    if(gameBoardState.gameControllerState.CanSwap(coord)) 
                        result.Add(coord);
                }
            return result;
        }
        public static int GetBoardHeight(this GameBoardState gameBoardState)
        {
            for(int i = 0; i < gameBoardState.gameGridState.rowCount; ++i)
                for(int j = 0; j < gameBoardState.gameGridState.columnCount; ++j)
                    if(!gameBoardState.gameGridState.IsEmpty(new(i, j)))
                        return gameBoardState.gameGridState.rowCount - i;
            return 0;
        }
    }
}
