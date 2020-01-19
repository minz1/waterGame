using Godot;
using System;

public class Camera : Control
{
    private Camera2D Cam;
    private HScrollBar ScrlBar;
    private Vector2 OriginalPos;

    public override void _Ready()
    {
        Cam = GetNode<Camera2D>("Camera2D");
        ScrlBar = GetNode<HScrollBar>("CanvasLayer/HScrollBar");
        OriginalPos = Cam.Offset;
    }

    public override void _PhysicsProcess(float delta)
    {
        Vector2 nextPos = OriginalPos;
        nextPos.x = OriginalPos.x + (ScrlBar.Value);

        Cam.Offset = nextPos;
    }
}