using Godot;
using System;

public class WinScreen : Node2D
{
    private GameManager _gameManager;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        var rage = _gameManager.GetRage();

        if (rage > 0)
        {
            var plural = rage == 1 ? "" : "s";
            Label rageMessage = GetNode<Label>("UI/RageMessage");
            rageMessage.Text = "And it only took killing " + rage
                + " poor slime" + plural + ".\nWhatever helps you sleep at night.";
        }

        _gameManager.ResetRage();
    }
}
