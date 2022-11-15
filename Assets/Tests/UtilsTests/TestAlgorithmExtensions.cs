using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;

public class TestAlgorithmExtensions
{
    [Test, Timeout(100)]
    [TestCase(0, 1, 2, 3, 4, 5, 6, 7, 8, ExpectedResult=true)]
    [TestCase(0, 1, 2, 3, 4, 3, 2, 1, 0, ExpectedResult=false)]
    [TestCase(7, 5, 4, 3, 1, 0, ExpectedResult=false)]
    [TestCase(-7, -5, -4, -3, -1, 0, ExpectedResult=true)]
    [TestCase(-7, -5, -3, -3, 0, 0, ExpectedResult=true)]
    [TestCase(0, 1, 1, 3, 4, 5, 5, 7, 7, ExpectedResult=true)]
    [TestCase(38, 74, 36, ExpectedResult=false)]
    [TestCase(0, ExpectedResult=true)]
    public bool TestMonotoneIncreasing(params int[] input)
    {
        List<int> numbers = new(input);
        return numbers.IsMonotoneIncreasing();
    }
    [Test, Timeout(100)]
    [TestCase(0, 1, 2, 3, 4, 5, 6, 7, 8, ExpectedResult=false)]
    [TestCase(0, 1, 2, 3, 4, 3, 2, 1, 0, ExpectedResult=false)]
    [TestCase(7, 5, 4, 3, 1, 0, ExpectedResult=true)]
    [TestCase(-7, -5, -4, -3, -1, 0, ExpectedResult=false)]
    [TestCase(5, 4, 3, 3, 2, 2, 0, 0, ExpectedResult=true)]
    [TestCase(7, 5, -3, -3, -5, -6, ExpectedResult=true)]
    [TestCase(0, -1, -1, -3, -4, -5, -5, -7, -7, ExpectedResult=true)]
    [TestCase(38, 74, 36, ExpectedResult=false)]
    [TestCase(0, ExpectedResult=true)]
    public bool TestMonotoneDecreasing(params int[] input)
    {
        List<int> numbers = new(input);
        return numbers.IsMonotoneDecreasing();
    }
    [Test, Timeout(100)]
    [TestCase(0, 1, 2, 3, 4, 5, 6, 7, 8, ExpectedResult=true)]
    [TestCase(0, 1, 2, 3, 3, 5, 6, 7, 7, ExpectedResult=true)]
    [TestCase(0, -1, -2, -3, -4, -5, -6, -7, -8, ExpectedResult=true)]
    [TestCase(0, -1, -2, -3, -3, -5, -6, -7, -7, ExpectedResult=true)]
    [TestCase(0, 1, 2, 3, 0, 5, 6, 7, 3, ExpectedResult=false)]
    [TestCase(0, -1, -2, -3, 3, -5, 6, -7, 7, ExpectedResult=false)]
    [TestCase(38, 74, 36, ExpectedResult=false)]
    [TestCase(0, ExpectedResult=true)]
    public bool TestIsMonotonic(params int[] input)
    {
        List<int> numbers = new(input);
        return numbers.IsMonotonic();
    }
}
