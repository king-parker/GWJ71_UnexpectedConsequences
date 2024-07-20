using Godot;
using System;

public class HUD : CanvasLayer
{
    private const string RAGE_BASE_TEXT = "Rage: ";

    private GameManager _gameManager;
    private Label _rageLabel;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _gameManager.Connect("RageUpdated", this, "UpdateRage");

        _rageLabel = GetNode<Label>("EnemyRage");
        UpdateRage(_gameManager.GetRage());
    }

    public void UpdateRage(int rage)
    {
        _rageLabel.Text = RAGE_BASE_TEXT + rage;

        if (rage > 0)
        {
            _rageLabel.Show();
        }
        else
        {
            _rageLabel.Hide();
        }
    }
}
