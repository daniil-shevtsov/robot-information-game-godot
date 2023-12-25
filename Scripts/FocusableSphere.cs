using System;
using Godot;

public partial class FocusableSphere : StaticBody3D
{
    public String lol = "kek";

    public CsgSphere3D sphere = null;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        sphere = GetNode<CsgSphere3D>("CSGSphere3D");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }
}
