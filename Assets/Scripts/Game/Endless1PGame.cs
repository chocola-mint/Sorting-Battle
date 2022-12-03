using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SortGame
{
    public class Endless1PGame : MonoBehaviour
    {
        private Endless1PGameState gameState;
        [SerializeField] private GameBoard p1GameBoard;
        private static readonly WaitForFixedUpdate waitForFixedUpdate = new();
        // Start is called before the first frame update
        void Start()
        {
            gameState = new(p1GameBoard.state);
            gameState.onGameOver += OnGameOver;
            StartCoroutine(CoroTick(fixedUpdateToTick: 1));
        }
        private IEnumerator CoroTick(int fixedUpdateToTick)
        {
            if(fixedUpdateToTick <= 0) 
                throw new System.ArgumentException(
                    $"Argument {nameof(fixedUpdateToTick)} must be greater than 0.");
            while(true)
            {
                gameState.Tick();
                for(int i = 0; i < fixedUpdateToTick; ++i)
                    yield return waitForFixedUpdate;
            }
        }
        private void OnGameOver()
        {
            Debug.Log("Game over");
            Debug.Log($"Total score: {p1GameBoard.state.gameScoreState.totalScore}");
        }
    }
}
