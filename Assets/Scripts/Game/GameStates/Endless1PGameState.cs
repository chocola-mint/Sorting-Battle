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
        public System.Func<int> waitInterval;
        public System.Func<int> levelUpInterval;
        public Endless1PGameState(GameBoardState p1GBState)
        {
            this.p1GBState = p1GBState;
            // Default implementation: Difficulty ramps up in levels closer to 20.
            // Difficulty maxes out at level 20.
            this.waitInterval = () => DefaultLevelToWaitIntervalCurve(p1Level);
            // Default implementation: Level up after every 300 ticks.
            this.levelUpInterval = () => DefaultLevelIntervalCurve(p1Level);
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
