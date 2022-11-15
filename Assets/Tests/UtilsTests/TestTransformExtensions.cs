using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEditor;
using SortGame;
public class TestTransformExtensions
{
    [Test, Description("Use prefabs to test DestroyAllChildren. Paths are defined relative to the UtilsTest folder.")]
    [TestCase("TestPrefab.prefab"), Timeout(1000)]
    public void TestDestroyAllChildren(string prefabPath)
    {
        var testPrefab = AssetDatabase.LoadAssetAtPath<GameObject>($"Assets/Tests/UtilsTests/{prefabPath}");
        var testInstance = GameObject.Instantiate(testPrefab);
        testInstance.transform.DestroyAllChildren();
        Assert.AreEqual(testInstance.transform.childCount, 0);
    }

}
