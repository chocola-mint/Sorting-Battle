using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SortGame
{
    public class Endless1PGame : GameBase<Endless1PGameState>
    {
        [SerializeField] private GameBoard p1GameBoard;
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private bool startGameImmediately = true;
        [SerializeField] private GameOverOverlay gameOverOverlay;
        // Start is called before the first frame update
        void Start()
        {
            gameState = new(p1GameBoard.state);
            if(startGameImmediately)StartGame();
        }
        public override void StartGame()
        {
            GameController.EnableAll();
            gameState.onGameOver += OnGameOver;
            StartTicking(1);
        }
        private void OnGameOver()
        {
            Debug.Log("Game over");
            Debug.Log($"Total score: {p1GameBoard.state.gameScoreState.totalScore}");
            GameController.DisableAll();
            StartCoroutine(CoroGameOver());
        }
        private IEnumerator CoroGameOver()
        {
            var handleP1Clear = StartCoroutine(p1GameBoard.CoroAnimateClearTiles());
            yield return handleP1Clear;
            if(gameOverOverlay) gameOverOverlay.gameObject.SetActive(true);
            else Debug.LogWarning("No game over overlay!");
        }
        private void Update() 
        {
            textMesh.text = $"Level: {gameState.level}\nScore: {p1GameBoard.state.gameScoreState.totalScore}";
        }
    }
}
