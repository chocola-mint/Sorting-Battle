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
            gameControllerState.Select(tileCoord);
        }
        public void BeginSelection()
        {
            gameControllerState.BeginSelection();
        }
        public void EndSelection()
        {
            gameControllerState.EndSelection();
        }
        private void OnDisable() 
        {
            EndSelection();
        }
        // Start is called before the first frame update
        void Start()
        {
            gameControllerState.onBeginSelection += () => {
                enabled = true;
            };
            gameControllerState.onEndSelection += () => {
                enabled = false;
            };
            enabled = false;
        }

        // Update is called once per frame
        void Update()
        {
        
        }
    }
}
