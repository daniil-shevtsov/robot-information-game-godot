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

        assertColor(gameScene.player.focusables[16], "0000ffff");

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.SimulateFrames(1);

        assertColor(gameScene.player.focusables[16], "00ff00ff");

        runner.SimulateMouseButtonPressed(MouseButton.Left);

        await runner.SimulateFrames(1);
        assertColor(gameScene.player.focusables[16], "ff0000ff");
    }

    private void assertColor(FocusableSphere focusable, String expectedColorHtml)
    {
        var currentMaterial = (BaseMaterial3D)focusable.sphere.MaterialOverride;
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
    }
}
