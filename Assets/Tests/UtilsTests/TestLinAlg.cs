using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestLinAlg
{
    [Test]
    [TestCase(0, 0, 1, 1, ExpectedResult=2)]
    [TestCase(-1, -1, 1, 1, ExpectedResult=4)]
    [TestCase(-3, -3, 0, 0, ExpectedResult=6)]
    [TestCase(-3, -3, -2, -2, ExpectedResult=2)]
    [TestCase(-2, -2, -2, -2, ExpectedResult=0)]
    public int TestL1Norm(int x1, int y1, int x2, int y2)
    {
        return LinAlg.L1Norm(new Vector2Int(x1, y1), new Vector2Int(x2, y2));
    }
    [Test]
    [TestCase(0 - 0.1f, 0 - 0.1f, 1 - 0.1f, 1 - 0.1f, 2)]
    [TestCase(-1 + 0.1f, -1 + 0.1f, 1 + 0.1f, 1 + 0.1f, 4)]
    [TestCase(-3 - 0.1f, -3 - 0.1f, 0 - 0.1f, 0 - 0.1f, 6)]
    [TestCase(-3 + 0.1f, -3 + 0.1f, -2 + 0.1f, -2 + 0.1f, 2)]
    [TestCase(-2 + 0.1f, -2 + 0.1f, -2 + 0.1f, -2 + 0.1f, 0)]
    public void TestL1NormFloat(float x1, float y1, float x2, float y2, float expected)
    {
        Assert.IsTrue(
            Mathf.Approximately(
                LinAlg.L1Norm(new Vector2(x1, y1), new Vector2(x2, y2)), 
                expected));
    }
}
