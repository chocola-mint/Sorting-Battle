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
        // Start is called before the first frame update
        void Start()
        {
            gameState = new(p1GameBoard.state);
            gameState.onGameOver += OnGameOver;
            StartTicking(1);
        }
        private void OnGameOver()
        {
            Debug.Log("Game over");
            Debug.Log($"Total score: {p1GameBoard.state.gameScoreState.totalScore}");
            GameController.DisableAll();
        }
        private void Update() 
        {
            textMesh.text = $"Level: {gameState.p1Level}\nScore: {p1GameBoard.state.gameScoreState.totalScore}";
        }
    }
}
