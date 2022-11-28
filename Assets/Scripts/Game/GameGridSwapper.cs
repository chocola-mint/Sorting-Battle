using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SortGame.GameFunctions;
using ChocoUtil.Coroutines;
namespace SortGame
{
    public interface IOnSwapReceiver
    {
        void OnSwap();
    }
    public class GameGridSwapper : GameGridOperatorBase
    {
        private SwapHandler swapper;
        private NumberBlock currentNumberBlock;
        // Start is called before the first frame update
        void Start()
        {
            swapper = new(gameGrid.state);
            enabled = false;
        }
        public void StartSwapping(Vector2 screenPosition)
        {
            CancelOtherOperatorsAndActivateThis();
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
            CancelOtherOperatorsAndActivateThis();
            return swapper.StartSwapping(target);
        }
        public void SwapTo(Vector2 screenPosition)
        {
            if(!enabled || !currentNumberBlock) return;
            currentNumberBlock.FollowPointer(screenPosition);
            foreach(var numberBlock in Raycast<NumberBlock>(screenPosition))
            {
                SwapTo(numberBlock.gameTile.gridCoord);
                break;
            }
        }
        public void SwapTo(Vector2Int target)
        {
            if(!enabled || !currentNumberBlock) return;
            var commands = swapper.SwapTo(target);
            if(commands.success)
            {
                // Swap was successful. Animate to reflect the changes.
                StartCoroutine(CoSwap(commands));
            }
        }
        IEnumerator CoSwap(SortGame.GameFunctions.SwapHandler.Commands commands)
        {
            var fromTile = gameGrid.GetGameTile(commands.swap.a);
            var toTile = gameGrid.GetGameTile(commands.swap.b);
            var from = fromTile.GetComponentInChildren<NumberBlock>();
            var to = toTile.GetComponentInChildren<NumberBlock>();
            yield return this.All(
                from.MoveTo(toTile),
                to.MoveTo(fromTile));
            foreach(var drop in commands.drops)
            {
                gameGrid.GetGameTile(drop.a).GetComponentInChildren<NumberBlock>()
                .MoveTo(gameGrid.GetGameTile(drop.b));
            }
        }
        public void EndSwapping()
        {
            if(currentNumberBlock)
            {
                currentNumberBlock.StopFollowPointer();
            }
            swapper.EndSwapping();
        }
        private void OnDisable() 
        {
            EndSwapping();    
        }
    }
}
