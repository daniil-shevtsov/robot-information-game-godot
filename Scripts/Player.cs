using Godot;
using System;
using System.Collections.Generic;

public partial class Player : CharacterBody3D
{
    // How fast the player moves in meters per second.
    [Export]
    public int Speed { get; set; } = 14;

    // The downward acceleration when in the air, in meters per second squared.
    [Export]
    public int FallAcceleration { get; set; } = 75;

    private Vector3 _targetVelocity = Vector3.Zero;
    private const float RayLength = 1000.0f;
    private Vector3 mousePosition = Vector3.Zero;

    public Camera3D camera3D = null;

    public List<FocusableSphere> focusables = new List<FocusableSphere>();

    private StandardMaterial3D blueMaterial = new StandardMaterial3D();
    private StandardMaterial3D greenMaterial = new StandardMaterial3D();
    private StandardMaterial3D redMaterial = new StandardMaterial3D();

    bool shouldInit = true;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        blueMaterial.AlbedoColor = Color.FromHtml("#0000FF");
        greenMaterial.AlbedoColor = Color.FromHtml("#00FF00");
        redMaterial.AlbedoColor = Color.FromHtml("#FF0000");

        camera3D = GetNode<Camera3D>("CameraPivot/Camera3D");
        focusables = new List<FocusableSphere>();
        var scene = GetTree().CurrentScene;
        var codeFocusable = GD.Load<PackedScene>("res://Prefabs/focusable_sphere.tscn");

        var radius = 6.0;

        var focusableCount = 32;
        for (int i = 0; i < focusableCount; ++i)
        {
            var instanceFocusable = codeFocusable.Instantiate();
            var myFocusable = (FocusableSphere)instanceFocusable;
            scene.CallDeferred("add_child", instanceFocusable);
            focusables.Add(myFocusable);

            myFocusable.lol = $"focusable{i}";

            var step = 2 * Math.PI / focusableCount;
            var angle = step * i;

            myFocusable.GlobalPosition = new Vector3(
                (float)(radius * Math.Sin(angle)),
                3f,
                (float)(radius * -Math.Cos(angle))
            );
        }
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
        if (shouldInit)
        {
            clearColors();
            shouldInit = false;
        }

        var direction = Vector3.Zero;
        // We check for each move input and update the direction accordingly.
        if (Input.IsActionPressed("move_right"))
        {
            direction.X += 1.0f;
        }
        if (Input.IsActionPressed("move_left"))
        {
            direction.X -= 1.0f;
        }
        if (Input.IsActionPressed("move_back"))
        {
            direction.Z += 1.0f;
        }
        if (Input.IsActionPressed("move_forward"))
        {
            direction.Z -= 1.0f;
        }
        if (direction != Vector3.Zero)
        {
            direction = direction.Normalized();
        }
        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        bool onFloor = IsOnFloor();
        if (!onFloor)
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }

    public override void _Input(InputEvent @event)
    {
        if (
            @event is InputEventMouseButton eventMouseButton
            && eventMouseButton.Pressed
            && eventMouseButton.ButtonIndex == MouseButton.Left
        )
        {
            onClickInput(eventMouseButton);
        }
        else if (@event is InputEventMouseMotion eventMouseMotion)
        {
            onMouseMotion(eventMouseMotion);
        }
    }

    private void onMouseMotion(InputEventMouseMotion eventMouseMotion)
    {
        var focusable = findIntersectedFocusable(eventMouseMotion.Position);

        if (focusable != null)
        {
            setFocusableColor(focusable, Color.FromHtml("#00FF00"));
        }
        else
        {
            clearColors();
        }
    }

    private void onClickInput(InputEventMouseButton eventMouseButton)
    {
        var focusable = findIntersectedFocusable(eventMouseButton.Position);
        if (focusable != null)
        {
            setFocusableColor(focusable, Color.FromHtml("#FF0000"));
        }
        else
        {
            clearColors();
        }
    }

    private FocusableSphere findIntersectedFocusable(Vector2 mousePosition)
    {
        var from = camera3D.ProjectRayOrigin(mousePosition);
        var to = from + camera3D.ProjectRayNormal(mousePosition) * RayLength;

        var spaceState = GetWorld3D().DirectSpaceState;
        var query = PhysicsRayQueryParameters3D.Create(from, to);
        query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
        var result = spaceState.IntersectRay(query);

        if (result != null && result.ContainsKey("collider"))
        {
            var focusable = (FocusableSphere)result["collider"];
            return focusable;
        }
        return null;
    }

    private void setFocusableColor(FocusableSphere focusable, Color newColor)
    {
        StandardMaterial3D material = new StandardMaterial3D();
        material.AlbedoColor = newColor;

        focusable.sphere.MaterialOverride = material;
    }

    private void setFocusableMaterial(FocusableSphere focusable, Material material)
    {
        focusable.sphere.MaterialOverride = material;
    }

    private void clearColors()
    {
        GD.Print("Clear colors");
        foreach (var focusable in focusables)
        {
            setFocusableMaterial(focusable, blueMaterial);
        }
    }
}
