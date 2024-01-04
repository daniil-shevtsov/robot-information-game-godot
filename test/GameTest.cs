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
    public void TestGameState()
    {
        var codeFocusable = GD.Load<PackedScene>("res://Prefabs/focusable_sphere.tscn");
        var instanceFocusable = codeFocusable.Instantiate();
        var myFocusable = (FocusableSphere)instanceFocusable;
        myFocusable.lol = $"focusable{5}";
        AssertThat(myFocusable.lol).IsEqual("focusable7");
    }

    [TestCase]
    public void TestFalse()
    {
        AssertThat(false).IsFalse();
    }

    [TestCase]
    public void TestTrue()
    {
        AssertThat(true).IsTrue();
    }
}
