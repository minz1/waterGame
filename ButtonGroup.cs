using Godot;
using System;

public class ButtonGroup : Control
{
    private Vector2[] PageLocations = new Vector2[5];
    private Camera2D Cam;
    private Button[] Buttons = new Button[5];
    private Tween CamTween;
    private int CurrentPage = 0;

    private int ButtsEnabled = 0;

    public void EnableNextButton()
    {
        Buttons[ButtsEnabled++].Visible = true;
    }

    public override void _Ready()
    {
        Cam = GetNode<Camera2D>("../../Camera2D");
        CamTween = GetNode<Tween>("Tween");

        for (int i = 0; i < 5; i++)
        {
            Button button = new Button();
            button.Visible = false;
            button.SetSize(new Vector2(25f, 25f));
            button.SetPosition(new Vector2((280f * i), 0));
            button.Text = (i + 1).ToString();
            this.AddChild(button);
            Buttons[i] = button;

            Vector2 pageLocation = Cam.Offset;
            pageLocation.x = (Cam.Offset.x + (1280 * i));
            PageLocations[i] = pageLocation;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        for (int i = 0; i < 5; i++)
        {
            if (Buttons[i].IsPressed() && (CurrentPage != i))
            {
                Vector2 originalPos = Cam.Offset;

                CurrentPage = i;
                CamTween.InterpolateProperty(Cam, "offset", originalPos, PageLocations[i], 0.25f, Tween.TransitionType.Linear, Tween.EaseType.InOut);

                CamTween.Start();
            }
        }
    }
}
