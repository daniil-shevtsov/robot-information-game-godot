using GdUnit4.Core;
using Godot;
using System;
using GdUnit4;
using System.Threading.Tasks;
using static GdUnit4.Assertions;
using GdUnit4.Asserts;

//using ExtensionMethods;

[TestSuite]
public partial class GameTest
{
    // ISceneRunner runner = null;

    // [BeforeTest]
    // public void Setup()
    // {
    //     runner = ISceneRunner.Load("res://GameScene.tscn");
    // }

    // [AfterTest]
    // public void TearDown()
    // {
    //     runner.Scene().Free();
    //     //runner.Dispose();
    //     runner = null;
    // }

    [TestCase]
    public async Task TestFocusableShouldBeBlueInitially()
    {
        var runner = ISceneRunner.Load("res://GameScene.tscn", true, true);
        var gameScene = (GameScene)runner.Scene();
        var player = gameScene.player;

        await runner.SimulateFrames(1);

        var focusable = gameScene.player.focusables[0];

        assertColor(focusable, "0000ffff");
    }

    [TestCase]
    public async Task TestFocusableShouldBecomeGreenWhenHoveredOver()
    {
        var runner = ISceneRunner.Load("res://GameScene.tscn", true, true);
        runner.MaximizeView();
        var gameScene = (GameScene)runner.Scene();
        var player = gameScene.player;

        await runner.SimulateFrames(1);

        var focusable = gameScene.player.focusables[0];
        var focusablePosition = gameScene.player.focusables[0].Position;
        var focusableScreenPosition = gameScene.player.camera3D.UnprojectPosition(
            focusablePosition
        );
        var mousePosition = focusableScreenPosition;

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.SimulateFrames(1);

        assertColor(focusable, "00ff00ff");
    }

    [TestCase]
    public async Task TestFocusableShouldBecomeRedWhenClicked()
    {
        var runner = ISceneRunner.Load("res://GameScene.tscn", true, true);
        runner.MaximizeView();
        var gameScene = (GameScene)runner.Scene();
        var player = gameScene.player;

        await runner.SimulateFrames(1);

        var focusable = gameScene.player.focusables[0];
        var focusablePosition = gameScene.player.focusables[0].Transform.Origin;
        var focusableScreenPosition = gameScene.player.camera3D.UnprojectPosition(
            focusablePosition
        );
        var mousePosition = focusableScreenPosition;

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.SimulateFrames(1);
        await runner.AwaitMillis(1000);

        runner.SimulateMouseButtonPress(MouseButton.Left);

        await runner.SimulateFrames(1);
        await runner.AwaitMillis(3000);

        assertColor(focusable, "ff0000ff");
    }

    private void assertColor(FocusableSphere focusable, String expectedColorHtml)
    {
        var currentMaterial = (BaseMaterial3D)focusable.sphere.MaterialOverride;
        GD.Print(
            $"expected {focusable.focusableId} {focusable} to have {expectedColorHtml} but real: {currentMaterial.AlbedoColor.ToHtml()}"
        );
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
    }
}
