using Godot;
using System;

public class WLabel : Label
{
    private readonly WButton WButton;

    public override void _PhysicsProcess(float delta)
    {
        Text = $"{WButton.UnitName}: {WButton.NumUnits}";
    }

    public WLabel() : this(null) { }

    public WLabel(WButton wbutton) : base()
    {
        this.WButton = wbutton;
        Vector2 LabelPos = new Vector2(WButton.GetPosition().x, (WButton.GetPosition().y + WButton.GetSize().y + 2f));
        this.SetPosition(LabelPos);
    }
}