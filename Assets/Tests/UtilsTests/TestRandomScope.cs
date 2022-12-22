using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestRandomScope : IRandomThread
{
    public Random.State randomState { get; set; }
    private Random.State threadRandom => randomState;
    private Random.State globalRandom => Random.state;
    [SetUp]
    public void Setup() 
    {
        randomState = Random.state;
    }
    [Test]
    public void TestGlobalRandomStatePreserved()
    {
        var stateBefore = globalRandom;
        using(new RandomScope(this)) {
            Debug.Log(Random.value);
            Debug.Log(Random.value);
            Debug.Log(Random.value);
        };
        var stateAfter = globalRandom;
        Assert.AreEqual(stateBefore, stateAfter);
    }
    [Test]
    public void TestThreadRandomStateAdvanced()
    {
        var stateBefore = threadRandom;
        using(new RandomScope(this)) 
        {
            Debug.Log(Random.value);
            Debug.Log(Random.value);
            Debug.Log(Random.value);
        };
        var stateAfter = threadRandom;
        Assert.AreNotEqual(stateBefore, stateAfter);
    }
}
