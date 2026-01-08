using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Export]
    public int JumpImpulse = 20;

    [Export]
    public int Speed = 14;

    [Export]
    public int FallAcceleration = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

    public override void _PhysicsProcess(double delta)
    {
        var direction = new Vector3(Input.GetAxis("move_left", "move_right"), 0f,
                                    Input.GetAxis("move_forward", "move_back"))
                                    .Normalized();

        if (direction != Vector3.Zero)
        {
            GetNode<Node3D>("Pivot").Basis = Basis.LookingAt(direction);
        }

        _targetVelocity.X = direction.X * Speed;
        _targetVelocity.Z = direction.Z * Speed;

        if (!IsOnFloor())
        {
            _targetVelocity.Y -= FallAcceleration * (float)delta;
        }

        if (IsOnFloor() && Input.IsActionJustPressed("jump"))
        {
            _targetVelocity.Y = JumpImpulse;
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
