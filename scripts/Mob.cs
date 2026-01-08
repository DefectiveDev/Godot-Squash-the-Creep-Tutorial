using Godot;
using System;

public partial class Mob : CharacterBody3D
{
    [Signal]
    public delegate void SquashedEventHandler();

    [Export]
    public int MinSpeed = 10;

    [Export]
    public int MaxSpeed = 18;

    public void Squash()
    {
        EmitSignal(SignalName.Squashed);
        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        MoveAndSlide();
    }

    public void Initialize(Vector3 startPosition, Vector3 playerPosition)
    {
        LookAtFromPosition(startPosition, playerPosition, Vector3.Up);

        RotateY((float)GD.RandRange(-Mathf.Pi/4, Math.PI/4));

        int randomSpeed = GD.RandRange(MinSpeed, MaxSpeed);

        Velocity = Vector3.Forward * randomSpeed;

        Velocity = Velocity.Rotated(Vector3.Up, Rotation.Y);

        GetNode<AnimationPlayer>("AnimationPlayer").SpeedScale = randomSpeed / MinSpeed;
    }

    private void OnVisibilityNotifierScreenExited()
    {
        QueueFree();
    }
}
