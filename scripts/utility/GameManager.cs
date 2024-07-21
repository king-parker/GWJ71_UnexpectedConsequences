using Godot;
using System;

public class GameManager : Node
{
    public static string TITLE_SCREEN = "res://scenes/menus/TitleScreen.tscn";
    public static string TUTORIAL_LEVEL = "res://scenes/levels/LevelTutorial.tscn";
    public static string LEVEL_ONE = "res://scenes/levels/Level1.tscn";
    public static string WIN_SCREEN = "res://scenes/menus/WinScreen.tscn";

    public enum Scenes
    {
        TitleScreen,
        Tutorial,
        LevelOne,
        WinScreen
    }

    public bool wasRageHidden { get; set; } = true;

    private int _rage = 0;
    private Timer _restartTimer;
    private Node _currentScene;
    private Scenes _currentSceneEnum;

    [Signal] public delegate void RageUpdated(int newRage);
    [Signal] public delegate void PlayerDeathEvent();

    public override void _Ready()
    {
        Viewport root = GetTree().Root;
        _currentScene = root.GetChild(root.GetChildCount() - 1); // Current scene is the last child in the root node
        _currentSceneEnum = Scenes.TitleScreen;

        _restartTimer = GetNode<Timer>("Timers/RestartTimer");
        _restartTimer.Connect("timeout", this, "OnRestartTimerTimeout");

        wasRageHidden = true;
    }

    public void GotoNextLevel()
    {
        switch (_currentSceneEnum)
        {
            case Scenes.TitleScreen:
                GotoScene(TUTORIAL_LEVEL, Scenes.Tutorial);
                break;
            case Scenes.Tutorial:
                GotoScene(LEVEL_ONE, Scenes.LevelOne);
                break;
            case Scenes.LevelOne:
                GotoScene(WIN_SCREEN, Scenes.WinScreen);
                break;
            case Scenes.WinScreen:
                GotoScene(TITLE_SCREEN, Scenes.TitleScreen);
                break;
        }
    }

    public void GotoScene(string path, Scenes newScene)
    {
        _currentSceneEnum = newScene;
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
        EmitSignal(nameof(PlayerDeathEvent));
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
