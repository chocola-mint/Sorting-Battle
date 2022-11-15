using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using SortGame;
[CustomEditor(typeof(GameBoard))]
public class GameBoardEditor : Editor {
    public override void OnInspectorGUI() 
    {
        var gameBoard = target as GameBoard;
        if(GUILayout.Button("Populate grid with random number blocks"))
        {
            gameBoard.LoadRandomNumbers();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        if(GUILayout.Button("Clear grid"))
        {
            gameBoard.ClearTiles();
            EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
        }
        base.OnInspectorGUI(); // Draw built-in inspector.
    }
}