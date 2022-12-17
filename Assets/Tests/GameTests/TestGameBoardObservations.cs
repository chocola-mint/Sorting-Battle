using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
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
}
