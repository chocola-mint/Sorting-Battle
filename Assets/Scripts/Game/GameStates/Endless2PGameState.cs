using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame
{
    public class Endless2PGameState : GameState
    {
        public int level { get; private set; } = 0;
        private GameBoardState p1State, p2State;
        private int p1LastPressureTick, p2LastPressureTick;
        // Array to put each player's board, for convenience.
        private GameBoardState[] playerStates;
        public System.Func<int> waitInterval;
        public System.Func<int> levelUpInterval;
        public event System.Action onP1Win, onP2Win, onDraw;
        public Endless2PGameState(
            GameBoardState p1GBState, 
            GameBoardState p2GBState)
        {
            this.p1State = p1GBState;
            this.p2State = p2GBState;
            p1State.status = p2State.status = GameBoardState.Status.Active;
            playerStates = new GameBoardState[]{ 
                p1State,
                p2State,
            };
            p1LastPressureTick = 0;
            p2LastPressureTick = 0;
            p1State.gameScoreState.onScoreIncrease += x => {
                p1State.gamePressureState.Attack(p2State.gamePressureState, x.removeCount + x.combo / 2);
                p2LastPressureTick = tick;
            };
            p2State.gameScoreState.onScoreIncrease += x => {
                p2State.gamePressureState.Attack(p1State.gamePressureState, x.removeCount + x.combo / 2);
                p1LastPressureTick = tick;
            };
            // Default implementation: Difficulty ramps up in levels closer to 20.
            // Difficulty maxes out at level 20.
            this.waitInterval = () => DefaultLevelToWaitIntervalCurve(level);
            // Defaulty implementation: Level up after every 300 ticks.
            this.levelUpInterval = () => DefaultLevelIntervalCurve(level);
            PushEvent(0, LoadEvent);
            PushEvent(waitInterval(), PushNewRowEvent);
            PushEvent(levelUpInterval(), LevelUpEvent);
            PushEvent(1, CheckAttackP1Event);
            PushEvent(1, CheckAttackP2Event);
        }
#region Events
        private void LoadEvent()
        {
            foreach(var state in playerStates)
                state.gameGridState.LoadRandom(rowPercentage:0.8f);
        }
        private void PushNewRowEvent()
        {
            foreach(var state in playerStates)
            {
                if(state.PushNewRow(state.gameGridState.columnCount - 1))
                    state.status = GameBoardState.Status.Lose;
            }
            if(CheckGameResultState()) GameOver();
            else PushEvent(waitInterval(), PushNewRowEvent);
        }
        private void LevelUpEvent()
        {
            ++level;
            Debug.Log($"Level: {level}");
            PushEvent(levelUpInterval(), LevelUpEvent);
        }
        private void CheckAttackP1Event() => CheckAttackEvent(p1State, ref p1LastPressureTick, CheckAttackP1Event);
        private void CheckAttackP2Event() => CheckAttackEvent(p2State, ref p2LastPressureTick, CheckAttackP2Event);
        private void CheckAttackEvent(GameBoardState subject, ref int lastPressureTick, System.Action checkEvent)
        {
            bool overflow = false;
            if(tick - lastPressureTick >= 150 + subject.gamePressureState.pressure)
            {
                // Can dump trash from pressure.
                lastPressureTick = tick;
                // TODO: Make this consume percentage.
                int trash = subject.gamePressureState.ConsumePressure(50);
                if(trash > 0)
                {
                    int numRows = 1 + (trash / 10);
                    overflow = subject.PushTrashRows(
                        numRows, 
                        Mathf.RoundToInt(Mathf.Lerp(1, 4, (float) trash / (float) 10.0f)));
                }
            }
            if(overflow)
            {
                subject.status = GameBoardState.Status.Lose;
                if(CheckGameResultState()) GameOver();
            }
            else PushEvent(1, checkEvent);
        }
#endregion
        private bool CheckGameResultState()
        {
            bool p1Lose = p1State.status == GameBoardState.Status.Lose;
            bool p2Lose = p2State.status == GameBoardState.Status.Lose;
            if(p1Lose && p2Lose)
            {
                // Draw
                Debug.Log("Draw");
                onDraw?.Invoke();
                return true;
            }
            else if(p2Lose)
            {
                // P1 win
                Debug.Log("P1 win");
                onP1Win?.Invoke();
                return true;
            }
            else if(p1Lose)
            {
                // P2 win
                Debug.Log("P2 win");
                onP2Win?.Invoke();
                return true;
            }
            return false;
        }
    }
}