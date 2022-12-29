using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.Core.GameFunctions;
using ChocoUtil.Algorithms;
namespace SortGame.Core
{
    /// <summary>
    /// A class that represents a game board, which contains a grid, a controller, a score value,
    /// and a pressure value.
    /// </summary>
    public class GameBoardState : IRandomThread
    {
        public struct Config
        {
            public bool useSeed;
            public int seed;
            public int rowCount, columnCount;
            public int numberUpperBound;
            public int minimumSortedLength;
            public int baseRemoveScore;
            public int removeLengthBonus;
            public int maxEffectiveCombo;
            public float comboScoreStep;
        }
        public readonly GameGridState gameGridState;
        public Random.State randomState { get; set; }
        public readonly GameControllerState gameControllerState;
        public readonly GameScoreState gameScoreState;
        public readonly GamePressureState gamePressureState;
        /// <summary>
        /// Callback event that's invoked when PushNewRow causes overflow and triggerEvent is set to true.
        /// </summary>
        public event System.Action onOverflow;
        public enum Status
        {
            Active, Inactive, Win, Lose
        }
        public Status status;
        public GameBoardState(Config config)
        {
            if(config.useSeed) IRandomThread.InitRandom(this, config.seed);
            else randomState = Random.state;
            gameGridState = new(config.rowCount, config.columnCount, config.numberUpperBound);
            gameControllerState = new(gameGridState, config.minimumSortedLength);
            gamePressureState = new();
            // Forward parameters to GameScoreState.
            gameScoreState = new(
                new(){
                    minimumRemoveCount = config.minimumSortedLength,
                    baseRemoveScore = config.baseRemoveScore,
                    removeLengthBonus = config.removeLengthBonus,
                    maxEffectiveCombo = config.maxEffectiveCombo,
                    comboScoreStep = config.comboScoreStep,
                }
            );
            // Tell GameScoreState whenever a remove event happens.
            gameControllerState.onRemove += gameScoreState.OnRemove;
        }
        // ! A GameBoardState can have multiple events that must be triggered manually.
        // ! The environment is supposed to invoke them appropriately according to the specification.
        public bool PushNewRow(int numberOfColumns, bool triggerEvent = false)
        {
            using(new RandomScope(this))
            {
                var columns = RandLib.RandomIntegerSequence(0, gameGridState.columnCount);
                bool anyOverflow = false;
                foreach(var column in columns[0..Mathf.Min(numberOfColumns, columns.Length)])
                {
                    anyOverflow |= gameGridState.PushUp(column);
                }
                if(anyOverflow && triggerEvent) onOverflow?.Invoke();
                return anyOverflow;
            }
        }
        public bool PushTrashRows(int numberOfRows, int numberOfColumns)
        {
            using(new RandomScope(this))
            {
                bool anyOverflow = false;
                for(int i = 0; i < numberOfRows; ++i)
                {
                    var columns = RandLib.RandomIntegerSequence(0, gameGridState.columnCount);
                    int columnLimit = i == 0 
                    ? Mathf.Min(numberOfColumns, columns.Length) 
                    : columns.Length;
                    foreach(var column in columns[0..columnLimit])
                    {
                        anyOverflow |= gameGridState.PushUp(column, GameTileState.Trash);
                    }
                }
                return anyOverflow;
            }
        }
        
    }
}
