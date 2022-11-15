using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
public class TestStringExtensions
{
    // A Test behaves as an ordinary method
    [Test]
    [TestCase("", ExpectedResult="")]
    [TestCase("ABC", ExpectedResult="ABC")]
    [TestCase("ABC CDE", ExpectedResult="ABCCDE")]
    [TestCase("0.124, 30", ExpectedResult="0.124,30")]
    [TestCase("0.124, 30 , 5235, 222", ExpectedResult="0.124,30,5235,222")]
    [TestCase("0.124, 30 , 5235, 222\n", ExpectedResult="0.124,30,5235,222\n")]
    public string TestStripWhiteSpaces(string input)
    {
        return input.StripWhiteSpaces();
    }

}
