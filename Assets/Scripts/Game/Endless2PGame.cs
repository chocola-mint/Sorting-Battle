using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using ChocoUtil.Algorithms;

namespace SortGame
{
    public class Endless2PGame : GameBase<Endless2PGameState>
    {
        [SerializeField] private GameBoard p1GameBoard, p2GameBoard;
        [SerializeField] private TMP_Text p1TextMesh, p2TextMesh;
        // Start is called before the first frame update
        void Start()
        {
            gameState = new(p1GameBoard.state, p2GameBoard.state);
            gameState.onP1Win += OnP1Win;
            gameState.onP2Win += OnP2Win;
            gameState.onDraw += OnDraw;
            gameState.onGameOver += OnGameOver;
            StartTicking(1);
        }
        private void OnP1Win()
        {

        }
        private void OnP2Win()
        {

        }
        private void OnDraw()
        {

        }
        private void OnGameOver()
        {
            Debug.Log("Game over");
            GameController.DisableAll();
        }
        void Update() 
        {
            p1TextMesh.text = $"Score: {p1GameBoard.state.gameScoreState.totalScore}";
            p2TextMesh.text = $"Score: {p2GameBoard.state.gameScoreState.totalScore}";
        }
    }
}
