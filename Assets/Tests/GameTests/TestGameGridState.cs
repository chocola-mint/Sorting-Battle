using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using System.Text;
public class TestGameGridState
{
    // A Test behaves as an ordinary method
    [Test]
    [Pairwise]
    public void TestMinimalConstructor(
        [Values(2, 3, 4, 5)] int rowCount, 
        [Values(3, 4, 5, 6)] int columnCount)
    {
        GameGridState state = new(rowCount, columnCount);
        Assert.IsTrue(state.rowCount == rowCount);
        Assert.IsTrue(state.columnCount == columnCount);
        for(int i = 0; i < rowCount; ++i)
            for(int j = 0; j < columnCount; ++j)
                Assert.IsTrue(state.Get(new(i, j)) < 0);
    }
    private int[,] CreateRandomMatrix(int rowCount, int columnCount)
    {
        int[,] matrix = new int[rowCount, columnCount];
        for(int i = 0; i < rowCount; ++i)
            for(int j = 0; j < columnCount; ++j)
            {
                int rand = Random.Range(0, 100);
                matrix[i, j] = rand;
            }
        return matrix;
    }
    [Test]
    [Pairwise]
    public void TestGetSet(
        [Values(2, 3, 4, 5)] int rowCount, 
        [Values(3, 4, 5, 6)] int columnCount)
    {
        GameGridState state = new(rowCount, columnCount);
        int[,] answer = CreateRandomMatrix(rowCount, columnCount);
        for(int i = 0; i < rowCount; ++i)
            for(int j = 0; j < columnCount; ++j)
                state.Set(new(i, j), answer[i, j]);
        for(int i = 0; i < rowCount; ++i)
            for(int j = 0; j < columnCount; ++j)
                Assert.AreEqual(answer[i, j], state.Get(new(i, j)));
    }

    [Test]
    [Pairwise]
    public void TestCopy(
        [Values(3, 4)] int rowCount, 
        [Values(4, 5)] int columnCount)
    {
        GameGridState a = new(rowCount, columnCount);
        GameGridState b = new(rowCount, columnCount);
        b.LoadRandom();
        a.InplaceCopy(b);
        Assert.True(a.ContentEqual(b));
        b.LoadRandom();
        GameGridState c = new(b);
        Assert.True(c.ContentEqual(b));
    }
    [Test]
    [Pairwise]
    public void TestSerialization(
        [Values(3, 4)] int rowCount, 
        [Values(4, 5)] int columnCount)
    {
        GameGridState a = new(rowCount, columnCount);
        a.LoadRandom();
        var b = GameGridState.Deserialize(a.Serialize());
        Assert.IsTrue(a.ContentEqual(b));
    }
    [Test]
    [Pairwise]
    public void TestClear(
        [Values(3, 4)] int rowCount, 
        [Values(4, 5)] int columnCount)
    {
        GameGridState state = new(rowCount, columnCount);
        state.LoadRandom();
        state.Clear();
        for(int i = 0; i < rowCount; ++i)
            for(int j = 0; j < columnCount; ++j)
                Assert.IsTrue(state.Get(new(i, j)) < 0);
    }
    [Test]
    [Pairwise]
    public void TestSwap(
        [Values(0, 1, 2)] int x1,
        [Values(0, 1, 2)] int y1,
        [Values(0, 1, 2)] int x2,
        [Values(0, 1, 2)] int y2
    )
    {
        GameGridState state = new(3, 3);
        state.LoadRandom();
        Vector2Int a = new(x1, y1), b = new(x2, y2);
        int aVal = state.Get(a), bVal = state.Get(b);
        state.Swap(a, b);
        Assert.AreEqual(aVal, state.Get(b));
        Assert.AreEqual(bVal, state.Get(a));
    }

    [Test]
    public void TestPullDownCase1()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1,  5, -1,  5\n" +
            "-1, -1,  4, -1,  5\n" +
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n".StripWhiteSpaces());
        var swaps = state.PullDown(2);
        Assert.AreEqual(swaps.Count, 2);
        Assert.IsTrue(swaps[0].a == new Vector2Int(2, 2));
        Assert.IsTrue(swaps[0].b == new Vector2Int(4, 2));
        Assert.IsTrue(swaps[1].a == new Vector2Int(1, 2));
        Assert.IsTrue(swaps[1].b == new Vector2Int(3, 2));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1,  5\n" +
                    "-1, -1, -1, -1,  5\n" +
                    "-1, -1,  5, -1, -1\n" +
                    "-1, -1,  4, -1, -1\n".StripWhiteSpaces())));
    }
    [Test]
    public void TestPullDownCase2()
    {
        GameGridState state = new(5, 5);
        var swaps = state.PullDown(1);
        Assert.AreEqual(swaps.Count, 0);
        Assert.IsTrue(state.ContentEqual(new(5, 5)));
    }
    [Test]
    public void TestPullDownCase3()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            "-1,  1, -1, -1, -1\n".StripWhiteSpaces());
        var swaps = state.PullDown(1);
        Assert.AreEqual(swaps.Count, 2);
        Assert.IsTrue(swaps[0].a == new Vector2Int(1, 1));
        Assert.IsTrue(swaps[0].b == new Vector2Int(3, 1));
        Assert.IsTrue(swaps[1].a == new Vector2Int(0, 1));
        Assert.IsTrue(swaps[1].b == new Vector2Int(2, 1));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  6, -1, -1, -1\n" +
                    "-1,  5, -1, -1, -1\n" +
                    "-1,  1, -1, -1, -1\n".StripWhiteSpaces())));
    }

}
