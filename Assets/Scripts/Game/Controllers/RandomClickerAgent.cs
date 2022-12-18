using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    /// <summary>
    /// Baseline AI implementation. Picks actions semi-randomly.
    /// <br></br>
    /// In summary: It will do consecutive select moves and swap moves separately.
    /// Whether it decides to select or swap is controlled by the select rate.
    /// </summary>
    public class RandomClickerAgent : AIController
    {
        [SerializeField] 
        private int tickPerClick = 10;
        [SerializeField]
        private int tickPerPush = 30;
        [SerializeField]
        [Range(0, 1)]
        private float selectRate = 0.8f;
        [SerializeField] 
        [Min(3)]
        private int consecutiveSelectCount = 20;
        [Min(2)]
        [SerializeField] 
        private int consecutiveSwapCount = 5;
        [Tooltip("Set to true to constrain random selections to legal tiles whenever possible.")]
        [SerializeField] 
        private bool legalTilesOnly = false;
        // Decision step of the AI. Used to keep track of consecutive actions.
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
            SwitchAction();
            base.Init();
        }
        private Vector2Int GetTileToClick()
        {
            if(legalTilesOnly)
            {
                if(currentAction == Action.Select)
                {
                    var selectableTiles = gameBoard.state.GetAllSelectableTiles();
                    if(selectableTiles.Count > 0)
                        return selectableTiles[Random.Range(0, selectableTiles.Count)];
                }
                else if(currentAction == Action.Swap)
                {
                    var swappableTiles = gameBoard.state.GetAllSwappableTiles();
                    if(swappableTiles.Count > 0)
                        return swappableTiles[Random.Range(0, swappableTiles.Count)];
                }
            }
            return allTiles[Random.Range(0, allTiles.Count)];
        }
        protected override IEnumerator OnAction()
        {
            if(gameBoard.state.GetBoardHeight() < gameBoard.state.gameControllerState.minimumSortedLength)
            {
                Push();
                yield return WaitForTicks(tickPerPush);
                yield break;
            }
            switch (currentAction)
            {
                case Action.Select:
                    Select(GetTileToClick());
                    if (step >= consecutiveSelectCount)
                        SwitchAction();
                    yield return WaitForTicks(tickPerClick);
                    break;
                case Action.Swap:
                    Swap(GetTileToClick());
                    if (step >= consecutiveSwapCount)
                        SwitchAction();
                    yield return WaitForTicks(tickPerClick);
                    break;
            }
            ++step;
        }
        private void SwitchAction()
        {
            if(Random.value <= selectRate) 
                currentAction = Action.Select;
            else 
                currentAction = Action.Swap;
            step = 0;
        }
    }
}
