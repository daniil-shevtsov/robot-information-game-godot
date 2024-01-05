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
        //my log: focusable: (7.3478806E-16, 3, 6) ((576, 570.4077))
        ISceneRunner runner = ISceneRunner.Load("res://GameScene.tscn");
        var gameScene = (GameScene)runner.Scene();
        var mousePosition = new Vector2(576, 595.0876f);
        var focusablePosition = gameScene.player.focusables[16].Position;
        await runner.AwaitMillis(1000);
        await runner.AwaitIdleFrame();
        await runner.SimulateFrames(3);
        var currentMaterial3 = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial3.AlbedoColor.ToHtml()).IsEqual("0000ffff");
        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);
        GD.Print(
            $"test log: mouse={mousePosition} focusable={focusablePosition} {gameScene.player.camera3D.UnprojectPosition(focusablePosition)}"
        );

        await runner.AwaitMillis(1000);
        await runner.AwaitIdleFrame();

        var currentMaterial = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual("00ff00ff");
        runner.SimulateMouseButtonPressed(MouseButton.Left);
        await runner.AwaitMillis(1000);
        await runner.AwaitIdleFrame();

        // await runner.AwaitMillis(1000);
        var currentMaterial2 = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial2.AlbedoColor.ToHtml()).IsEqual("ff0000ff");
        await runner.AwaitIdleFrame();

        //await runner.AwaitMillis(4000);
    }
}
