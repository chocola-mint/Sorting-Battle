using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame.Core
{
    /// <summary>
    /// Implementation of the Endless 1P Game mode.
    /// </summary>
    public class Endless1PGameState : GameState
    {
        protected readonly GameBoardState p1GBState;
        protected readonly float emptyRowPercentage = 0.8f;
        private int lastComboTick = int.MaxValue;
        private const int ResetComboTickDuration = 100;
        public Endless1PGameState(GameBoardState p1GBState, float emptyRowPercentage = 0.8f)
        {
            this.p1GBState = p1GBState;
            this.emptyRowPercentage = emptyRowPercentage;
            // This does not affect gameplay. It just makes the UI easier to implement.
            // Treat as if everyone loses all pressure upon game over.
            onGameOver += () => {
                p1GBState.gamePressureState.ConsumePressure(p1GBState.gamePressureState.maxPressure);
            };
            p1GBState.onOverflow += GameOver;
            InitEvents();
        }
        protected override void InitEvents()
        {
            base.InitEvents();
            PushEvent(0, LoadEvent);
        } 
        private void LoadEvent()
        {
            using(new RandomScope(p1GBState))
                p1GBState.gameGridState.LoadRandom(rowPercentage: emptyRowPercentage);
        }
        private void CheckResetComboEvent()
        {
            if(tick - lastComboTick >= ResetComboTickDuration)
            {
                p1GBState.gameScoreState.ResetCombo();
            }
            PushEvent(1, CheckResetComboEvent);
        }
        protected override sealed void PushNewRowEvent()
        {
            if(p1GBState.PushNewRow(p1GBState.gameGridState.columnCount - 1)) GameOver();
            else PushEvent(GetTickBetweenEachNewRow(), PushNewRowEvent);
        }
    }
}
