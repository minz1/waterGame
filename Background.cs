using Godot;
using System;

public class Background : AnimatedSprite
{
    private Camera2D Cam;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        Cam = GetNode<Camera2D>("../../Camera2D");
    }


    public override void _PhysicsProcess(float delta)
    {
        Vector2 Pos = new Vector2(Cam.Offset.x - 640, Position.y);

        SetPosition(Pos);
    }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
