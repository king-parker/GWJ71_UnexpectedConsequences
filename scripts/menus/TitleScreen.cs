using Godot;
using System;

public class TitleScreen : Node2D
{
    private Button _startButton;
    private GameManager _gameManager;

    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        
        _startButton = GetNode<Button>("UI/StartButton");
        _startButton.Connect("pressed", _gameManager, nameof(GameManager.GotoNextLevel));
        _startButton.GrabFocus();
    }
}
