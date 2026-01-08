using Godot;
using System;

public partial class Player : CharacterBody3D
{
    [Signal]
    public delegate void HitEventHandler();

    [Export]
    public int BounceImpulse =16;

    [Export]
    public int JumpImpulse = 20;

    [Export]
    public int Speed = 14;

    [Export]
    public int FallAcceleration = 75;

    private Vector3 _targetVelocity = Vector3.Zero;

    private void Die()
    {
        EmitSignal(SignalName.Hit);
        QueueFree();
    }

    private void OnMobDetectorBodyEntered(Node3D body) => Die();

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

        for (int index = 0; index < GetSlideCollisionCount(); index++)
        {
            KinematicCollision3D collision = GetSlideCollision(index);

            if (collision.GetCollider() is Mob mob)
            {
                // check for hitting above
                if (Vector3.Up.Dot(collision.GetNormal()) > 0.1f)
                {
                    mob.Squash();
                    _targetVelocity.Y = BounceImpulse;

                    break;
                }
            }
        }

        Velocity = _targetVelocity;
        MoveAndSlide();
    }
}
