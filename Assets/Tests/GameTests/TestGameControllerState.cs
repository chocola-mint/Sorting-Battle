using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestGameControllerState
{
    [Test]
    public void TestIntegrationCase1()
    {
        GameGridState gameGridState = GameGridState.Deserialize(
            "-1, -1,  2, -1, -1\n" +
            "-1, -1,  6,  4, -1\n" +
            "-1,  1, 54,  7, -1\n" +
            " 5,  2, 14, 11, 66\n" +
            " 3,  4, 22, 12, 88\n");
        GameControllerState state = new(gameGridState, 3);
        state.StartSwapping(new(4, 3));
        state.SwapTo(new(4, 2));
        state.EndSwapping();
        Assert.IsTrue(
            gameGridState.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1,  2, -1, -1\n" +
                    "-1, -1,  6,  4, -1\n" +
                    "-1,  1, 54,  7, -1\n" +
                    " 5,  2, 14, 11, 66\n" +
                    " 3,  4, 12, 22, 88\n")));
        // Remove the bottommost row.
        state.BeginSelection();
        Assert.IsTrue(state.Select(new(4, 0)));
        Assert.IsTrue(state.Select(new(4, 1)));
        Assert.IsTrue(state.Select(new(4, 2)));
        Assert.IsTrue(state.Select(new(4, 3)));
        Assert.IsTrue(state.Select(new(4, 4)));
        var (selection, drops, shouldRemove) = state.EndSelection();
        Assert.IsTrue(shouldRemove);
        Assert.AreEqual(selection.Count, 5);
        Assert.Greater(drops.Count, 0);
        Assert.IsTrue(
            gameGridState.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1,  2, -1, -1\n" +
                    "-1, -1,  6,  4, -1\n" +
                    "-1,  1, 54,  7, -1\n" +
                    " 5,  2, 14, 11, 66\n")));
        
        // Try a failed removal here.
        state.BeginSelection();
        Assert.IsTrue(state.Select(new(2, 2)));
        Assert.IsTrue(state.Select(new(3, 2)));
        Assert.IsFalse(state.Select(new(4, 2)));
        (selection, drops, shouldRemove) = state.EndSelection();
        Assert.IsFalse(shouldRemove);
        Assert.AreEqual(selection.Count, 2);
        // Make sure the grid is unchanged.
        Assert.IsTrue(
            gameGridState.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1,  2, -1, -1\n" +
                    "-1, -1,  6,  4, -1\n" +
                    "-1,  1, 54,  7, -1\n" +
                    " 5,  2, 14, 11, 66\n")));

        // Redo the removal, making sure that it's clean.
        state.BeginSelection();
        Assert.IsTrue(state.Select(new(1, 2)));
        Assert.IsTrue(state.Select(new(2, 2)));
        Assert.IsTrue(state.Select(new(3, 2)));
        (selection, drops, shouldRemove) = state.EndSelection();
        Assert.IsTrue(shouldRemove);
        Assert.AreEqual(selection.Count, 3);
        Assert.AreEqual(drops.Count, 0);
        Assert.IsTrue(
            gameGridState.ContentEqual(
                GameGridState.Deserialize(
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1, -1, -1\n" +
                    "-1, -1, -1,  4, -1\n" +
                    "-1,  1, -1,  7, -1\n" +
                    " 5,  2, 14, 11, 66\n")));
    }
}
