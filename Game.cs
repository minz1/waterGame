using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
    // private constants for calculating the money the player has
    private float _Money = 0f;
    private float _ProfitRate = 1f;

    private int CurrentStage = 1;
    private int NextStageRequirement = 1000;

    // Labels for different text labels in the game
    private Label MoneyLabel;
    private Label ProfitLabel;
    private Label StageLabel;

    // List of all the buttons in the game
    private List<WButton> Stage2Buttons = new List<WButton>();

    // References to the different nodes within the game
    private Control GUI;
    private ScrollCamera ScrollCamera;

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

    private float[] ButtXVals = {90f, 500f, 910f};
    private float[] ButtYVals = {200f, 330f, 460f};
    private int XIndex = 0;
    private int YIndex = 0;

    private void CreateButton(string buttName, float baseCost, float cashPerSec, bool isBad)
    {
        if (YIndex > 2)
        {
            YIndex = 0;
            XIndex++;
        }

        WButton button = new WButton(buttName, baseCost, cashPerSec, isBad);
        GUI.AddChild(button);

        float xPos = ButtXVals[XIndex] + (1280f * (CurrentStage - 1f));
        button.SetPosition(new Vector2(xPos, ButtYVals[YIndex]));
        button.SetSize(new Vector2(220f, 100f));

        WLabel label = new WLabel(button);
        GUI.AddChild(label);

        YIndex++;
    }


    private void CreateFirstStageButtons()
    {
        CreateButton("Wells", 10f, 1f, false);

        CreateButton("Eco-Friendly Filtration", 25f, 2f, false);

        CreateButton("Cheap Filtration", 15f, 5f, true);
    }

    private void CreateSecondStageButtons()
    {
        CreateButton("UV Filtration", 100f, 20f, false);

        CreateButton("Vapor Filtration", 75f, 25f, true);

        CreateButton("Tap Water", 200f, 40f, false);

        CreateButton("Bottled Water", 250f, 30f, true);
    }

    private void CreateThirdStageButtons()
    {
        if (BadPoints >= 5)
        {
            //CreateButton("");
        }
    }

    private void IncreaseStage()
    {
        CurrentStage++;

        YIndex = 0;
        XIndex = 0;

        if (CurrentStage == 2)
        {
            CreateSecondStageButtons();
            NextStageRequirement = 10000;
            StageLabel.Text = "Stage: River";
            ProfitRate += 10f;
            ScrollCamera.ScrollBarVisibility = true;
        }
        if (CurrentStage == 3)
        {
            if (BadPoints >= 5)
            {
                StageLabel.Text = "Stage: Ocean";
            }
            else
            {
                StageLabel.Text = "Stage: Spring Water";
            }
        }
    }

    public override void _Ready()
    {
        // Pull in our required labels
        MoneyLabel = GetNode<Label>("GUI/CanvasLayer/MoneyLabel");
        ProfitLabel = GetNode<Label>("GUI/CanvasLayer/ProfitLabel");
        StageLabel = GetNode<Label>("GUI/CanvasLayer/StageLabel");

        // Pull in GUI reference
        GUI = GetNode<Control>("GUI");

        // Pull in Camera
        ScrollCamera = GetNode<ScrollCamera>("ScrollCamera");

        CreateFirstStageButtons();
    }

    public override void _PhysicsProcess(float delta)
    {
        Money += ProfitRate * delta;
        MoneyLabel.Text = $"{Money:C2}";
        ProfitLabel.Text = $"{ProfitRate:C2} per second";

        if (Money > NextStageRequirement)
        {
            IncreaseStage();
        }
    }
}
