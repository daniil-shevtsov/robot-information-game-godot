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

        await runner.SimulateFrames(1);

        var focusablePosition = gameScene.player.focusables[16].Position;
        var focusableScreenPosition = gameScene.player.camera3D.UnprojectPosition(
            focusablePosition
        );
        var mousePosition = focusableScreenPosition;

        await runner.SimulateFrames(1);

        var currentMaterial3 = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial3.AlbedoColor.ToHtml()).IsEqual("0000ffff");

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.SimulateFrames(1);

        var currentMaterial = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual("00ff00ff");
        runner.SimulateMouseButtonPressed(MouseButton.Left);

        await runner.SimulateFrames(1);

        var currentMaterial2 = (BaseMaterial3D)
            gameScene.player.focusables[16].sphere.MaterialOverride;
        AssertObject(currentMaterial2.AlbedoColor.ToHtml()).IsEqual("ff0000ff");
    }
}
