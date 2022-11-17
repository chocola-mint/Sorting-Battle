using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using SortGame;

[CustomEditor(typeof(GameGrid))]
public class GameGridEditor : Editor {
    private void Bake(GameObject root)
    {
        GameGrid gameGrid = root.GetComponentInChildren<GameGrid>();
        root.transform.DestroyAllChildren();
        var group = gameGrid.GetComponent<HorizontalOrVerticalLayoutGroup>();
        GameGrid.InferSizeMode inferSizeMode = (GameGrid.InferSizeMode)
            serializedObject.FindProperty("inferSizeMode").enumValueIndex;
        GameObject rowPrefab = serializedObject.FindProperty("rowPrefab").objectReferenceValue as GameObject;
        GameObject tilePrefab = serializedObject.FindProperty("tilePrefab").objectReferenceValue as GameObject;
        
        float spacing = group.spacing;
        float fullSize = (inferSizeMode == GameGrid.InferSizeMode.UseWidth ? 
            gameGrid.GetComponent<RectTransform>().rect.width : 
            gameGrid.GetComponent<RectTransform>().rect.height);
        float size;
        if(inferSizeMode == GameGrid.InferSizeMode.UseWidth)
        {
            size = (fullSize - spacing * gameGrid.columnCount) / gameGrid.columnCount;
            gameGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                fullSize,
                size * gameGrid.rowCount + spacing * gameGrid.rowCount
            );
        }
        else
        {
            size = (fullSize - spacing * gameGrid.rowCount) / gameGrid.rowCount;
            gameGrid.GetComponent<RectTransform>().sizeDelta = new Vector2(
                size * gameGrid.columnCount + spacing * gameGrid.columnCount,
                fullSize
            );
        }
        for(int i = 0; i < gameGrid.rowCount; ++i)
        {
            var rowGO = PrefabUtility.InstantiatePrefab(rowPrefab) as GameObject;
            rowGO.name = $"Row {i}";
            rowGO.transform.SetParent(root.transform);
            var childGroup = rowGO.GetComponent<HorizontalOrVerticalLayoutGroup>();
            childGroup.spacing = spacing;
            for(int j = 0; j < gameGrid.columnCount; ++j)
            {
                var tileGO = PrefabUtility.InstantiatePrefab(tilePrefab) as GameObject;
                tileGO.transform.SetParent(rowGO.transform);
                tileGO.name = $"Tile {i},{j}";
                var tile = tileGO.transform as RectTransform;
                tile.sizeDelta = new Vector2(size, size);
                tile.pivot = Vector2.one * 0.5f;
            }
            // Force recompute of layout groups.
            childGroup.childForceExpandWidth ^= true;
            childGroup.childForceExpandWidth ^= true;
        }
    }
    public override void OnInspectorGUI() {
        if(GUILayout.Button("Bake"))
        {
            var gameGrid = target as GameGrid;
            var prefabStage = UnityEditor.SceneManagement.PrefabStageUtility.GetCurrentPrefabStage();
            if(prefabStage)
            {
                var assetPath = prefabStage.assetPath;
                Debug.Log(assetPath);
                Debug.Log("Bake as prefab");
                using(var editingScope = new PrefabUtility.EditPrefabContentsScope(assetPath))
                {
                    Bake(editingScope.prefabContentsRoot);
                }
            }
            else
            {
                Bake(gameGrid.gameObject);
                EditorSceneManager.MarkSceneDirty(EditorSceneManager.GetActiveScene());
            }
            
        }
        base.OnInspectorGUI();
        
    }
}