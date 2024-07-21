using Godot;
using System;

public class HUD : CanvasLayer
{
    private const string RAGE_BASE_TEXT = "Rage: ";

    private GameManager _gameManager;
    private Label _rageLabel;
    private ColorRect _deathOverlay;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _gameManager = GetNode<GameManager>("/root/GameManager");
        _gameManager.Connect(nameof(GameManager.RageUpdated), this, "UpdateRage");
        _gameManager.Connect(nameof(GameManager.PlayerDeathEvent), this, "ShowDeathOverlay");

        _rageLabel = GetNode<Label>("EnemyRage");
        UpdateRage(_gameManager.GetRage());

        _deathOverlay = GetNode<ColorRect>("DeathOverlay");
        _deathOverlay.Hide();
        
    }

    public void UpdateRage(int rage)
    {
        _rageLabel.Text = RAGE_BASE_TEXT + rage;

        if (rage > 0)
        {
            _rageLabel.Show();
            if (_gameManager.wasRageHidden)
            {
                _gameManager.wasRageHidden = false;
                GetNode<AnimationPlayer>("EnemyRage/AppearAnimation").Play("appear");
            }
        }
        else
        {
            _rageLabel.Hide();
            _gameManager.wasRageHidden = true;
        }
    }

    private void ShowDeathOverlay()
    {
        _deathOverlay.Show();
    }
}
