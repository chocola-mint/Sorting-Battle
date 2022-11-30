using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;
namespace SortGame
{
    public class GameBoardState
    {
        public struct Config
        {
            public int seed;
            public int rowCount, columnCount;
            public int minimumSortedLength;
            public int baseRemoveScore;
            public int removeLengthBonus;
            public int maxEffectiveCombo;
            public float comboScoreStep;
        }
        public readonly GameGridState gameGridState;
        public Random.State randomState;
        public readonly GameControllerState gameControllerState;
        public readonly GameScoreState gameScoreState;
        public GameBoardState(Config config)
        {
            Random.InitState(config.seed);
            randomState = Random.state;
            gameGridState = new(config.rowCount, config.columnCount);
            gameControllerState = new(gameGridState, config.minimumSortedLength);
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
    }
}
