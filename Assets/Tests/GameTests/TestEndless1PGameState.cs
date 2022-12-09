using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestEndless1PGameState : Endless1PGameState
{
    // ! Note: This test suite only checks basic assumptions (no qualitative tests).
    // ! Although the seed used during tests is fixed, it is not a hard requirement to 
    // ! use the same seed or same RNG algorithm.
    public TestEndless1PGameState() : base(
        p1GBState: new GameBoardState(
            new GameBoardState.Config(){
                useSeed = true,
                seed = 42,
                rowCount = 10,
                columnCount = 5,
                numberUpperBound = 10,
                minimumSortedLength = 3,
                baseRemoveScore = 50,
                removeLengthBonus = 25,
                maxEffectiveCombo = 10,
                comboScoreStep = 2,
            }
        ), 
        emptyRowPercentage: 0.8f
    ) {}
    [SetUp]
    public void BeforeEachTest()
    {
        GameOver();
        InitEvents();
    }
    [TearDown]
    public void AfterEachTest()
    {
        GameOver();
    }
    [Test]
    public void TestGridLoaded()
    {
        var gridState = p1GBState.gameGridState;
        // Check if board IS empty.
        for(int i = 0; i < gridState.rowCount; ++i)
            for(int j = 0; j < gridState.columnCount; ++j)
                Assert.IsTrue(gridState.IsEmpty(new(i, j)));
        Tick(1);
        // Check that grid is loaded.
        for(int i = 0; i < gridState.rowCount; ++i)
            // Loaded means these rows are empty.
            if(i < Mathf.FloorToInt(gridState.rowCount * emptyRowPercentage))
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsEmpty(new(i, j)));
            else // Loaded means these rows are loaded with numbers.
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsNumber(new(i, j)));
    }
    [Test]
    public void TestPushNewRowEvent()
    {
        var gridState = p1GBState.gameGridState;
        // This should make the grid loaded, and trigger the first PushNewRowEvent.
        Tick(GetTickBetweenEachNewRow());
        for(int i = 0; i < gridState.rowCount; ++i)
            // These rows should be empty.
            if(i < Mathf.FloorToInt(gridState.rowCount * emptyRowPercentage) - 1)
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsEmpty(new(i, j)));
            // This row should contain exactly one empty tile.
            else if(i == Mathf.FloorToInt(gridState.rowCount * emptyRowPercentage) - 1)
            {
                int emptyCount = 0;
                for(int j = 0; j < gridState.columnCount; ++j)
                {
                    Assert.IsTrue(gridState.IsEmpty(new(i, j)) || gridState.IsNumber(new(i, j)));
                    if(gridState.IsEmpty(new(i, j))) emptyCount++;
                }
                Assert.AreEqual(1, emptyCount);
            }
            else // These rows should all be numbers.
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsNumber(new(i, j)));
        
        // We still have to check the second PushNewRowEvent.
        Tick(GetTickBetweenEachNewRow());
        for(int i = 0; i < gridState.rowCount; ++i)
            // These rows should be empty.
            if(i < Mathf.FloorToInt(gridState.rowCount * emptyRowPercentage) - 2)
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsEmpty(new(i, j)));
            // These rows should contain no more than two empty tiles.
            else if(i < Mathf.FloorToInt(gridState.rowCount * emptyRowPercentage))
            {
                int emptyCount = 0;
                for(int j = 0; j < gridState.columnCount; ++j)
                {
                    Assert.IsTrue(gridState.IsEmpty(new(i, j)) || gridState.IsNumber(new(i, j)));
                    if(gridState.IsEmpty(new(i, j))) emptyCount++;
                }
                Assert.LessOrEqual(emptyCount, 2);
            }
            else // These rows should all be numbers.
                for(int j = 0; j < gridState.columnCount; ++j)
                    Assert.IsTrue(gridState.IsNumber(new(i, j)));
        
    }
    [Test]
    public void TestGameOver()
    {
        int gameOverExecutedCount = 0;
        System.Action gameOver = () => {
            gameOverExecutedCount++;
        };
        onGameOver += gameOver;
        var gridState = p1GBState.gameGridState;
        // First tick. Obviously the game shouldn't be over here.
        Tick(1);
        Assert.AreEqual(0, gameOverExecutedCount);
        // Because GetTickBetweenEachNewRow will decrease as level increases, 
        // this is an overestimation of the number of ticks needed to end a game
        // (assuming the player does nothing).
        Tick(GetTickBetweenEachNewRow() * Mathf.CeilToInt(gridState.rowCount * emptyRowPercentage * 2));
        onGameOver -= gameOver;
        Assert.AreEqual(1, gameOverExecutedCount);
    }
}
