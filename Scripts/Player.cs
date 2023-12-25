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

    private Camera3D camera3D = null;

    private List<FocusableSphere> focusables = new List<FocusableSphere>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        camera3D = GetNode<Camera3D>("CameraPivot/Camera3D");
        focusables = new List<FocusableSphere>();
        //TODO: Create focusables with code
        focusables.Add(GetNode<FocusableSphere>("../FocusableSphere"));
        focusables.Add(GetNode<FocusableSphere>("../FocusableSphere2"));
        focusables.Add(GetNode<FocusableSphere>("../FocusableSphere3"));
        focusables[0].lol = "kek0";
        focusables[1].lol = "kek1";
        focusables[2].lol = "kek2";
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta) { }

    public override void _PhysicsProcess(double delta)
    {
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
            var from = camera3D.ProjectRayOrigin(eventMouseButton.Position);
            var to = from + camera3D.ProjectRayNormal(eventMouseButton.Position) * RayLength;

            var spaceState = GetWorld3D().DirectSpaceState;
            var query = PhysicsRayQueryParameters3D.Create(from, to);
            query.Exclude = new Godot.Collections.Array<Rid> { GetRid() };
            var result = spaceState.IntersectRay(query);

            var intersectedObject = (FocusableSphere)result["collider"];
            GD.Print($"from={from} to={to} intersects={intersectedObject.lol}");
        }
    }
}
