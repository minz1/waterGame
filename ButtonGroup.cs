using Godot;
using System;

public class ButtonGroup : Control
{
    // creates array of positions for the pages
    private Vector2[] PageLocations = new Vector2[5];
    // reference to the camera
    private Camera2D Cam;
    // list of buttons for page movement
    private Button[] Buttons = new Button[5];
    // tween node for fancy movement between pages
    private Tween CamTween;
    // variable keeping track of our current page
    private int CurrentPage = 0;

    // variable keeping track of which buttons we've enabled
    private int ButtsEnabled = 0;

    // function which enables the next buttons
    public void EnableNextButton()
    {
        Buttons[ButtsEnabled++].Visible = true;
    }

    public override void _Ready()
    {
        // creates references to camera and tween nodes
        Cam = GetNode<Camera2D>("../../Camera2D");
        CamTween = GetNode<Tween>("Tween");

        // generates all the buttons
        for (int i = 0; i < 5; i++)
        {
            Button button = new Button();
            button.Visible = false;
            button.SetSize(new Vector2(25f, 25f));
            button.SetPosition(new Vector2((280f * i), 0));
            button.Text = (i + 1).ToString();
            this.AddChild(button);
            Buttons[i] = button;

            // does the math for the different page locations
            Vector2 pageLocation = Cam.Offset;
            pageLocation.x = (Cam.Offset.x + (1280 * i));
            PageLocations[i] = pageLocation;
        }
    }

    public override void _PhysicsProcess(float delta)
    {
        // checks every button to see if its pressed
        for (int i = 0; i < 5; i++)
        {
            // if a button is pressed, and we're not already on it's page, continue
            if (Buttons[i].IsPressed() && (CurrentPage != i))
            {
                Vector2 originalPos = Cam.Offset;

                CurrentPage = i;
                // this is greek to me still, but this interpolates the original pos and the intended pos to create a smooth slide effect
                CamTween.InterpolateProperty(Cam, "offset", originalPos, PageLocations[i], 0.25f, Tween.TransitionType.Linear, Tween.EaseType.InOut);

                CamTween.Start();
            }
        }
    }
}
