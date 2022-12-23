using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ChocoUtil.Algorithms;

namespace SortGame.Core
{
    /// <summary>
    /// Implementation of the Endless 2P Game mode.
    /// </summary>
    public class Endless2PGameState : VersusGameState
    {
        public event System.Action onP1Win, onP2Win, onDraw;
        public Endless2PGameState(
            GameBoardState p1GBState, 
            GameBoardState p2GBState)
        {
            p1GBState.status = p2GBState.status = GameBoardState.Status.Active;
            RegisterPlayer(p1GBState, () => onP1Win?.Invoke());
            RegisterPlayer(p2GBState, () => onP2Win?.Invoke());
            InitEvents();
        }
        // This override makes rows appear less frequently in this game mode.
        protected override int GetTickBetweenEachNewRow()
        {
            return (int)Mathf.Lerp(300, 100, Ease.InQuad(level / 20.0f));
        }
        protected override void OnDraw()
        {
            onDraw?.Invoke();
        }
    }
}