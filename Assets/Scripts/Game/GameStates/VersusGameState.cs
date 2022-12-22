using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SortGame
{
    /// <summary>
    /// Abstract base classes for game modes that have more than 1 player.
    /// </summary>
    public abstract class VersusGameState : GameState
    {
        /// <summary>
        /// Class that represents a player, describing interactions between each player.
        /// <br></br>
        /// Basically a wrapper around GameBoardState.
        /// </summary>
        protected class PlayerState
        {
            public readonly GameBoardState gameBoardState;
            // Back-pointer to reference VersusGameState (for PushEvent, etc.)
            private readonly VersusGameState gameState;
            public readonly System.Action onWin;
            private int lastPressureTick = 0;
            private const int PressureTickDuration = 120;
            private const int MaxPressurePerAttack = 20;
            private const int TrashPerAdditionalRow = 10; // How much trash equals one additional row.
            private int lastComboTick = int.MaxValue;
            private const int ResetComboTickDuration = 200;
            public PlayerState(VersusGameState versusGameState, GameBoardState gameBoardState, System.Action onWin)
            {
                this.gameState = versusGameState;
                this.gameBoardState = gameBoardState;
                this.onWin = onWin;
                gameBoardState.gameScoreState.onScoreIncrease += Attack;
                // This does not affect gameplay. It just makes the UI easier to implement.
                // Treat as if everyone loses all pressure upon game over.
                versusGameState.onGameOver += () => {
                    gameBoardState.gamePressureState.ConsumePressure(gameBoardState.gamePressureState.maxPressure);
                };
                gameBoardState.onOverflow += () => {
                    gameBoardState.status = GameBoardState.Status.Lose;
                    if(versusGameState.CheckGameResultState()) versusGameState.GameOver();
                };
                // Load per-player events here.
                versusGameState.PushEvent(0, LoadEvent);
                versusGameState.PushEvent(1, CheckReceiveTrashEvent);
            }
            // ! This is invoked whenever the player's pressure changed.
            // ! Together with "PressureTickDuration", this makes each ReceiveTrashEvent
            // ! happen at least PressureTickDuration apart.
            private void ResetPressureTick() => lastPressureTick = gameState.tick;
            private void Attack(GameScoreState.ScoreIncreaseInfo scoreIncreaseInfo)
            {
                // Find a target to attack.
                var target = gameState.FindTarget(this);
                // Use GamePressureState.Attack to transfer pressure to target.
                gameBoardState.gamePressureState.Attack(
                    other: target.gameBoardState.gamePressureState, 
                    attackPower: ComputeAttackPower(scoreIncreaseInfo));
                // Reset target's pressure tick.
                target.ResetPressureTick();
            }
            private int ComputeAttackPower(GameScoreState.ScoreIncreaseInfo scoreIncreaseInfo)
            {
                return scoreIncreaseInfo.removeCount + scoreIncreaseInfo.combo / 2;
            }
            private (int numRows, int numColumns) ComputeAttackLoad(int trash)
            {
                if(trash > 0) 
                    return (
                        numRows: 1 + trash / TrashPerAdditionalRow, 
                        numColumns: Mathf.RoundToInt(
                            Mathf.Lerp(
                                1, 
                                gameBoardState.gameGridState.columnCount - 1, 
                                //If there's at least one additional row, 
                                // the top row must have only one empty tile.
                                (float) trash / (float) TrashPerAdditionalRow)));
                else return (numRows: 0, numColumns: 0);
            }
#region Per-Player Events
            private void LoadEvent()
            {
                using(new RandomScope(gameBoardState))
                    gameBoardState.gameGridState.LoadRandom(rowPercentage:0.8f);
            }
            private void CheckResetComboEvent()
            {
                if(gameState.tick - lastComboTick >= ResetComboTickDuration)
                {
                    gameBoardState.gameScoreState.ResetCombo();
                }
                gameState.PushEvent(1, CheckResetComboEvent);
            }
            private void CheckReceiveTrashEvent()
            {
                bool overflow = false;
                if(gameState.tick - lastPressureTick >= PressureTickDuration)
                {
                    // Can dump trash from pressure.
                    int trash = gameBoardState.gamePressureState.ConsumePressure(MaxPressurePerAttack);
                    (int trashRows, int trashColumns) = ComputeAttackLoad(trash);
                    if(trashRows > 0) 
                        overflow = gameBoardState.PushTrashRows(trashRows, trashColumns);
                    // Because we consumed pressure, we have to reset pressure tick here.
                    ResetPressureTick();
                }
                if(overflow)
                {
                    gameBoardState.status = GameBoardState.Status.Lose;
                    if(gameState.CheckGameResultState()) gameState.GameOver();
                }
                else gameState.PushEvent(1, CheckReceiveTrashEvent);
            }
#endregion
        }
        // List of players in the game.
        private List<PlayerState> playerStates = new();
        /// <summary>
        /// Register a new player.
        /// </summary>
        /// <param name="gameBoardState">The player's GameBoardState.</param>
        protected void RegisterPlayer(GameBoardState gameBoardState, System.Action callbackOnWin)
        {
            playerStates.Add(new(this, gameBoardState, callbackOnWin));
        }
        /// <summary>
        /// Find a valid target, given an attacking player.
        /// <br></br>
        /// Can be overridden by child classes to use other algorithms. 
        /// The default implementation just finds the first player that's not the attacker.
        /// </summary>
        /// <returns>The player to attack.</returns>
        protected virtual PlayerState FindTarget(PlayerState attacker)
        {
            foreach(var player in playerStates)
                if(player != attacker) return player;
            throw new System.Exception("No target available in versus game!");
        }
        /// <summary>
        /// Callback invoked when the game ends in a draw.
        /// </summary>
        protected abstract void OnDraw();
        /// <summary>
        /// Method to invoke to check if the game has been decided.
        /// <br></br>
        /// Can be overridden to add different win conditions. 
        /// The default win condition is "last man standing".
        /// </summary>
        /// <returns>True if game has been decided. False otherwise.</returns>
        protected virtual bool CheckGameResultState()
        {
            if(playerStates.All(player => player.gameBoardState.status == GameBoardState.Status.Lose))
            {
                // Every player lost on the same tick, so it's a draw.
                // This may happen during PushNewRowEvent.
                OnDraw();
                return true;
            }
            else if(playerStates.All(
                player => player.gameBoardState.status == GameBoardState.Status.Active
                || player.gameBoardState.status == GameBoardState.Status.Inactive))
            {
                // Game is not decided yet.
                return false;
            }
            else
            {
                // Find players who haven't lost yet.
                var survivors = playerStates
                .Where(player => player.gameBoardState.status != GameBoardState.Status.Lose)
                .ToList();
                // If there's only one player left who hasn't lost, invoke their onWin callback.
                if(survivors.Count == 1)
                {
                    survivors[0].onWin?.Invoke();
                    return true;
                }
                // Otherwise the game isn't decided yet.
                else return false;
            }

        }
        /// <summary>
        /// VersusGameState's PushNewRowEvent implementation. Uses PlayerState.
        /// </summary>
        protected override sealed void PushNewRowEvent()
        {
            foreach(var player in playerStates)
            {
                // We set triggerEvent to false here to take care of the edge case of a draw.
                if(player.gameBoardState.PushNewRow(player.gameBoardState.gameGridState.columnCount - 1, triggerEvent: false))
                    player.gameBoardState.status = GameBoardState.Status.Lose;
            }
            if(CheckGameResultState()) GameOver();
            else PushEvent(GetTickBetweenEachNewRow(), PushNewRowEvent);
        }
    }
}
