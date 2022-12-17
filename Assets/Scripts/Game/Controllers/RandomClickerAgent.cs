using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class RandomClickerAgent : AIController
    {
        [SerializeField] private int tickPerClick = 10;
        [Range(0, 1)]
        private float selectRate = 0.8f;
        [SerializeField] 
        [Min(3)]
        private int consecutiveSelectCount = 20;
        [Min(2)]
        [SerializeField] 
        private int consecutiveSwapCount = 5;
        private int step = 0;
        private enum Action
        {
            Select,
            Swap,
        }
        private Action currentAction = Action.Select;
        private List<Vector2Int> allTiles;
        protected override void Init()
        {
            allTiles = gameBoard.state.GetAllTiles();
            base.Init();
        }
        protected override IEnumerator OnAction()
        {
            ++step;
            var tileToClick = allTiles[Random.Range(0, allTiles.Count)];
            Debug.Log($"{gameObject.name}: Click {tileToClick}");
            if(currentAction == Action.Select)
            {
                if(step >= consecutiveSelectCount)
                    SwitchAction(tileToClick);
                Select(tileToClick);
            }
            else if(currentAction == Action.Swap)
            {
                if(step >= consecutiveSwapCount)
                    SwitchAction(tileToClick);
                Swap(tileToClick);
            }
            yield return WaitForTicks(tickPerClick);
        }
        private void SwitchAction(Vector2Int tileToClick)
        {
            if(Random.value <= selectRate) 
                currentAction = Action.Select;
            else 
                currentAction = Action.Swap;
            step = 0;
        }
    }
}
