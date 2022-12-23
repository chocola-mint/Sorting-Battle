using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame.Core;

public class TestGamePressureState
{
    [Test]
    public void TestAddPressure()
    {
        GamePressureState p1 = new(100);
        Assert.AreEqual(0, p1.pressure);
        p1.AddPressure(10);
        Assert.AreEqual(10, p1.pressure);
        p1.AddPressure(20);
        Assert.AreEqual(30, p1.pressure);
        p1.AddPressure(100);
        Assert.AreEqual(100, p1.pressure);
    }
    [Test]
    public void TestPressureRate()
    {
        const double Epsilon = 0.001;
        GamePressureState p1 = new(100);
        Assert.AreEqual(0, p1.pressureRate, Epsilon);
        p1.AddPressure(15);
        Assert.AreEqual(0.15f, p1.pressureRate, Epsilon);
        p1.AddPressure(25);
        Assert.AreEqual(0.4f, p1.pressureRate, Epsilon);
        p1.AddPressure(100);
        Assert.AreEqual(1f, p1.pressureRate, Epsilon);
    }
    [Test]
    public void TestConsumePressure()
    {
        GamePressureState p1 = new(100);
        Assert.AreEqual(0, p1.ConsumePressure(10));
        p1.AddPressure(50);
        // Remove 30 pressure from a total of 50 pressure.
        Assert.AreEqual(30, p1.ConsumePressure(30));
        Assert.AreEqual(20, p1.pressure);
        // Only 20 pressure left, so ConsumePressure should return 20 rather than 70.
        Assert.AreEqual(20, p1.ConsumePressure(70)); 
    }
    [Test]
    public void TestAttack()
    {
        GamePressureState p1 = new(100), p2 = new(100);
        p1.AddPressure(50);
        p2.AddPressure(50);
        // Because P1 has 50 pressure, the 25 attack power will be used to reduce its own pressure.
        p1.Attack(p2, 25);
        Assert.AreEqual(25, p1.pressure);
        Assert.AreEqual(50, p2.pressure); // No additional pressure on P2.
        // Here, after removing the remaining 25 pressure from P1, the remaining 5 attack power will be used on P2.
        p1.Attack(p2, 30);
        Assert.AreEqual(0, p1.pressure);
        Assert.AreEqual(55, p2.pressure);
        // And finally, this maximizes P2's pressure.
        p1.Attack(p2, 70);
        Assert.AreEqual(0, p1.pressure);
        Assert.AreEqual(100, p2.pressure);
    }
}
