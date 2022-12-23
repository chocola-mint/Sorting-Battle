using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame.Core;

public class TestGameGridStatePullDown
{
    [Test]
    public void TestPullDownCase1()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1,  5, -1,  5\n" +
            "-1, -1,  4, -1,  5\n" +
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n");
        state.PullDown(2);
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1,  5\n" +
                    "-1, -1, -1, -1,  5\n" +
                    "-1, -1,  5, -1, -1\n" +
                    "-1, -1,  4, -1, -1\n")));
    }
    [Test]
    public void TestPullDownCase2()
    {
        GameGridState state = new(5, 5);
        state.PullDown(1);
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
            "-1,  1, -1, -1, -1\n");
        state.PullDown(1);
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  6, -1, -1, -1\n" +
                    "-1,  5, -1, -1, -1\n" +
                    "-1,  1, -1, -1, -1\n")));
    }

    [Test]
    public void TestSwapAndPullDownCase1()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1, -1, -1\n" +
            "-1,  4, -1, -1, -1\n");
        state.SwapAndPullDown(new(3, 1), new(3, 2));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  7, -1, -1, -1\n" +
                    "-1,  6, -1, -1, -1\n" +
                    "-1,  4,  5, -1, -1\n")));
    }
    [Test]
    public void TestSwapAndPullDownCase2()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1,  7, -1, -1, -1\n" +
            "-1,  6, -1, -1, -1\n" +
            "-1,  5, -1, -1, -1\n" +
            "-1,  4, -1, -1, -1\n");
        state.SwapAndPullDown(new(1, 1), new(1, 2));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  6, -1, -1, -1\n" +
                    "-1,  5, -1, -1, -1\n" +
                    "-1,  4,  7, -1, -1\n")));
    }
    [Test]
    public void TestSwapAndPullDownCase3()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            " 1, -1, -1, -1, -1\n" +
            " 2,  5, -1, -1, -1\n" +
            " 3,  4, -1, -1, -1\n");
        state.SwapAndPullDown(new(3, 0), new(3, 1));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    " 1, -1, -1, -1, -1\n" +
                    " 5,  2, -1, -1, -1\n" +
                    " 3,  4, -1, -1, -1\n")));
        state.SwapAndPullDown(new(2, 0), new(2, 1));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1,  1, -1, -1, -1\n" +
                    " 5,  2, -1, -1, -1\n" +
                    " 3,  4, -1, -1, -1\n")));
    }
    [Test]
    public void TestSwapAndPullDownCase4()
    {
        GameGridState state = GameGridState.Deserialize(
            "-1, -1, -1, -1, -1\n" +
            "-1, -1, -1, -1, -1\n" +
            " 1, -1, -1, -1, -1\n" +
            " 2,  5, -1, -1, -1\n" +
            " 3,  4, -1, -1, -1\n");
        state.SwapAndPullDown(new(4, 2), new(4, 3));
        Assert.IsTrue(
            state.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    " 1, -1, -1, -1, -1\n" +
                    " 2,  5, -1, -1, -1\n" +
                    " 3,  4, -1, -1, -1\n")));
    }
}
