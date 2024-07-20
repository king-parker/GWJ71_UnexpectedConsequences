using Godot;
using System;

public class GameManager : Node
{
    private int _rage = 0;
    private Timer _restartTimer;

    [Signal] public delegate void RageUpdated(int newRage);

    public override void _Ready()
    {
        _restartTimer = GetNode<Timer>("Timers/RestartTimer");
        _restartTimer.Connect("timeout", this, "OnRestartTimerTimeout");
    }

    public void IncrementRage()
    {
        _rage += 1;
        EmitSignal(nameof(RageUpdated), _rage);
    }

    public void ResetRage()
    {
        _rage = 0;
        EmitSignal(nameof(RageUpdated), _rage);
    }

    public int GetRage()
    {
        return _rage;
    }

    public void PlayerDied()
    {
        _restartTimer.Start();
        Engine.TimeScale = 0.5f;
    }

    private void OnRestartTimerTimeout()
    {
        Engine.TimeScale = 1;
        GetTree().ReloadCurrentScene();
    }
}
