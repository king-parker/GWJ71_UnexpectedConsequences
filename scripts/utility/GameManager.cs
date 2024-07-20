using Godot;
using System;

public class GameManager : Node
{
    public static string TUTORIAL_LEVEL = "res://scenes/levels/LevelTutorial.tscn";
    public static string LEVEL_ONE = "res://scenes/levels/Level1.tscn";
    
    private int _rage = 0;
    private Timer _restartTimer;
    private Node _currentScene;

    [Signal] public delegate void RageUpdated(int newRage);

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1); // Current scene is the last child in the root node

        _restartTimer = GetNode<Timer>("Timers/RestartTimer");
        _restartTimer.Connect("timeout", this, "OnRestartTimerTimeout");
    }

    public void GotoScene(string path)
    {
        CallDeferred(nameof(DeferredGotoScene), path);
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

    private void DeferredGotoScene(string path)
    {
        _currentScene.Free();

        var nextScene = GD.Load<PackedScene>(path);

        _currentScene = nextScene.Instance();

        GetTree().Root.AddChild(_currentScene);

        GetTree().CurrentScene = _currentScene;
    }

    private void OnRestartTimerTimeout()
    {
        Engine.TimeScale = 1;
        GetTree().ReloadCurrentScene();
    }
}
