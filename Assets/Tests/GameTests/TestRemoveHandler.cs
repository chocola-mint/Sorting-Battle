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
        var (numberCount, trashCount, shouldRemove) = handler.EndSelection();
        Assert.IsTrue(shouldRemove);
        Assert.AreEqual(4, numberCount);
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n")));
    }
    [Test]
    public void TestRemoveTrash()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -2\n" +
            "-1,  6, -2, -1,  9\n" +
            "-2,  5,  6,  7,  8\n" +
            "-2,  4, -2, -2, -2\n");
        RemoveHandler handler = new(state);
        handler.BeginSelection();
        handler.Select(new(3, 1));
        handler.Select(new(3, 2));
        handler.Select(new(3, 3));
        handler.Select(new(3, 4));
        var (numberCount, trashCount, shouldRemove) = handler.EndSelection();
        Assert.IsTrue(shouldRemove);
        Assert.AreEqual(4, numberCount);
        Assert.AreEqual(5, trashCount);
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  7, -1, -1, -1\n" +
                    "-1,  6, -1, -1, -2\n" +
                    "-2,  4, -1, -1,  9\n")));
    }
}
