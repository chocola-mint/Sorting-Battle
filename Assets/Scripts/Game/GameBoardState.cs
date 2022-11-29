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
        public readonly int minimumSortedLength;
        private readonly RemoveHandler remover;
        private readonly SwapHandler swapper;
        /// <summary>
        /// Whether the simulated game environment is paused or not. 
        /// See <see cref="Block"></see> and <see cref="Unblock"></see>.
        /// </summary>
        public bool isBlocking { get; private set; }
        public GameBoardState(Config config)
        {
            Random.InitState(config.seed);
            randomState = Random.state;
            gameGridState = new(config.rowCount, config.columnCount);
            minimumSortedLength = config.minimumSortedLength;
            remover = new(gameGridState);
            swapper = new(gameGridState);
        }
        /// <summary>
        /// Pauses the simulated game environment, ignoring most inputs.
        /// This can be used to process "view" operations, like playing animations.
        /// </summary>
        public void Block()
        {
            isBlocking = true;
        }
        /// <summary>
        /// Unpauses the simulated game environment. See <see cref="Block"></see>.
        /// </summary>
        public void Unblock()
        {
            isBlocking = false;
        }
    }
}
