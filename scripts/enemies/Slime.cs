using Godot;
using System;

public class Slime : Node2D
{
    [Export] public float Speed = 50.0f;
    [Export] public bool StartGoingRight = true;
    [Export] public int RageScaling = 10;

    private float _baseSpeed;
    private float _direction = 1f;
    private AnimatedSprite _animation;
    private RayCast2D _rayCastSide;
    private RayCast2D _rayCastDown;
    private const float _rayCastLength = 10;
    private const float _rayCastYOffset = -6;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _baseSpeed = Speed;

        _animation = GetNode<AnimatedSprite>("AnimatedSprite");
        _animation.Play("default");

        _rayCastSide = GetNode<RayCast2D>("RayCastSide");
        _rayCastDown = GetNode<RayCast2D>("RayCastDown");

        _direction = StartGoingRight ? 1 : -1;
        SetDirection();

        _gameManager = GetNode<GameManager>("/root/GameManager");
        _gameManager.Connect("RageUpdated", this, "ProcessRage");

        ProcessRage(_gameManager.GetRage());
    }

    public override void _Process(float delta)
    {
        if (_rayCastSide.IsColliding() || !_rayCastDown.IsColliding())
            {
                _direction *= -1;
                SetDirection();
            }

        Position += new Vector2(_direction * Speed * delta, 0);
    }

    public void Death()
    {
        _gameManager.IncrementRage();
        QueueFree();
    }

    private void SetDirection()
    {
        _rayCastSide.CastTo = new Vector2(_direction * _rayCastLength, 0);
        _rayCastDown.Position = new Vector2(_direction * _rayCastLength, _rayCastYOffset);
        _animation.FlipH = (_direction == -1);
    }

    private void ProcessRage(int rage)
    {
        float factor = 1 + rage / (float)RageScaling;
        // GD.Print("Scaling factor: " + factor);
        Speed = _baseSpeed * factor;
        // GD.Print("New Speed: " + Speed);
        Scale = new Vector2(factor, factor);
    }
}
