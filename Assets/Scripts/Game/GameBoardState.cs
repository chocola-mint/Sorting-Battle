using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;
namespace SortGame
{
    public class GameBoardState
    {
        public record Config
        {
            public int seed;
            public int rowCount, columnCount;
            public int minimumSortedLength = 3;
        }
        public int score { get; private set; } = 0;
        public readonly GameGridState gameGridState;
        public Random.State randomState;
        public readonly GameControllerState gameControllerState;
        /// <summary>
        /// Whether the simulated game environment is paused or not.
        /// While paused, write operations are locked (like swaps), but read operations
        /// (like selects) are not.
        /// See <see cref="Block"></see> and <see cref="Unblock"></see>.
        /// </summary>
        public bool writeLocked { get; private set; }
        public GameBoardState(Config config)
        {
            Random.InitState(config.seed);
            randomState = Random.state;
            gameGridState = new(config.rowCount, config.columnCount);
            gameControllerState = new(gameGridState, config.minimumSortedLength);
        }
    }
}
