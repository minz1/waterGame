using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
    // private constants for calculating the money the player has
    private float _Money = 0f;
    private float _ProfitRate = 1f;

    // Labels for different text labels in the game
    private Label MoneyLabel;
    private Label WellLabel;
    private Label ProfitLabel;

    // List of all the buttons in the game
    private List<WButton> Buttons = new List<WButton>();

    // References to the different buttons within the game
    private Control GUI;

    // points counting how many bad choices the player has made
    private int BadPoints = 0;

    // Basic property for money
    public float Money
    {
        get
        {
            return _Money;
        }
        set
        {
            if (value > 0)
                _Money = value;
        }
    }

    // Basic property for the value added to money every second
    public float ProfitRate
    {
        get
        {
            return _ProfitRate;
        }
        set
        {
            _ProfitRate = value;
        }
    }

    public void IncreaseBadPoints() { BadPoints++; }

    private void CreateButtons()
    {
        WButton wellButton = new WButton("Well", 10, 1, false);
        GUI.AddChild(wellButton);
        Buttons.Add(wellButton);
        wellButton.SetPosition(new Vector2(90f, 180f));
        wellButton.SetSize(new Vector2(220f, 100f));

        WButton ecoFiltButton = new WButton("Eco-Friendly Filtration", 25, 2, false);
        GUI.AddChild(ecoFiltButton);
        Buttons.Add(ecoFiltButton);
        ecoFiltButton.SetPosition(new Vector2(90f, 370f));
        ecoFiltButton.SetSize(new Vector2(220f, 100f));

        WButton cheapFiltButton = new WButton("Cheap Filtration", 15, 5, true);
        GUI.AddChild(cheapFiltButton);
        Buttons.Add(cheapFiltButton);
        cheapFiltButton.SetPosition(new Vector2(500f, 370f));
        cheapFiltButton.SetSize(new Vector2(220f, 100f));
    }

    public override void _Ready()
    {
        // Pull in our required labels
        MoneyLabel = GetNode<Label>("GUI/MoneyLabel");
        WellLabel = GetNode<Label>("GUI/WellLabel");
        ProfitLabel = GetNode<Label>("GUI/ProfitLabel");

        // Pull in GUI reference
        GUI = GetNode<Control>("GUI");

        CreateButtons();
    }

    public override void _PhysicsProcess(float delta)
    {
        Money += ProfitRate * delta;
        MoneyLabel.Text = $"{Money:C2}";
        ProfitLabel.Text = $"{ProfitRate:C2} per second";
        WellLabel.Text = $"Wells: {Buttons.ToArray()[0].GetNumUnits()}";
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
