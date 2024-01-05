using GdUnit4.Core;
using Godot;
using System;
using GdUnit4;
using static GdUnit4.Assertions;
using System.Threading.Tasks;
using ExtensionMethods;
using GdUnit4.Asserts;

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

        AssertThat(gameScene.player.focusables[16]).HasColor("0000ffff");

        runner.SimulateMouseMove(mousePosition);
        runner.SetMousePos(mousePosition);

        await runner.SimulateFrames(1);

        AssertThat(gameScene.player.focusables[16]).HasColor("00ff00ff");
        gameScene.player.focusables[16].HasColor("00ff00ff");

        runner.SimulateMouseButtonPressed(MouseButton.Left);

        await runner.SimulateFrames(1);

        AssertThat(gameScene.player.focusables[16]).HasColor("ff0000ff");
    }

    private void assertColor(FocusableSphere focusable, String expectedColorHtml)
    {
        var currentMaterial = (BaseMaterial3D)focusable.sphere.MaterialOverride;
        AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
    }
}

namespace ExtensionMethods
{
    public static class LolExtensions
    {
        public static bool IsGreaterThan(this int i, int value)
        {
            return i > value;
        }

        public static bool IsLesserThan(this FocusableSphere focusable, String expectedColorHtml)
        {
            return false;
        }

        public static void HasColor(this FocusableSphere focusable, String expectedColorHtml)
        {
            var currentMaterial = (BaseMaterial3D)focusable.sphere.MaterialOverride;
            AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
        }

        public static void HasColor(
            this IAssertBase<FocusableSphere> focusable,
            String expectedColorHtml
        )
        {
            var currentMaterial = (BaseMaterial3D)focusable.UnboxVariant().sphere.MaterialOverride;
            AssertObject(currentMaterial.AlbedoColor.ToHtml()).IsEqual(expectedColorHtml);
        }
    }
}
