using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using SortGame;
using System.Text;
using System.Linq;

public class TestCsv
{
    [Test]
    [TestCase("A,B,C\n", "A", "B", "C")]
    [TestCase("1234,4546,123\n", "1234", "4546", "123")]
    [TestCase("\n")]
    public void TestReadLineString(string line, params string[] answer)
    {
        Assert.IsTrue(Enumerable.SequenceEqual(Csv.ReadLineAsString(line), answer));
    }
    [Test]
    [TestCase("0,1,2,3,4,5\n", 0,1,2,3,4,5)]
    [TestCase("-5,-1,-32,-53,-555554,-905, 5000000\n", -5,-1,-32,-53,-555554,-905,5000000)]
    [TestCase("\n")]
    public void TestReadLineInt(string line, params int[] answer)
    {
        Assert.IsTrue(Enumerable.SequenceEqual(Csv.ReadLineAsInt(line), answer));
    }
    [Test]
    [TestCase("0,1.2,2.3,3.4,4.5,5.6\n", 0,1.2f,2.3f,3.4f,4.5f,5.6f)]
    [TestCase("-5,-1.23,-32,-53.77,-555554,-905.5,5000000.0001\n", -5,-1.23f,-32,-53.77f,-555554,-905.5f,5000000.0001f)]
    [TestCase("\n")]
    public void TestReadLineFloat(string line, params float[] answer)
    {
        Assert.IsTrue(Csv.ReadLineAsFloat(line).Zip(answer, (x, y) => Mathf.Approximately(x, y)).All(x => x));
    }
    private string TestWriteLine<T>(params T[] input)
    {
        StringBuilder stringBuilder = new();
        Csv.WriteLine(stringBuilder, input.ToList());
        return stringBuilder.ToString();
    }
    [Test]
    [TestCase("A", "B", "C", ExpectedResult="A,B,C\n")]
    [TestCase("1234", "4546", "123", ExpectedResult="1234,4546,123\n")]
    [TestCase(ExpectedResult="")]
    public string TestWriteLineString(params string[] input)
    {
        return TestWriteLine(input);
    }
    [Test]
    [TestCase(0,1,2,3,4,5, ExpectedResult="0,1,2,3,4,5\n")]
    [TestCase(-5,-1,-32,-53,-555554,-905,5000000, ExpectedResult="-5,-1,-32,-53,-555554,-905,5000000\n")]
    [TestCase(ExpectedResult="")]
    public string TestWriteLineInt(params int[] input)
    {
        return TestWriteLine(input);
    }
    [Test]
    [TestCase(0,1.2f,2.3f,3.4f,4.5f,5.6f)]
    [TestCase(-5,-1.23f,-32,-53.77f,-555554,-905.5f,5000000.0001f)]
    [TestCase()]
    public void TestWriteLineFloat(params float[] input)
    {
        Assert.IsTrue(Csv.ReadLineAsFloat(TestWriteLine(input)).Zip(input, (x, y) => Mathf.Approximately(x, y)).All(x => x));
    }

    [Test]
    [TestCase("AB\nBC\nCD\nDE\n", "AB", "BC", "CD", "DE")]
    [TestCase("AB,a\nBC,b\nCD,c\nDE,d\n", "AB,a", "BC,b", "CD,c", "DE,d")]
    public void TestGetAllLines(string source, params string[] answer)
    {
        var result = Csv.GetAllLines(source).ToArray();
        Assert.IsTrue(result.Length == answer.Length);
        for(int i = 0; i < result.Length; ++i)
            Assert.AreEqual(result[i], answer[i]);
    }
    
}
