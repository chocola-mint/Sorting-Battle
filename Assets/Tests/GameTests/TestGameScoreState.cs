using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame.Core;

public class TestGameScoreState
{
    [Test]
    public void TestInitialState()
    {
        GameScoreState state = new(
            new(){
                minimumRemoveCount = 3,
                baseRemoveScore = 50,
                maxEffectiveCombo = 10,
                removeLengthBonus = 25,
                comboScoreStep = 2.0f,
            }
        );
        Assert.AreEqual(state.combo, 0);
        Assert.AreEqual(state.totalScore, 0);
        Assert.AreEqual(state.comboScoreBuffer, 0);
    }
    [Test]
    public void TestCombo()
    {
        GameScoreState state = new(
            new(){
                minimumRemoveCount = 3,
                baseRemoveScore = 50,
                maxEffectiveCombo = 10,
                removeLengthBonus = 25,
                comboScoreStep = 2.0f,
            }
        );
        // Build a 4x combo here.
        state.OnRemove(3);
        Assert.AreEqual(state.combo, 1);
        state.OnRemove(4);
        Assert.AreEqual(state.combo, 2);
        state.OnRemove(3);
        Assert.AreEqual(state.combo, 3);
        state.OnRemove(4);
        Assert.AreEqual(state.combo, 4);
        state.OnRemove(1); // End combo here.
        Assert.AreEqual(state.combo, 0);
    }
    [Test]
    public void TestScore()
    {
        // ! The scoring formula can be adjusted down the line. 
        // ! We only make sure that it's strictly increasing.
        GameScoreState state = new(
            new(){
                minimumRemoveCount = 3,
                baseRemoveScore = 50,
                maxEffectiveCombo = 10,
                removeLengthBonus = 25,
                comboScoreStep = 2.0f,
            }
        );
        // 15x combo here.
        for(int i = 0, prevScore = state.totalScore; i < 15; ++i)
        {
            state.OnRemove(3);
            Assert.Greater(state.totalScore, prevScore);
            prevScore = state.totalScore;
        }
    }
    [Test]
    public void TestComboBuffer()
    {
        // ! The scoring formula can be adjusted down the line. 
        // ! We only make sure that it's strictly increasing.
        GameScoreState state = new(
            new(){
                minimumRemoveCount = 3,
                baseRemoveScore = 50,
                maxEffectiveCombo = 10,
                removeLengthBonus = 25,
                comboScoreStep = 2.0f,
            }
        );
        // Build a 10x combo here.
        for(int i = 0; i < 10; ++i)
            state.OnRemove(4);
        Assert.AreEqual(state.combo, 10);
        Assert.AreEqual(state.totalScore, state.comboScoreBuffer);
        state.OnRemove(2); // End combo here.
        Assert.AreEqual(state.comboScoreBuffer, 0);
        // Obvious constraint: Total score must be greater than the combo score buffer
        // after the second combo starts.
        for(int i = 0; i < 3; ++i)
            state.OnRemove(3);
        Assert.Greater(state.totalScore, state.comboScoreBuffer);
    }
}
