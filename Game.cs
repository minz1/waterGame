using Godot;
using System;
using System.Collections.Generic;

public class Game : Node2D
{
    // initial amount of money player has
    private float _Money = 990f;
    // the amount of money player earns per second
    private float _ProfitRate = 1f;

    // the player's current level of progression
    private int CurrentStage = 1;
    // the threshold needed to progress to the next stage
    private int NextStageRequirement = 1000;

    // sprite for the game background
    private AnimatedSprite Background;

    // Labels for different text labels in the game
    private Label MoneyLabel;
    private Label ProfitLabel;
    private Label StageLabel;
    private Label GoalLabel;
    private Label TargetRateLabel;

    // Reference to the GUI node to add buttons and labels to
    private Control ButtonHolder;
    // reference to button group for UI navigation
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
            if (value < 0)
            {
                _Money = 0;
            }
            else
            {
                _Money = value;
            }
        }
    }

    // Basic property for the profit rate added to money every second
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

    // increases bad points whenever called by a bad button
    public void IncreaseBadPoints() { BadPoints++; }

    // the base x values buttons can be created on
    private float[] ButtXVals = {90f, 500f, 910f};
    // the y values buttons can be created on
    private float[] ButtYVals = {200f, 330f, 460f};
    // current index of our button creation function for the x
    private int XIndex = 0;
    // current index of our button creation function for the y
    private int YIndex = 0;

    // generates each button, its label, and give it a spot in the gamespace.
    // currently, can only support up to 9 spots per stage due to the hard coded nature of the positions.
    private void CreateButton(string buttName, float baseCost, float cashPerSec, bool isBad)
    {
        if (YIndex > 2)
        {
            YIndex = 0;
            XIndex++;
        }

        WButton button = new WButton(buttName, baseCost, cashPerSec, isBad);
        ButtonHolder.AddChild(button);

        float xPos = ButtXVals[XIndex] + (1280f * (CurrentStage - 1f));
        button.SetPosition(new Vector2(xPos, ButtYVals[YIndex]));
        button.SetSize(new Vector2(220f, 100f));

        WLabel label = new WLabel(button);
        ButtonHolder.AddChild(label);

        YIndex++;
    }

    // creates all the buttons for stage 1
    private void CreateFirstStageButtons()
    {
        CreateButton("Wells", 10f, 1f, false);

        CreateButton("Eco-Friendly Filtration", 25f, 2f, false);

        CreateButton("Cheap Filtration", 15f, 5f, true);
    }

    // creates all the buttons for stage 2
    private void CreateSecondStageButtons()
    {
        CreateButton("UV Filtration", 100f, 20f, false);

        CreateButton("Vapor Filtration", 75f, 25f, true);

        CreateButton("Tap Water", 200f, 40f, false);

        CreateButton("Plastic Bottles", 250f, 30f, true);
    }

    // creates all the buttons for stage 3
    private void CreateThirdStageButtons()
    {
        CreateButton("Clean Landfills", 1500f, 300f, false);

        CreateButton("Collect Rain Water", 1200f, 200f, false);

        CreateButton("Cheap Plastic Bottles", 450f, 200f, true);

        CreateButton("3rd World Plants", 1000f, 400f, true);


        // presents the first stage where the players may see different choices based on their previous choices
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

    // creates all the buttons for stage 4
    private void CreateFourthStageButtons()
    {
        CreateButton("Styrofoam Bottles", 9000f, 3000f, true);

        CreateButton("Hydro-Electric Power", 14000f, 2500f, false);

        // if player continues to be bad, they get into the lose condition of the game: ruining the planet
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


    // creates all the buttons for stage 5
    private void CreateFifthStageButtons()
    {
        CreateButton("Plant Trees", 1000f, 15000f, false);

        CreateButton("Clean Landfills", 5000f, 20000f, false);

        CreateButton("Renewable Bottles", 5000f, 1200f, false);

        CreateButton("Hydroelectric Energy", 250000f, 300000f, false);

        CreateButton("Clean Oceans", 5000f, 1000f, false);

        CreateButton("Recycle Plastics", 100000f, 50000f, false);
    }

    // function for handing the move between stages
    private void IncreaseStage()
    {
        // increments our stage number variable
        CurrentStage++;

        // resets indexes for x + y position
        YIndex = 0;
        XIndex = 0;

        // enables the next button in the button group, moves us to the next stage and changes the name
        if (CurrentStage == 2)
        {
            Background.Frame = 1;
            ButtGroup.EnableNextButton();
            CreateSecondStageButtons();
            NextStageRequirement = 20000;
            StageLabel.Text = "Stage: River";
            ProfitRate += 10f;
        }

        // same as before, but with a possible split if the player makes bad choices
        if (CurrentStage == 3)
        {
            ButtGroup.EnableNextButton();
            CreateThirdStageButtons();
            NextStageRequirement = 250000;
            ProfitRate += 200;
            if (BadPoints >= 10)
            {
                Background.Frame = 2;
                StageLabel.Text = "Stage: Ocean";
            }
            else
            {
                Background.Frame = 3;
                StageLabel.Text = "Stage: Spring Water";
            }
        }

        // same as stage 3
        if (CurrentStage == 4)
        {
            ButtGroup.EnableNextButton();
            CreateFourthStageButtons();
            NextStageRequirement = 5000000;
            ProfitRate += 5000;
            if (BadPoints >= 20)
            {
                Background.Frame = 5;
                StageLabel.Text = "Stage: Glacier";
            }
            else
            {
                Background.Frame = 4;
                StageLabel.Text = "Stage: Dam";
            }
        }

        // flips the profit if the player is bad, beginning the loss condition for bad players. activates end-game philanthropist content for good players
        if (CurrentStage == 5)
        {
            if (BadPoints >= 20) {
                ProfitRate = -(ProfitRate * 0.1f);
                StageLabel.Text = "Stage 5: ???";
            }
            else
            {
                Background.Frame = 5;
                ButtGroup.EnableNextButton();
                CreateFifthStageButtons();
                StageLabel.Text = "Stage 5: Philanthropy";
                NextStageRequirement = Int32.MaxValue;
                ProfitRate += 10000;
            }
        }
    }

    private void EndGameBad()
    {
        // TODO
    }

    public override void _Ready()
    {
        // pulls in background object
        Background = GetNode<AnimatedSprite>("GUI/CanvasLayer/Background");
        
        // Pulls in our labels
        MoneyLabel = GetNode<Label>("GUI/CanvasLayer/MoneyLabel");
        ProfitLabel = GetNode<Label>("GUI/CanvasLayer/ProfitLabel");
        StageLabel = GetNode<Label>("GUI/CanvasLayer/StageLabel");
        GoalLabel = GetNode<Label>("GUI/CanvasLayer/GoalLabel");
        TargetRateLabel = GetNode<Label>("GUI/CanvasLayer/TargetRateLabel");

        // Pull in GUI reference
        ButtonHolder = GetNode<Control>("GUI/CanvasLayer/ButtonHolder");

        // Get Button Group Reference
        ButtGroup = GetNode<ButtonGroup>("ScrollCamera/CanvasLayer/ButtonGroup");

        // enables the first button
        ButtGroup.EnableNextButton();

        // enables the first set of game buttons
        CreateFirstStageButtons();
    }

    public override void _PhysicsProcess(float delta)
    {
        // increments the money value
        Money += ProfitRate * delta;

        // label text setting handling
        MoneyLabel.Text = $"{Money:C2}";
        ProfitLabel.Text = $"{ProfitRate:C2} per second";

        // sets the goal to something mysterious if a bad player hits the loss condition
        if ((CurrentStage == 5) && (BadPoints >= 20))
        {
            GoalLabel.Text = "Next Goal: ???";
            ProfitRate = -ProfitRate;
        }
        else
        {
            // shows the next bar to reach for profit
            GoalLabel.Text = $"Next Goal: {NextStageRequirement:C2}";
        }

        // increments the stage when the player reaches a certain threshold
        if (Money > NextStageRequirement)
        {
            IncreaseStage();
        }

        // ends the game once the bad player loses all their money
        if ((CurrentStage == 5) && (Money < 0))
        {
            EndGameBad();
        }
    }
}
