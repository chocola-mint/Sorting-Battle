using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    public class Endless1PGameState : GameState
    {
        public int p1Level { get; private set; } = 0;
        private GameBoardState p1GBState;
        private System.Func<int> waitInterval;
        private System.Func<int> levelUpInterval;
        public Endless1PGameState(GameBoardState p1GBState)
        {
            this.p1GBState = p1GBState;
            // Default implementation: Difficulty ramps up in levels closer to 20.
            // Difficulty maxes out at level 20.
            this.waitInterval = () => {
                return (int)Mathf.Lerp(150, 60, Ease.InQuad(p1Level / 20.0f));
            };
            // Defaulty implementation: Level up after every 300 ticks.
            this.levelUpInterval = () => 300;
            PushInitialEvents();
        }
        public Endless1PGameState(
            GameBoardState p1GBState, 
            System.Func<int> waitInterval, System.Func<int> levelUpInterval)
        {
            this.p1GBState = p1GBState;
            this.waitInterval = waitInterval;
            this.levelUpInterval = levelUpInterval;
            PushInitialEvents();
        } 
        private void PushInitialEvents()
        {
            PushEvent(0, LoadEvent);
            PushEvent(waitInterval(), PushNewRowEvent);
            PushEvent(levelUpInterval(), LevelUpEvent);
        } 
#region Events
        private void LoadEvent()
        {
            p1GBState.gameGridState.LoadRandom(rowPercentage:0.8f);
        }
        private void PushNewRowEvent()
        {
            if(PushNewRow()) GameOver();
            else PushEvent(waitInterval(), PushNewRowEvent);
        }
        private void LevelUpEvent()
        {
            ++p1Level;
            Debug.Log($"Level: {p1Level}");
            PushEvent(levelUpInterval(), LevelUpEvent);
        }
#endregion
        private bool PushNewRow()
        {
            return p1GBState.PushNewRow(p1GBState.gameGridState.columnCount - 1);
        }
    }
}
