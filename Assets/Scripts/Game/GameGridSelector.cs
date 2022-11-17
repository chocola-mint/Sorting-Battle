using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using SortGame.GameFunctions;

namespace SortGame
{
    public interface ReceiveEvent_OnSelect
    {
        void OnSelect();
    }
    public interface ReceiveEvent_OnDeselect
    {
        void OnDeselect();
    }
    [RequireComponent(typeof(GraphicRaycaster))]
    public class GameGridSelector : MonoBehaviour
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
                    Select(numberBlock.gameTile.gridCoord);
                    break;
                }
            }
        }
        public void Select(Vector2Int tileCoord)
        {
            if(selectionHandler.Select(tileCoord))
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<ReceiveEvent_OnSelect>())
                    receiver.OnSelect();
        }
        public void BeginSelection()
        {
            selectionHandler.BeginSelection();
        }
        public void EndSelection()
        {
            var selection = selectionHandler.EndSelection();
            foreach(var tileCoord in selection)
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<ReceiveEvent_OnDeselect>())
                    receiver.OnDeselect();
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
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
