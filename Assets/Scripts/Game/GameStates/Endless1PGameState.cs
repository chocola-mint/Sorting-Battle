using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    public class Endless1PGameState : GameState
    {
        private GameBoardState p1GBState;
        public Endless1PGameState(GameBoardState p1GBState)
        {
            this.p1GBState = p1GBState;
            InitEvents();
        }
        protected override void InitEvents()
        {
            base.InitEvents();
            PushEvent(0, LoadEvent);
        } 
        private void LoadEvent()
        {
            p1GBState.gameGridState.LoadRandom(rowPercentage:0.8f);
        }
        protected override sealed void PushNewRowEvent()
        {
            if(p1GBState.PushNewRow(p1GBState.gameGridState.columnCount - 1)) GameOver();
            else PushEvent(GetTickBetweenEachNewRow(), PushNewRowEvent);
        }
    }
}
