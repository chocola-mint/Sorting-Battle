using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace SortGame
{
    public class Endless1PGame : GameBase<Endless1PGameState>
    {
        [SerializeField] private GameBoard p1GameBoard;
        [SerializeField] private PlayerType p1PlayerType;
        [SerializeField] private TMP_Text textMesh;
        [SerializeField] private bool startGameImmediately = true;
        [SerializeField] private GameOverOverlay gameOverOverlay;
        private GameEventDelegate gameEventDelegate;
        // Start is called before the first frame update
        void Start()
        {
            gameEventDelegate = GetComponent<GameEventDelegate>();
            gameState = new(p1GameBoard.state);
            p1PlayerType.CreatePrefabInstance(p1GameBoard, "P1");
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
            StopTicking();
            StartCoroutine(CoroGameOver());
        }
        private IEnumerator CoroGameOver()
        {
            gameEventDelegate.onGameOverStart.Invoke();
            var handleP1Clear = StartCoroutine(p1GameBoard.CoroAnimateClearTilesRowByRow());
            yield return handleP1Clear;
            if(gameOverOverlay) gameOverOverlay.gameObject.SetActive(true);
            else Debug.LogWarning("No game over overlay!");
            gameEventDelegate.onGameOverEnd.Invoke();
        }
        private void Update() 
        {
            textMesh.text = $"Level: {gameState.level}\nScore: {p1GameBoard.state.gameScoreState.totalScore:00000}";
        }
    }
}
