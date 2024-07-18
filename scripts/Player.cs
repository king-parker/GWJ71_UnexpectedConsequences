using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] private float _speed = 100.0f;
    [Export] private float _jumpSpeed = -300.0f;
    [Export] private float _gravity = 980.0f;

    private Vector2 _velocity = new Vector2();
    private bool _jumping = false;
    private AnimatedSprite _animation;

    public override void _Ready()
    {
        _animation = GetNode<AnimatedSprite>("AnimatedSprite");
        _animation.Play("idle");
    }

    public override void _PhysicsProcess(float delta)
    {
        // Add the gravity.
        _velocity.y += _gravity * (float)delta;

        // Handle jump.
        if (Input.IsActionJustPressed("ui_accept") && IsOnFloor())
            _velocity.y = _jumpSpeed;

        // Get the input direction.
        float direction = Input.GetAxis("ui_left", "ui_right");
        _velocity.x = direction * _speed;

        _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));
    }
}
