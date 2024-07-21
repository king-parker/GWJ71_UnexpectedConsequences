using Godot;
using System;

public class LevelEndZone : Area2D
{
    private GameManager _gameManager;
    private bool _hasTriggered = false;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");

        this.Connect("body_entered", this, "OnBodyEntered");
    }

    private void OnBodyEntered(Node body)
    {
        if (!_hasTriggered)
        {
            _hasTriggered = true;
            GD.Print("End of level");

            _gameManager.GotoNextLevel();
        }
    }
}
