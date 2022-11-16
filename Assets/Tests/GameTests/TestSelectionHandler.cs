using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using SortGame.GameFunctions;
public class TestSelectionHandler
{
    [Test]
    public void TestVerticalDecreasingSelection()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1, -1, -1\n" +
            "-1,  4, -1, -1, -1\n"));
        handler.BeginSelection();
        handler.Select(new(1, 1));
        handler.Select(new(2, 1));
        handler.Select(new(3, 1));
        handler.Select(new(4, 1));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 4);
        Assert.AreEqual(selection[0], new Vector2Int(1, 1));
        Assert.AreEqual(selection[1], new Vector2Int(2, 1));
        Assert.AreEqual(selection[2], new Vector2Int(3, 1));
        Assert.AreEqual(selection[3], new Vector2Int(4, 1));
    }
    [Test]
    public void TestVerticalIncreasingSelection()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1,  6, -1\n" +
            "-1,  4,  5, 12, 13\n"));
        handler.BeginSelection();
        handler.Select(new(3, 1));
        handler.Select(new(3, 2));
        handler.Select(new(3, 3));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 1);
        Assert.AreEqual(selection[0], new Vector2Int(3, 1));
    }
    [Test]
    public void TestHorizontalIncreasingSelection()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            " 2,  6, -1, -1, -1\n" +
            "15,  5, -1,  6, -1\n" +
            " 1,  4,  5, 12, 13\n"));
        handler.BeginSelection();
        handler.Select(new(4, 0));
        handler.Select(new(4, 1));
        handler.Select(new(4, 2));
        handler.Select(new(4, 3));
        handler.Select(new(4, 4));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 5);
        Assert.AreEqual(selection[0], new Vector2Int(4, 0));
        Assert.AreEqual(selection[1], new Vector2Int(4, 1));
        Assert.AreEqual(selection[2], new Vector2Int(4, 2));
        Assert.AreEqual(selection[3], new Vector2Int(4, 3));
        Assert.AreEqual(selection[4], new Vector2Int(4, 4));
    }
    [Test]
    public void TestVerticalMonotoneSelection()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            " 2,  6, -1, -1, -1\n" +
            "15,  5, -1,  6, -1\n" +
            " 1,  4,  5, 12, 13\n"));
        handler.BeginSelection();
        handler.Select(new(4, 0));
        handler.Select(new(3, 0));
        handler.Select(new(2, 0));
        handler.Select(new(1, 0));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 2);
        Assert.AreEqual(selection[0], new Vector2Int(4, 0));
        Assert.AreEqual(selection[1], new Vector2Int(3, 0));
    }
    [Test]
    public void TestSingleDirection()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1, -1, -1,  1\n" +
            "-1,  7, -1, -1,  2\n" +
            " 2,  6, -1, -1,  3\n" +
            "15,  5, -1,  5,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        handler.BeginSelection();
        handler.Select(new(0, 4));
        handler.Select(new(1, 4));
        handler.Select(new(2, 4));
        handler.Select(new(3, 4));
        handler.Select(new(4, 4));
        handler.Select(new(4, 3));
        handler.Select(new(4, 2));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 5);
        Assert.AreEqual(selection[0], new Vector2Int(0, 4));
        Assert.AreEqual(selection[1], new Vector2Int(1, 4));
        Assert.AreEqual(selection[2], new Vector2Int(2, 4));
        Assert.AreEqual(selection[3], new Vector2Int(3, 4));
        Assert.AreEqual(selection[4], new Vector2Int(4, 4));
    }
    [Test]
    public void TestNoDiagonals()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1,  9, -1,  1\n" +
            "-1,  7,  8, -1,  2\n" +
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        handler.BeginSelection();
        handler.Select(new(2, 0));
        handler.Select(new(1, 1));
        handler.Select(new(0, 3));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 1);
        Assert.AreEqual(selection[0], new Vector2Int(2, 0));
    }
    [Test]
    public void TestNoGoingBack()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1,  9, -1,  1\n" +
            "-1,  7,  8, -1,  2\n" +
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        handler.BeginSelection();
        handler.Select(new(3, 1));
        handler.Select(new(3, 2));
        handler.Select(new(3, 0));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 2);
        Assert.AreEqual(selection[0], new Vector2Int(3, 1));
        Assert.AreEqual(selection[1], new Vector2Int(3, 2));
    }
    [Test]
    public void TestSameNumbers()
    {
        SelectionHandler handler = new(GameGridState.Deserialize(
            "-1, -1,  9, -1,  1\n" +
            "-1,  7,  8, -1,  2\n" +
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  5,  6,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        handler.BeginSelection();
        handler.Select(new(3, 1));
        handler.Select(new(3, 2));
        handler.Select(new(3, 3));
        handler.Select(new(3, 4));
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 3);
        Assert.AreEqual(selection[0], new Vector2Int(3, 1));
        Assert.AreEqual(selection[1], new Vector2Int(3, 2));
        Assert.AreEqual(selection[2], new Vector2Int(3, 3));
    }
    [Test]
    public void TestEmptySelection()
    {
        SelectionHandler handler = new(new(5, 5));
        handler.BeginSelection();
        var selection = handler.EndSelection();
        Assert.AreEqual(selection.Count, 0);
    }
}
