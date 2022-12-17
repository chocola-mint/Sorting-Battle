using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using System.Linq;
public class TestGameBoardObservations
{
    [Test]
    public void TestGetAllTiles()
    {
        GameBoardState state = new(new(){
            rowCount = 3,
            columnCount = 5,
        });
        var actual = state.GetAllTiles();
        Assert.That(actual, Is.EquivalentTo(new Vector2Int[]{
            new(0, 0), new(0, 1), new(0, 2), new(0, 3), new(0, 4),
            new(1, 0), new(1, 1), new(1, 2), new(1, 3), new(1, 4),
            new(2, 0), new(2, 1), new(2, 2), new(2, 3), new(2, 4),
        }));
    }
    private void AssertUnorderedEquivalence<T>(List<T> actual, List<T> expected)
    {
        foreach(var a in actual) Assert.IsTrue(expected.Contains(a), $"Expected answer does not contain {a}");
        foreach(var e in expected) Assert.IsTrue(actual.Contains(e), $"Actual answer contains {e}, which does not exist in expected answer.");
    }
    [Test]
    public void TestGetAllSelectableTilesAllNumbers()
    {
        GameBoardState state = new(new(){
            rowCount = 5,
            columnCount = 5,
        });
        state.gameGridState.InplaceCopy(GameGridState.Deserialize(
            " 6,  5,  9,  4,  1\n" +
            " 7,  7,  8,  3,  2\n" +
            " 2,  6,  3,  2,  3\n" +
            "15,  5,  5,  6,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        // Base case: All tiles are selectable here, because they are all numbers.
        Assert.That(state.GetAllSelectableTiles(), Is.EquivalentTo(state.GetAllTiles()));
        // And then comes the actual cases (based on TestSelectionHandler).
        state.gameControllerState.BeginSelection();
        state.gameControllerState.Select(new(3, 1));
        AssertUnorderedEquivalence(
            state.GetAllSelectableTiles(),
            new List<Vector2Int>(){
                new(2, 1),
                new(3, 0), new(3, 2),
                new(4, 1),
            });
        state.gameControllerState.Select(new(3, 2));
        AssertUnorderedEquivalence(
            state.GetAllSelectableTiles(),
            new List<Vector2Int>(){
                new(3, 3),
            });
        state.gameControllerState.Select(new(3, 3));
        Assert.AreEqual(0, state.GetAllSelectableTiles().Count);
        var actual = state.GetAllSelectableTiles();
    }
    [Test]
    public void TestGetAllSwappableTilesAllNumbers()
    {
        GameBoardState state = new(new(){
            rowCount = 5,
            columnCount = 5,
        });
        state.gameGridState.InplaceCopy(GameGridState.Deserialize(
            " 6,  5,  9,  4,  1\n" +
            " 7,  7,  8,  3,  2\n" +
            " 2,  6,  3,  2,  3\n" +
            "15,  5,  5,  6,  4\n" +
            " 1,  4,  5,  6,  5\n"));
        // Base case: All tiles are selectable here, because they are all numbers.
        Assert.That(state.GetAllSwappableTiles(), Is.EquivalentTo(state.GetAllTiles()));
        state.gameControllerState.StartSwapping(new(0, 0));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(0, 1), new(1, 0),
            });
        state.gameControllerState.SwapTo(new(1, 0));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(0, 0), new(2, 0), new(1, 1),
            });
        state.gameControllerState.SwapTo(new(1, 1));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(0, 1),
                new(1, 0), new(1, 2), 
                new(2, 1),
            });
    }
    [Test]
    public void TestGetAllSwappableTilesSomeTrash()
    {
        GameBoardState state = new(new(){
            rowCount = 5,
            columnCount = 5,
        });
        state.gameGridState.InplaceCopy(GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            "15,  5,  5,  6, -1\n" +
            " 1,  4, -2,  6,  5\n"));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(3, 0), new(3, 1), new(3, 2), new(3, 3),
                new(4, 0), new(4, 1),            new(4, 3), new(4, 4),
            });
        state.gameControllerState.StartSwapping(new(4, 1));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(4, 0), new(3, 1),
            });
        state.gameControllerState.SwapTo(new(3, 1));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(3, 0), new(4, 1), new(3, 2),
            });
        state.gameControllerState.SwapTo(new(3, 2));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(3, 1), new(3, 3), 
            });
        state.gameControllerState.SwapTo(new(3, 3));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(3, 2), new(4, 3), 
            });
        state.gameControllerState.SwapTo(new(4, 3));
        AssertUnorderedEquivalence(
            state.GetAllSwappableTiles(),
            new List<Vector2Int>(){
                new(3, 3),
                new(4, 4), 
            });
    }
}
