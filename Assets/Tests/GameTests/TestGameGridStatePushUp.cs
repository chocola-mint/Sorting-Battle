using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestGameGridStatePushUp
{
    [Test]
    public void TestPushUpWithNoOverflow()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            " 1, -1, -1, -1, -1\n" +
            " 2,  5, -1, -1, -1\n" +
            " 3,  4, -1, -1, -1\n");
        var swaps = state.PushUp(0, 4);
        Assert.AreEqual(swaps.Count, 3);
        Assert.AreEqual(swaps[0].a, new Vector2Int(2, 0));
        Assert.AreEqual(swaps[0].b, new Vector2Int(1, 0));
        Assert.AreEqual(swaps[1].a, new Vector2Int(3, 0));
        Assert.AreEqual(swaps[1].b, new Vector2Int(2, 0));
        Assert.AreEqual(swaps[2].a, new Vector2Int(4, 0));
        Assert.AreEqual(swaps[2].b, new Vector2Int(3, 0));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    " 1, -1, -1, -1, -1\n" +
                    " 2, -1, -1, -1, -1\n" +
                    " 3,  5, -1, -1, -1\n" +
                    " 4,  4, -1, -1, -1\n")));
    }
    [Test]
    public void TestPushUpWithOverflow()
    {
        GameGridState state = GameGridState.Deserialize(
            " 1, -1, -1, -1, -1\n" +
            " 2, -1, -1, -1, -1\n" +
            " 3, -1, -1, -1, -1\n" +
            " 4,  5, -1, -1, -1\n" +
            " 5,  4, -1, -1, -1\n");
        var swaps = state.PushUp(0, 6);
        Assert.AreEqual(swaps.Count, 5);
        Assert.AreEqual(swaps[0].a, new Vector2Int(0, 0));
        Assert.AreEqual(swaps[0].b, new Vector2Int(-1, 0));
        Assert.AreEqual(swaps[1].a, new Vector2Int(1, 0));
        Assert.AreEqual(swaps[1].b, new Vector2Int(0, 0));
        Assert.AreEqual(swaps[2].a, new Vector2Int(2, 0));
        Assert.AreEqual(swaps[2].b, new Vector2Int(1, 0));
        Assert.AreEqual(swaps[3].a, new Vector2Int(3, 0));
        Assert.AreEqual(swaps[3].b, new Vector2Int(2, 0));
        Assert.AreEqual(swaps[4].a, new Vector2Int(4, 0));
        Assert.AreEqual(swaps[4].b, new Vector2Int(3, 0));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    " 2, -1, -1, -1, -1\n" +
                    " 3, -1, -1, -1, -1\n" +
                    " 4, -1, -1, -1, -1\n" +
                    " 5,  5, -1, -1, -1\n" +
                    " 6,  4, -1, -1, -1\n")));
    }
}
