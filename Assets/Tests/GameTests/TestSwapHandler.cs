using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using SortGame.GameFunctions;

public class TestSwapHandler
{
    [Test]
    public void TestSimpleSwap()
    {
        GameGridState state = GameGridState.Deserialize(
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  1\n");
        SwapHandler handler = new(state);
        Assert.IsTrue(handler.StartSwapping(new(2, 0)));
        Assert.IsTrue(handler.swappingActive);
        handler.SwapTo(new(2, 1));
        Assert.IsTrue(
            state.ContentEqual(GameGridState.Deserialize(
                " 2,  6,  3, -1,  3\n" +
                "15,  5,  2,  5,  4\n" +
                " 4,  1,  5,  6,  1\n")));
        handler.EndSwapping();
        Assert.IsFalse(handler.swappingActive);
    }
    [Test]
    public void TestStartSwappingEmpty()
    {
        GameGridState state = GameGridState.Deserialize(
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  1\n");
        SwapHandler handler = new(state);
        Assert.IsFalse(handler.StartSwapping(new(0, 3)));
        Assert.IsTrue(
            state.ContentEqual(GameGridState.Deserialize(
                " 2,  6,  3, -1,  3\n" +
                "15,  5,  2,  5,  4\n" +
                " 1,  4,  5,  6,  1\n")));
    }
    [Test]
    public void TestSwapToNonAdjacent()
    {
        GameGridState state = GameGridState.Deserialize(
            " 2,  6,  3, -1,  3\n" +
            "15,  5,  2,  5,  4\n" +
            " 1,  4,  5,  6,  1\n");
        SwapHandler handler = new(state);
        Assert.IsTrue(handler.StartSwapping(new(0, 0)));
        handler.SwapTo(new(2, 0));
        Assert.IsTrue(
            state.ContentEqual(GameGridState.Deserialize(
                " 2,  6,  3, -1,  3\n" +
                "15,  5,  2,  5,  4\n" +
                " 1,  4,  5,  6,  1\n")));
    }
    
    // ! TESTS DEPRECATED
    // ! To simplify the game, swapping a number into an empty tile is not allowed.
    // ! This means drops won't occur as a direct consequence of a swap.

    // [Test]
    // public void TestSwapsWithDrops()
    // {
    //     GameGridState state = GameGridState.Deserialize(
    //         " 2,  6,  3, -1,  3\n" +
    //         "15,  5,  2, -1,  4\n" +
    //         " 1,  4,  5,  6,  1\n");
    //     SwapHandler handler = new(state);
    //     Assert.IsTrue(handler.StartSwapping(new(1, 4)));
    //     Commands cmds;
    //     cmds = handler.SwapTo(new(1, 3));
    //     Assert.IsTrue(cmds.success);
    //     Assert.IsTrue(cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(1, 4, 1, 3));
    //     Assert.IsNotNull(cmds.drops);
    //     Assert.AreEqual(cmds.drops.Count, 1);
    //     Assert.AreEqual(cmds.drops[0].a, new Vector2Int(0, 4));
    //     Assert.AreEqual(cmds.drops[0].b, new Vector2Int(1, 4));
    //     Assert.IsTrue(
    //         state.ContentEqual(GameGridState.Deserialize(
    //             " 2,  6,  3, -1, -1\n" +
    //             "15,  5,  2,  4,  3\n" +
    //             " 1,  4,  5,  6,  1\n")));
    // }
    // [Test]
    // public void TestMultipleConsecutiveSwaps()
    // {
    //     GameGridState state = GameGridState.Deserialize(
    //         " 2,  6,  3, -1, -1\n" +
    //         "15,  5,  2, -1, -1\n" +
    //         " 1,  4,  5,  6, -1\n");
    //     SwapHandler handler = new(state);
    //     Commands cmds;
    //     Assert.IsTrue(handler.StartSwapping(new(1, 2)));
    //     cmds = handler.SwapTo(new(1, 3));
    //     Assert.IsTrue(cmds.success && cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(1, 2, 1, 3));
    //     cmds = handler.SwapTo(new(2, 3));
    //     Assert.IsTrue(cmds.success && !cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(1, 3, 2, 3));
    //     cmds = handler.SwapTo(new(2, 4));
    //     Assert.IsTrue(cmds.success && cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(2, 3, 2, 4));
    //     Assert.IsTrue(
    //         state.ContentEqual(GameGridState.Deserialize(
    //             " 2,  6, -1, -1, -1\n" +
    //             "15,  5,  3, -1, -1\n" +
    //             " 1,  4,  5,  6,  2\n")));
    // }
    // [Test]
    // public void TestMultipleConsecutiveSwapsWithInterruption()
    // {
    //     GameGridState state = GameGridState.Deserialize(
    //         " 2,  6,  3, -1, -1\n" +
    //         "15,  5,  2, -1, -1\n" +
    //         " 1,  4,  5,  6, -1\n");
    //     SwapHandler handler = new(state);
    //     Commands cmds;
    //     Assert.IsTrue(handler.StartSwapping(new(0, 2)));
    //     cmds = handler.SwapTo(new(0, 3));
    //     Assert.IsTrue(cmds.success && cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(0, 2, 0, 3));
    //     Assert.IsFalse(handler.swappingActive);
    //     Assert.IsTrue(handler.StartSwapping(new(1, 3)));
    //     cmds = handler.SwapTo(new(2, 3));
    //     Assert.IsTrue(cmds.success && !cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(1, 3, 2, 3));
    //     cmds = handler.SwapTo(new(2, 4));
    //     Assert.IsTrue(cmds.success && cmds.shouldFall);
    //     Assert.AreEqual(cmds.swap, MakeSwapOp(2, 3, 2, 4));
    //     Assert.IsTrue(
    //         state.ContentEqual(GameGridState.Deserialize(
    //             " 2,  6, -1, -1, -1\n" +
    //             "15,  5,  2, -1, -1\n" +
    //             " 1,  4,  5,  6,  3\n")));
    // }
}
