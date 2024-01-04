using GdUnit4.Core;
using Godot;
using System;
using GdUnit4;
using static GdUnit4.Assertions;
using System.Threading.Tasks;

[TestSuite]
public partial class GameTest
{
    [Before]
    public void Setup() { }

    [TestCase]
    public async Task TestSceneRunner()
    {
        ISceneRunner runner = ISceneRunner.Load("res://GameScene.tscn");
        var gameScene = (GameScene)runner.Scene();
        GD.Print($"my log: {gameScene.player.focusables.Count}");
        await runner.SimulateFrames(10);
    }

    [TestCase]
    public void TestFocusable()
    {
        var codeFocusable = GD.Load<PackedScene>("res://Prefabs/focusable_sphere.tscn");
        var instanceFocusable = codeFocusable.Instantiate();
        var myFocusable = (FocusableSphere)instanceFocusable;
        myFocusable.lol = $"focusable{5}";
        AssertThat(myFocusable.lol).IsEqual("focusable5");
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
