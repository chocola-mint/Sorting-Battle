using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    public class Endless1PGameState : GameState
    {
        protected readonly GameBoardState p1GBState;
        protected readonly float emptyRowPercentage = 0.8f;
        public Endless1PGameState(GameBoardState p1GBState, float emptyRowPercentage = 0.8f)
        {
            this.p1GBState = p1GBState;
            this.emptyRowPercentage = emptyRowPercentage;
            InitEvents();
        }
        protected override void InitEvents()
        {
            base.InitEvents();
            PushEvent(0, LoadEvent);
        } 
        private void LoadEvent()
        {
            p1GBState.gameGridState.LoadRandom(rowPercentage: emptyRowPercentage);
        }
        protected override sealed void PushNewRowEvent()
        {
            if(p1GBState.PushNewRow(p1GBState.gameGridState.columnCount - 1)) GameOver();
            else PushEvent(GetTickBetweenEachNewRow(), PushNewRowEvent);
        }
    }
}
