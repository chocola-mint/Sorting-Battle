using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;

namespace SortGame
{
    public class Endless2PGame : GameBase<Endless2PGameState>
    {
        [SerializeField] private Endless2PGameSettings settings;
        [SerializeField] private GameBoard p1GameBoard, p2GameBoard;
        [SerializeField] private PlayerType p1PlayerType, p2PlayerType;
        [SerializeField] private TMP_Text p1TextMesh, p2TextMesh;
        [SerializeField] private bool startGameImmediately = true;
        [SerializeField] private GameOverOverlay gameOverOverlay;
        private void Awake() 
        {
            if(settings != null)
            {
                // Apply overrides.
                if(settings.p1PlayerTypeOverride != null) p1PlayerType = settings.p1PlayerTypeOverride;
                if(settings.p2PlayerTypeOverride != null) p2PlayerType = settings.p2PlayerTypeOverride;
            }
            Debug.Assert(p1PlayerType != null);
            Debug.Assert(p2PlayerType != null);
        }
        // Start is called before the first frame update
        void Start()
        {
            gameState = new(p1GameBoard.state, p2GameBoard.state);
            p1PlayerType.CreatePrefabInstance(p1GameBoard, "P1");
            p2PlayerType.CreatePrefabInstance(p2GameBoard, "P2");
            if(startGameImmediately)StartGame();
        }
        public override void StartGame()
        {
            GameController.EnableAll();
            gameState.onP1Win += OnP1Win;
            gameState.onP2Win += OnP2Win;
            gameState.onDraw += OnDraw;
            gameState.onGameOver += OnGameOver;
            StartTicking(1);
        }
        private void OnP1Win()
        {
            Debug.Log("P1 Win");
            if(gameOverOverlay) gameOverOverlay.SetResultText("P1 Win!");
        }
        private void OnP2Win()
        {
            Debug.Log("P2 Win");
            if(gameOverOverlay) gameOverOverlay.SetResultText("P2 Win!");
        }
        private void OnDraw()
        {
            if(gameOverOverlay) gameOverOverlay.SetResultText("Draw...");
        }
        private void OnGameOver()
        {
            Debug.Log("Game over");
            GameController.DisableAll();
            StartCoroutine(CoroGameOver());
        }
        private IEnumerator CoroGameOver()
        {
            var handleP1Clear = StartCoroutine(p1GameBoard.CoroAnimateClearTiles());
            var handleP2Clear = StartCoroutine(p2GameBoard.CoroAnimateClearTiles());
            yield return handleP1Clear;
            yield return handleP2Clear;
            if(gameOverOverlay) gameOverOverlay.gameObject.SetActive(true);
            else Debug.LogWarning("No game over overlay!");
        }
        void Update() 
        {
            p1TextMesh.text = $"Score: {p1GameBoard.state.gameScoreState.totalScore}";
            p2TextMesh.text = $"Score: {p2GameBoard.state.gameScoreState.totalScore}";
        }
    }
}
