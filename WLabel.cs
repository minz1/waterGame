using Godot;
using System;

public class WLabel : Label
{
    // holds a reference to a button
    private readonly WButton WButton;

    public override void _PhysicsProcess(float delta)
    {
        // sets our text to the amount of units purchased
        Text = $"{WButton.UnitName}: {WButton.NumUnits}";
    }

    // no argument constructor, just in case the editor expects this
    public WLabel() : this(null) { }

    // basic constructor for this custom label
    public WLabel(WButton wbutton) : base()
    {
        this.WButton = wbutton;
        Vector2 LabelPos = new Vector2(WButton.GetPosition().x, (WButton.GetPosition().y + WButton.GetSize().y + 2f));
        this.SetPosition(LabelPos);
        
        this.SetSize(new Vector2(0f, 14f));
    }
}