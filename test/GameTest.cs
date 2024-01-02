using GdUnit4.Core;
using Godot;
using System;
using GdUnit4;
using static GdUnit4.Assertions;

[TestSuite]
public partial class GameTest
{
    [Before]
    public void Setup() { }

    [TestCase]
    public void TestTrue()
    {
        AssertThat(true).IsTrue();
    }
}
