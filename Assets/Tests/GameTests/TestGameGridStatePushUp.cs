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
        bool overflow = state.PushUp(0, 4);
        Assert.IsFalse(overflow);
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
        bool overflow = state.PushUp(0, 6);
        Assert.IsTrue(overflow);
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
