using Godot;
using System;

public class Player : KinematicBody2D
{
    [Export] private float _speed = 100.0f;
    [Export] private float _jumpSpeed = 300.0f;
    [Export] private float _gravity = 15.0f;
    [Export] private bool _isAttacking = false;

    private Vector2 _velocity = new Vector2();
    private bool _isJumping = false;
    private bool _isAlive = true;
    private AnimatedSprite _characterAnimation;
    private AnimationPlayer _attackAnimation;
    private AnimatedSprite _attackPositioning;
    private AudioStreamPlayer2D _jumpSound;
    private Timer _coyoteTimer;
    private Timer _jumpBufferTimer;
    private Timer _shortHopTimer;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _isAttacking = false;
        _isAlive = true;

        _characterAnimation = GetNode<AnimatedSprite>("CharacterAnimation");
        _characterAnimation.Play("idle");

        _attackAnimation = GetNode<AnimationPlayer>("AttackAnimation");
        _attackAnimation.Play("RESET");

        _attackPositioning = GetNode<AnimatedSprite>("Attack");

        _jumpSound = GetNode<AudioStreamPlayer2D>("Sounds/JumpSound");

        _coyoteTimer = GetNode<Timer>("Timers/CoyoteTimer");
        _coyoteTimer.Stop();

        _jumpBufferTimer = GetNode<Timer>("Timers/JumpBufferTimer");
        _jumpBufferTimer.Stop();

        _shortHopTimer = GetNode<Timer>("Timers/ShortHopTimer");
        _shortHopTimer.Stop();

        _gameManager = GetNode<GameManager>("/root/GameManager");
        _gameManager.Connect("PlayerDeathEvent", this, "OnDeath");
    }

    public override void _PhysicsProcess(float delta)
    {
        // Add the gravity.
        if (!IsOnFloor())
        {
            _velocity.y += _gravity;
            if (_velocity.y > 0 && _isJumping)
            {
                _isJumping = false;
            }
        }

        // Handle jump.
        if (Input.IsActionJustPressed("jump"))
        {
            _jumpBufferTimer.Start();
            if (IsOnFloor() || !_coyoteTimer.IsStopped())
            {
                PlayerJump();
            }
        }
        else if (IsOnFloor() && !_jumpBufferTimer.IsStopped())
        {
            PlayerJump();
        }

        if (!Input.IsActionPressed("jump") && _isJumping && _shortHopTimer.IsStopped())
        {
            _velocity.y = 0;
            _isJumping = false;
        }

        // Get the input direction.
        float direction = Input.GetAxis("move_left", "move_right");
        _velocity.x = direction * _speed;

        if (direction > 0)
        {
            _characterAnimation.FlipH = false;
            if (!_isAttacking)
            {
                _attackPositioning.FlipH = false;
                if (_attackPositioning.Position.x < 0)
                {
                    _attackPositioning.Position *= new Vector2(-1, 1);
                }
            }
        }
        else if (direction < 0)
        {
            _characterAnimation.FlipH = true;
            if (!_isAttacking)
            {
                _attackPositioning.FlipH = true;
                if (_attackPositioning.Position.x > 0)
                {
                    _attackPositioning.Position *= new Vector2(-1, 1);
                }
            }
        }
        else
        {
            if (!_isAttacking)
            {
                bool isFacingLeft = _characterAnimation.FlipH;
                _attackPositioning.FlipH = isFacingLeft;
                if (isFacingLeft && _attackPositioning.Position.x > 0)
                {
                    _attackPositioning.Position *= new Vector2(-1, 1);
                }
                else if (!isFacingLeft && _attackPositioning.Position.x < 0)
                {
                    _attackPositioning.Position *= new Vector2(-1, 1);
                }
            }
        }

        // Choose player animation
        if (IsOnFloor() || !_coyoteTimer.IsStopped())
        {    
            if (direction == 0) _characterAnimation.Play("idle");
            else _characterAnimation.Play("run");
        }
        else
        {
            _characterAnimation.Play("jump");
        }

        // Choose attack animation
        if (Input.IsActionJustPressed("attack") && _isAlive)
        {
            _attackAnimation.Play("attack");
        }
        else if (!_isAlive)
        {
            _attackAnimation.Play("RESET");
        }

        bool wasOnFloor = IsOnFloor();
        _velocity = MoveAndSlide(_velocity, new Vector2(0, -1));

        if (wasOnFloor && !IsOnFloor())
        {
            _coyoteTimer.Start();
        }
    }

    private void PlayerJump()
    {
        _velocity.y = -_jumpSpeed;
        _jumpSound.Play();
        _isJumping = true;
        _shortHopTimer.Start();
    }

    private void OnDeath()
    {
        _isAlive = false;
    }

}
