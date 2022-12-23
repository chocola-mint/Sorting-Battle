using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame.Core;

public class TestGameState : GameState
{
    [TearDown]
    public void Reset()
    {
        // We invoke GameOver to clear the scheduler.
        GameOver();
    }
    // ! We don't test PushNewRowEvent here. It won't matter as long as we don't invoke InitEvents.
    protected override void PushNewRowEvent()
    {
        throw new System.NotImplementedException();
    }
    [Test]
    public void TestSingleEvent()
    {
        bool eventProcessed = false;
        PushEvent(0, () => {
            eventProcessed = true;
        });
        Tick(1);
        Assert.IsTrue(eventProcessed);
    }
    [Test]
    public void TestIllegalTick()
    {
        PushEvent(0, () => Assert.Fail());
        Assert.Throws<System.ArgumentException>(() => Tick(-1));
        Assert.Throws<System.ArgumentException>(() => Tick(0));
        Assert.Throws<System.ArgumentException>(() => Tick(-10));
    }
    [Test]
    public void TestEventSequence()
    {
        List<int> answer = new();
        for(int i = 0; i < 10; ++i)
            for(int j = 0; j < 3; ++j)
            {
                int k = i;
                PushEvent(i, () => answer.Add(k));
            }
        Tick(30);
        Assert.That(answer, Is.EquivalentTo(new int[]{
            0, 0, 0,
            1, 1, 1,
            2, 2, 2,
            3, 3, 3,
            4, 4, 4,
            5, 5, 5,
            6, 6, 6,
            7, 7, 7,
            8, 8, 8,
            9, 9, 9,
        }));
    }
    [Test]
    public void TestGameOver()
    {
        bool gameOverOccurred = false;
        onGameOver += () => {
            gameOverOccurred = true;
        };
        PushEvent(10, GameOver);
        Tick(10);
        Assert.IsTrue(gameOverOccurred);
    }
    [Test]
    public void TestSkipToNextEvent()
    {
        bool firstCallbackExecuted = false;
        bool secondCallbackExecuted = false;
        PushEvent(1337, () => firstCallbackExecuted = true);
        PushEvent(1337, () => secondCallbackExecuted = true);
        PushEvent(1338, () => {
            Assert.Fail();
        });
        SkipToNextEvent();
        Assert.IsTrue(firstCallbackExecuted);
        Assert.IsTrue(secondCallbackExecuted);
    }
    [Test]
    public void TestEventLoop()
    {
        int counter = 0;
        System.Action callback = null;
        callback = () => {
            counter++;
            PushEvent(5, callback);
        };
        PushEvent(5, callback);
        Tick(2);
        Assert.AreEqual(0, counter);
        Tick(3);
        Assert.AreEqual(1, counter);
        Tick(10);
        Assert.AreEqual(3, counter);
        Tick(20);
        Assert.AreEqual(7, counter);
    }
}
