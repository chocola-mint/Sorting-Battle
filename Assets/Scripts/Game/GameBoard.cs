using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SortGame.GameFunctions;

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
        [SerializeField] private int seed = 1337;
        public void LoadRandomNumbers()
        {
            if(!gameGrid) gameGrid = GetComponentInChildren<GameGrid>();
            gameGrid.ClearTiles();
            Random.state = state.randomState; // Load this board's random state.
            gameGrid.LoadRandomNumbers();
            state.randomState = Random.state; // Save afterwards.
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
                    seed = seed, 
                    rowCount = gameGrid.rowCount,
                    columnCount = gameGrid.columnCount,
                    // TODO: Expose these parameters...
                    minimumSortedLength = 3,
                    baseRemoveScore = 50,
                    removeLengthBonus = 25,
                    maxEffectiveCombo = 10,
                    comboScoreStep = 2,
                }
            );
            
        }
        
        // Start is called before the first frame update
        void Start()
        {
            LoadRandomNumbers();
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}