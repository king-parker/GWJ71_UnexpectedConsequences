using Godot;
using System;

public class Killzone : Area2D
{
    private Timer _timer;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", this, "OnTimerTimeout");

        this.Connect("body_entered", this, "OnBodyEntered");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("You died!");
        Engine.TimeScale = 0.5f;
        CollisionShape2D bodyCollision = body.GetNode<CollisionShape2D>("CollisionShape2D");
        if (bodyCollision != null)
        {
            bodyCollision.QueueFree();
        }
        _timer.Start();
    }

    private void OnTimerTimeout()
    {
        Engine.TimeScale = 1;
        GetTree().ReloadCurrentScene();
    }
}
