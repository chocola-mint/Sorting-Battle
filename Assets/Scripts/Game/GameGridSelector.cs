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
    public interface IOnRemoveReceiver
    {
        void OnRemove();
    }
    // TODO: Switch to using GameControllerState.
    // TODO: Also, add GameGridRemover.
    public class GameGridSelector : GameGridOperatorBase
    {
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
            if(gameControllerState.Select(tileCoord))
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<IOnSelectReceiver>())
                    receiver.OnSelect();
        }
        public void BeginSelection()
        {
            CancelOtherOperatorsAndActivateThis();
            gameControllerState.BeginSelection();
        }
        public void EndSelection()
        {
            var (selection, drops, shouldRemove) = gameControllerState.EndSelection();
            foreach(var tileCoord in selection)
            {    
                foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponents<IOnDeselectReceiver>())
                    receiver.OnDeselect();
                if(shouldRemove)
                    foreach(var receiver in gameGrid.GetGameTile(tileCoord).GetComponentsInChildren<IOnRemoveReceiver>())
                        receiver.OnRemove();
            }
            foreach(var drop in drops)
            {
                gameGrid.GetGameTile(drop.a).GetComponentInChildren<NumberBlock>()
                .MoveTo(gameGrid.GetGameTile(drop.b));
            }
        }
        private void OnDisable() 
        {
            EndSelection();
        }
        // Start is called before the first frame update
        void Start()
        {
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
