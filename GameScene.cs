using Godot;
using System;

public partial class GameScene : Node3D
{
    public Player player = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GD.Print($"Scene{this} Ready");
        player = GetNode<Player>("Player");
        GD.Print($"Scene{this} has Player{player}");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
