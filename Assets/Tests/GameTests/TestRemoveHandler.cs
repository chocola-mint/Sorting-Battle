using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using SortGame.GameFunctions;
using static System.TupleExtensions;

public class TestRemoveHandler
{
    [Test]
    public void TestVerticalDecreasingSelectionRemoval()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1, -1, -1\n" +
            "-1,  4, -1, -1, -1\n");
        RemoveHandler handler = new(state);
        handler.BeginSelection();
        handler.Select(new(1, 1));
        handler.Select(new(2, 1));
        handler.Select(new(3, 1));
        handler.Select(new(4, 1));
        var (removedTiles, swaps) = handler.EndSelection();
        Assert.AreEqual(removedTiles.Count, 4);
        Assert.AreEqual(removedTiles[0], new Vector2Int(1, 1));
        Assert.AreEqual(removedTiles[1], new Vector2Int(2, 1));
        Assert.AreEqual(removedTiles[2], new Vector2Int(3, 1));
        Assert.AreEqual(removedTiles[3], new Vector2Int(4, 1));
        Assert.AreEqual(swaps.Count, 0);
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n")));
    }
}
