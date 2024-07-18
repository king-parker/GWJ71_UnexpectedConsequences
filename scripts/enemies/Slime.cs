using Godot;
using System;

public class Slime : Node2D
{
    [Export] public float Speed = 50.0f;
    [Export] public bool StartGoingRight = true;

    private float _direction = 1f;
    private AnimatedSprite _animation;
    private RayCast2D _rayCastSide;
    private RayCast2D _rayCastDown;
    private const float _rayCastLength = 10;
    private const float _rayCastYOffset = -6;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite>("AnimatedSprite");
        _animation.Play("default");

        _rayCastSide = GetNode<RayCast2D>("RayCastSide");
        _rayCastDown = GetNode<RayCast2D>("RayCastDown");

        _direction = StartGoingRight ? 1 : -1;
        SetDirection();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (_rayCastSide.IsColliding() || !_rayCastDown.IsColliding())
            {
                _direction *= -1;
                SetDirection();
            }

        Position += new Vector2(_direction * Speed * delta, 0);
    }

    private void SetDirection()
    {
        _rayCastSide.CastTo = new Vector2(_direction * _rayCastLength, 0);
        _rayCastDown.Position = new Vector2(_direction * _rayCastLength, _rayCastYOffset);
        _animation.FlipH = (_direction == -1);
    }
}
