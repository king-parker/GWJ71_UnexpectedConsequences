using Godot;
using System;

public class GameManager : Node
{
    private int _rage = 0;

    [Signal] public delegate void RageUpdated(int newRage);

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
}
