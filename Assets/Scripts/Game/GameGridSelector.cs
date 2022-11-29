using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;
using System.Linq;

namespace SortGame
{
    public interface IOnSelectReceiver
    {
        void OnSelect();
    }
    public interface IOnDeselectReceiver
    {
        void OnDeselect();
    }
    public class GameGridSelector : GameGridOperatorBase
    {
        private SelectionHandler selectionHandler;
        public void Select(Vector2 screenPosition)
        {
            if(!enabled) return;
            foreach(var numberBlock in Raycast<NumberBlock>(screenPosition))
            {
                Select(numberBlock.gameTile.gridCoord);
                return;
            }
        }
        public void Select(Vector2Int tileCoord)
        {
            if(!enabled) return;
            if(selectionHandler.Select(tileCoord))
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<IOnSelectReceiver>())
                    receiver.OnSelect();
        }
        public void BeginSelection()
        {
            CancelOtherOperatorsAndActivateThis();
            selectionHandler.BeginSelection();
        }
        public void EndSelection()
        {
            var selection = selectionHandler.EndSelection();
            foreach(var tileCoord in selection)
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<IOnDeselectReceiver>())
                    receiver.OnDeselect();
        }
        private void OnDisable() 
        {
            EndSelection();
        }
        // Start is called before the first frame update
        void Start()
        {
            selectionHandler = new(gameGrid.state);
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
