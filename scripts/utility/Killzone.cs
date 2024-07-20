using Godot;
using System;

public class Killzone : Area2D
{
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");

        this.Connect("body_entered", this, "OnBodyEntered");
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("You died!");
        CollisionShape2D bodyCollision = body.GetNode<CollisionShape2D>("HurtBox");
        bodyCollision?.QueueFree();

        _gameManager.PlayerDied();
    }
}
