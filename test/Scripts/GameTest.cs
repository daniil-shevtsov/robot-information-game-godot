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
        GD.Print("test log: TestFocusableShouldBeBlueInitially start");
        var runner = ISceneRunner.Load("res://GameScene.tscn", true, true);
        var gameScene = (GameScene)runner.Scene();
        var player = gameScene.player;
        GD.Print($"TestFocusableShouldBeBlueInitially scene={gameScene} player={player}");

        await runner.SimulateFrames(1);

        var focusable = gameScene.player.focusables[0];

        assertColor(focusable, "0000ffff");
        GD.Print("test log: TestFocusableShouldBeBlueInitially end");
    }

    [TestCase]
    public async Task TestFocusableShouldBecomeGreenWhenHoveredOver()
    {
        GD.Print("test log: TestFocusableShouldBecomeGreenWhenHoveredOver start");
        var runner = ISceneRunner.Load("res://GameScene.tscn", true, true);
        runner.MaximizeView();
        var gameScene = (GameScene)runner.Scene();
        var player = gameScene.player;
        GD.Print(
            $"TestFocusableShouldBecomeGreenWhenHoveredOver scene={gameScene} player={player}"
        );

        await runner.SimulateFrames(1);
        await runner.AwaitMillis(2000);

        var focusable = gameScene.player.focusables[0];
        var focusablePosition = gameScene.player.focusables[0].Position;
        var focusableScreenPosition = gameScene.player.camera3D.UnprojectPosition(
            focusablePosition
        );
        var mousePosition = focusableScreenPosition;

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.AwaitMillis(2000);

        await runner.SimulateFrames(1);

        assertColor(focusable, "00ff00ff");
        await runner.AwaitMillis(2000);
        GD.Print("test log: TestFocusableShouldBecomeGreenWhenHoveredOver end");
    }

    // [TestCase]
    // public async Task TestSceneRunner()
    // {
    //     GD.Print("test log: TestSceneRunner start");
    //     var runner = ISceneRunner.Load("res://GameScene.tscn");
    //     runner.MaximizeView();
    //     var gameScene = (GameScene)runner.Scene();

    //     await runner.SimulateFrames(1);
    //     await runner.AwaitIdleFrame();

    //     var focusablePosition = gameScene.player.focusables[16].Position;
    //     var focusableScreenPosition = gameScene.player.camera3D.UnprojectPosition(
    //         focusablePosition
    //     );
    //     var mousePosition = focusableScreenPosition;

    //     await runner.SimulateFrames(1);
    //     await runner.AwaitIdleFrame();

    //     assertColor(gameScene.player.focusables[16], "0000ffff");

    //     runner.SimulateMouseMove(mousePosition);
    //     runner.SetMousePos(mousePosition);

    //     await runner.SimulateFrames(1);
    //     await runner.AwaitIdleFrame();

    //     assertColor(gameScene.player.focusables[16], "00ff00ff");

    //     runner.SimulateMouseButtonPressed(MouseButton.Left);

    //     await runner.SimulateFrames(1);
    //     await runner.AwaitIdleFrame();
    //     assertColor(gameScene.player.focusables[16], "ff0000ff");
    //     GD.Print("test log: TestSceneRunner end");
    // }

    private void assertColor(FocusableSphere focusable, String expectedColorHtml)
    {
        var currentMaterial = (BaseMaterial3D)focusable.sphere.MaterialOverride;
        GD.Print(
            $"expected {focusable.lol} {focusable} to have {expectedColorHtml} but real: {currentMaterial.AlbedoColor.ToHtml()}"
        );
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
    }
}
