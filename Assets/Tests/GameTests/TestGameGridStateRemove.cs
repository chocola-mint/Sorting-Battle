using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame.Core;

public class TestGameGridStateRemove
{
    [Test]
    public void TestRemoveTileCase1()
    {
        GameGridState state = GameGridState.Deserialize(
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  1\n");
        state.RemoveTile(new(2, 0));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1,  6,  3, -1,  3\n" +
                    " 2,  5,  2,  5,  4\n" +
                    "15,  4,  5,  6,  1\n")));
        state.RemoveTile(new(2, 3));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1,  6,  3, -1,  3\n" +
                    " 2,  5,  2, -1,  4\n" +
                    "15,  4,  5,  5,  1\n")));
        state.RemoveTile(new(0, 1));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1,  3, -1,  3\n" +
                    " 2,  5,  2, -1,  4\n" +
                    "15,  4,  5,  5,  1\n")));
        state.RemoveTile(new(0, 0));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1,  3, -1,  3\n" +
                    " 2,  5,  2, -1,  4\n" +
                    "15,  4,  5,  5,  1\n")));
    }
    [Test]
    public void TestRemoveTilesCase1()
    {
        GameGridState state = GameGridState.Deserialize(
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  1\n");
        state.RemoveTiles(new Vector2Int[]{new(2, 0), new(2, 1), new(2, 2), new(2, 3)});
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1,  3\n" +
                    " 2,  6,  3, -1,  4\n" +
                    "15,  5,  2,  5,  1\n")));
        state.RemoveTiles(new Vector2Int[]{new(2, 1), new(1, 1)});
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1,  3\n" +
                    " 2, -1,  3, -1,  4\n" +
                    "15, -1,  2,  5,  1\n")));
    }

}
