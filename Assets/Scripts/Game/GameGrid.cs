using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using TMPro;
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
        public void LoadRandomNumbers()
        {
            #if UNITY_EDITOR
            
            if(UnityEditor.EditorApplication.isPlaying)
            {
                // Runtime GameGridState exists here.
                state.LoadRandom();
                foreach(var gameTile in allTiles.Value)
                    Instantiate(numberBlockPrefab, gameTile.transform)
                    .GetComponent<NumberBlock>().SetNumber(state.Get(gameTile.gridCoord));
            }
            else 
            {
                foreach(var gameTile in GetAllTiles())
                    (PrefabUtility.InstantiatePrefab(numberBlockPrefab, gameTile.transform) as GameObject)
                    .GetComponent<NumberBlock>().SetNumber(Random.Range(0, 100));
            }
            #else
            // Runtime GameGridState exists here.
            state.LoadRandom();
            foreach(var gameTile in allTiles.Value)
                Instantiate(numberBlockPrefab, gameTile.transform)
                .GetComponent<NumberBlock>().SetNumber(state.grid[gameTile.gridCoord.x, gameTile.gridCoord.y].number);
            #endif
        }
        public void ClearTiles()
        {
            foreach(var numberBlock in GetComponentsInChildren<NumberBlock>())
                GameObject.DestroyImmediate(numberBlock.gameObject);
        }
        private void Awake() 
        {
            allTiles = new(() => GetAllTiles().ToList());
            state = new(numberOfRows, numberOfColumns);
            ClearTiles();
        }
        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }


}