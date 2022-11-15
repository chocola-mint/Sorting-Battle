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
    [RequireComponent(typeof(GraphicRaycaster))]
    public class GameBoard : MonoBehaviour
    {
        public const int MinimumSortedLength = 3;
        private GraphicRaycaster raycaster;
        private GameGrid gameGrid;
        private SelectionHandler selectionHandler;
        public void Select(Vector2 screenPosition)
        {
            List<RaycastResult> raycastResults = new();
            raycaster.Raycast(
                new PointerEventData(EventSystem.current){ 
                    position = screenPosition,
                }, 
                raycastResults);
            foreach(var raycastResult in raycastResults)
            {
                var numberBlock = raycastResult.gameObject.GetComponentInParent<NumberBlock>();
                if(numberBlock)
                {
                    selectionHandler.Select(numberBlock.gameTile.gridCoord);
                    break;
                }
            }
        }
        public void Select(Vector2Int tileCoord)
        {
            selectionHandler.Select(tileCoord);
        }
        public void BeginSelection() => selectionHandler.BeginSelection();
        public void EndSelection() => selectionHandler.EndSelection();
        public void LoadRandomNumbers()
        {
            gameGrid.ClearTiles();
            gameGrid.LoadRandomNumbers();
        }
        public void ClearTiles()
        {
            gameGrid.ClearTiles();
        }
        private void Awake() 
        {
            raycaster = GetComponent<GraphicRaycaster>();    
            gameGrid = GetComponentInChildren<GameGrid>();
        }
        
        // Start is called before the first frame update
        void Start()
        {
            selectionHandler = new(gameGrid.state);
            LoadRandomNumbers();
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }
}