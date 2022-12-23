using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
using SortGame.Core;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SortGame
{
    [RequireComponent(typeof(HorizontalOrVerticalLayoutGroup))]
    [RequireComponent(typeof(RectTransform))]
    public class GameGrid : MonoBehaviour
    {
        [SerializeField]
        private int numberOfRows = 10, numberOfColumns = 5;
        public enum InferSizeMode
        {
            UseWidth, UseHeight
        }
        [SerializeField] private InferSizeMode inferSizeMode;
        [SerializeField] private GameObject rowPrefab, tilePrefab;
        [SerializeField] GameObject numberBlockPrefab;
        public int rowCount => numberOfRows;
        public int columnCount => numberOfColumns;
        public GameGridState state { get; private set; }
        private System.Lazy<List<GameTile>> allTiles;
        public GameTile GetGameTile(Vector2Int tileCoord)
        {
            return transform.GetChild(tileCoord.x).GetChild(tileCoord.y).GetComponent<GameTile>();
        }
        public IEnumerable<GameTile> GetAllTiles()
        {
            foreach(Transform row in transform)
                foreach(Transform tile in row)
                    yield return tile.GetComponent<GameTile>();
        }
        public void LoadRandomNumbers(float rowPercentage = 1)
        {
            #if UNITY_EDITOR
            
            if(UnityEditor.EditorApplication.isPlaying)
            {
                // Runtime GameGridState exists here.
                state.LoadRandom(rowPercentage: rowPercentage);
            }
            else 
            {
                foreach(var gameTile in GetAllTiles())
                    (PrefabUtility.InstantiatePrefab(numberBlockPrefab, gameTile.transform) as GameObject)
                    .GetComponent<NumberBlock>().SetNumber(Random.Range(0, 100));
            }
            #else
            // Runtime GameGridState exists here.
            state.LoadRandom(rowPercentage: rowPercentage);
            #endif
        }
        public void NewBlock(Vector2Int tileCoord)
        {
            var numberBlock = Instantiate(numberBlockPrefab, GetGameTile(tileCoord).transform)
            .GetComponent<NumberBlock>();
            numberBlock.SetNumber(state.Get(tileCoord));
            if(state.IsTrash(tileCoord)) numberBlock.ToTrash(animate: false);
        }
        public void ClearTiles()
        {
            foreach(var numberBlock in GetComponentsInChildren<NumberBlock>())
                GameObject.DestroyImmediate(numberBlock.gameObject);
        }
        private void Awake() 
        {
            allTiles = new(() => GetAllTiles().ToList(), false);
            state = GetComponentInParent<GameBoard>().state.gameGridState;
            ClearTiles();
            state.onNewBlock += NewBlock;
        }
    }
}