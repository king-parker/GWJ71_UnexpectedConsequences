using Godot;
using System;

public class PlayerAttackHitBox : Area2D
{
    public override void _Ready()
    {
        this.Connect("area_entered", this, "OnAreaEntered");
    }

    private void OnAreaEntered(Area2D area)
    {
        GD.Print("Slime hit!");
        Slime slime = area.GetParent<Slime>();
        if (slime != null)
        {
            slime.Death();
        }
    }
}
