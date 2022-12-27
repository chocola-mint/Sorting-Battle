using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif
namespace SortGame
{
    /// <summary>
    /// Represents a game instance, containing info like the grid.
    /// There can be multiple game instances, depending on the game mode.
    /// </summary>
    public class GameBoard : MonoBehaviour
    {
        private GameGrid gameGrid;
        public GameBoardState state { get; private set; }
        [Tooltip("Fixed seed to initialize the random state of the board. Useful for debugging or procedural generation.")]
        [SerializeField] private int seed = 1337;
        [Tooltip("Enable to use the engine's random state at the time of execution, instead of a fixed seed.")]
        [SerializeField] private bool randomSeed = false;
        [SerializeField] private int numberUpperBound = 100;
        /// <summary>
        /// This method is used to inject random seed (for sharing seeds).
        /// </summary>
        public void InjectSeed(int seed)
        {
            this.seed = seed;
            randomSeed = false;
        }
#if UNITY_EDITOR
        [ContextMenu("Save grid state")]
        public void SaveGridState()
        {
            var path = EditorUtility.SaveFilePanel("Save grid state as CSV",
            "", $"GameGridState_{System.DateTime.Now}.csv", "csv");
            if(path.Length != 0)
            {
                using(var writer = new System.IO.StreamWriter(path))
                {
                    writer.Write(state.gameGridState.Serialize());
                }
            }
        }
#endif
        public void LoadRandomNumbers(float rowPercentage = 1)
        {
            if(!gameGrid) gameGrid = GetComponentInChildren<GameGrid>();
            gameGrid.ClearTiles();
            using(new RandomScope(state))
                gameGrid.LoadRandomNumbers(rowPercentage);
        }
        public void ClearTiles()
        {
            if(!gameGrid) gameGrid = GetComponentInChildren<GameGrid>();
            gameGrid.ClearTiles();
        }
        private void Awake() 
        {   
            gameGrid = GetComponentInChildren<GameGrid>();
            state = new(
                new(){
                    useSeed = !randomSeed,
                    seed = seed, 
                    rowCount = gameGrid.rowCount,
                    columnCount = gameGrid.columnCount,
                    numberUpperBound = numberUpperBound,
                    // TODO: Expose these parameters...
                    minimumSortedLength = 3,
                    baseRemoveScore = 50,
                    removeLengthBonus = 25,
                    maxEffectiveCombo = 10,
                    comboScoreStep = 2,
                }
            );
            
        }
    }
}