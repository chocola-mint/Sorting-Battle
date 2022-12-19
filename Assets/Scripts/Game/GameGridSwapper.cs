using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;
using ChocoUtil.Coroutines;
namespace SortGame
{
    // TODO: Switch to using GameControllerState.
    public class GameGridSwapper : GameGridOperatorBase
    {
        private NumberBlock currentNumberBlock;
        // Start is called before the first frame update
        void Start()
        {
            // Make "enabled" reflect swapping state.
            gameControllerState.onStartSwapping += () => {
                enabled = true;
            };
            gameControllerState.onEndSwapping += () => {
                if(currentNumberBlock)
                {
                    currentNumberBlock.StopFollowPointer();
                }
                enabled = false;
            };
            enabled = false;
        }
        public void StartSwapping(Vector2 screenPosition)
        {
            foreach(var numberBlock in Raycast<NumberBlock>(screenPosition))
            {
                if(StartSwapping(numberBlock.gameTile.gridCoord))
                {
                    currentNumberBlock = numberBlock;
                    currentNumberBlock.FollowPointer(screenPosition);
                    break;
                }
            }
        }
        public bool StartSwapping(Vector2Int target)
        {
            return gameControllerState.StartSwapping(target);
        }
        public void SwapTo(Vector2 screenPosition)
        {
            if(!enabled) return;
            if(currentNumberBlock)
                currentNumberBlock.FollowPointer(screenPosition);
            foreach(var numberBlock in Raycast<NumberBlock>(screenPosition))
            {
                SwapTo(numberBlock.gameTile.gridCoord);
                break;
            }
        }
        public void SwapTo(Vector2Int target)
        {
            if(!enabled) return;
            gameControllerState.SwapTo(target);
        }
        public void EndSwapping()
        {
            gameControllerState.EndSwapping();
        }
        private void OnDisable() 
        {
            EndSwapping();    
        }
    }
}
