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
        public void LoadRandomNumbers()
        {
            if(!gameGrid) gameGrid = GetComponentInChildren<GameGrid>();
            gameGrid.ClearTiles();
            gameGrid.LoadRandomNumbers();
        }
        public void ClearTiles()
        {
            if(!gameGrid) gameGrid = GetComponentInChildren<GameGrid>();
            gameGrid.ClearTiles();
        }
        private void Awake() 
        {   
            gameGrid = GetComponentInChildren<GameGrid>();
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