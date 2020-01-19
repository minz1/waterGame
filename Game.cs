using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
    // private constants for calculating the money the player has
    private float _Money = 1000000f;
    private float _ProfitRate = 1f;

    private int CurrentStage = 1;
    private int NextStageRequirement = 1000;

    // Labels for different text labels in the game
    private Label MoneyLabel;
    private Label ProfitLabel;
    private Label StageLabel;
    private Label GoalLabel;

    // List of all the buttons in the game
    private List<WButton> Stage2Buttons = new List<WButton>();

    // References to the different nodes within the game
    private Control GUI;
    private ButtonGroup ButtGroup;

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

        CreateButton("Plastic Bottles", 250f, 30f, true);
    }

    private void CreateThirdStageButtons()
    {
        CreateButton("Clean Landfills", 1500f, 300f, false);

        CreateButton("Collect Rain Water", 1200f, 200f, false);

        CreateButton("Cheap Plastic Bottles", 450f, 200f, true);

        CreateButton("3rd World Plants", 1000f, 400f, true);


        if (BadPoints >= 10)
        {
            CreateButton("Florescent Lights", 500f, 250f, true);

            CreateButton("Gas Powered Pumps", 700f, 350f, true);
        }
        else
        {
            CreateButton("Solar-Powered Pumps", 700f, 200f, false);

            CreateButton("Local Water Plants", 1200f, 250f, false);
        }
    }

    private void CreateFourthStageButtons()
    {
        CreateButton("Styrofoam Bottles", 9000f, 3000f, true);

        CreateButton("Hydro-Electric Power", 14000f, 2500f, false);

        if (BadPoints >= 20)
        {
            CreateButton("Mega Plants", 10000f, 7000f, true);

            CreateButton("Cheap Piping", 8000f, 4800f, true);

            CreateButton("De-Regulation Lobbying", 11000f, 10000f, true);

            CreateButton("Glacier Melting", 11000f, 20000f, true);
        }
        else
        {
            CreateButton("Tighter Quality Control", 13000f, 4000f, false);

            CreateButton("Wind Powered Plants", 12000f, 4500f, false);

            CreateButton("Electric Transport", 11000f, 3000f, false);

            CreateButton("Sanitation Plants", 1000f, 2800f, false);
        }
    }

    private void CreateFifthStageButtons()
    {
        CreateButton("Plant Trees", 1000f, 15000f, false);

        CreateButton("Clean Landfills", 5000f, 20000f, false);

        CreateButton("Renewable Bottles", 5000f, 1200f, false);

        CreateButton("Hydroelectric Energy", 250000f, 300000f, false);

        CreateButton("Clean Oceans", 5000f, 1000f, false);

        CreateButton("Recycle Plastics", 100000f, 50000f, false);
    }

    private void IncreaseStage()
    {
        CurrentStage++;

        YIndex = 0;
        XIndex = 0;

        if (CurrentStage == 2)
        {
            ButtGroup.EnableNextButton();
            CreateSecondStageButtons();
            NextStageRequirement = 20000;
            StageLabel.Text = "Stage: River";
            ProfitRate += 10f;
        }

        if (CurrentStage == 3)
        {
            ButtGroup.EnableNextButton();
            CreateThirdStageButtons();
            NextStageRequirement = 250000;
            ProfitRate += 200;
            if (BadPoints >= 10)
            {
                StageLabel.Text = "Stage: Ocean";
            }
            else
            {
                StageLabel.Text = "Stage: Spring Water";
            }
        }

        if (CurrentStage == 4)
        {
            ButtGroup.EnableNextButton();
            CreateThirdStageButtons();
            NextStageRequirement = 1000000;
            ProfitRate += 5000;
            if (BadPoints >= 20)
            {
                StageLabel.Text = "Stage: Glacier";
            }
            else
            {
                StageLabel.Text = "Stage: Dam";
            }
        }

        if (CurrentStage == 5)
        {
            if (BadPoints >= 20) {
                ProfitRate = -ProfitRate;
            }
            else
            {
                ButtGroup.EnableNextButton();
                CreateFifthStageButtons();
                StageLabel.Text = "Stage 5: Philanthropy";
                NextStageRequirement = Int32.MaxValue;
                ProfitRate += 10000;
            }
        }
    }

    private void EndGame()
    {
        // TODO
    }

    public override void _Ready()
    {
        // Pull in our required labels
        MoneyLabel = GetNode<Label>("GUI/CanvasLayer/MoneyLabel");
        ProfitLabel = GetNode<Label>("GUI/CanvasLayer/ProfitLabel");
        StageLabel = GetNode<Label>("GUI/CanvasLayer/StageLabel");
        GoalLabel = GetNode<Label>("GUI/CanvasLayer/GoalLabel");

        // Pull in GUI reference
        GUI = GetNode<Control>("GUI");

        // Get Button Group Reference
        ButtGroup = GetNode<ButtonGroup>("ScrollCamera/CanvasLayer/ButtonGroup");

        ButtGroup.EnableNextButton();

        CreateFirstStageButtons();
    }

    public override void _PhysicsProcess(float delta)
    {
        Money += ProfitRate * delta;
        MoneyLabel.Text = $"{Money:C2}";
        ProfitLabel.Text = $"{ProfitRate:C2} per second";
        if ((CurrentStage == 5) && (BadPoints >= 20))
        {
            GoalLabel.Text = "Next Goal: ???";
        }
        else
        {
            GoalLabel.Text = $"Next Goal: {NextStageRequirement:C2}";
        }

        if (Money > NextStageRequirement)
        {
            IncreaseStage();
        }

        if ((CurrentStage == 5) && (Money < 0))
        {
            EndGame();
        }
    }
}
