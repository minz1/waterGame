using Godot;
using System;

public class WButton : Button
{
    // the initial price of the button
    private readonly float BasePrice;
    // the amount of money the item will give you per sec
    private readonly float CashPerSecond;
    // name of the item
    private readonly string _UnitName;
    // whether or not the item has an adverse effect on the environment
    private readonly bool IsBad;
    // reference to game
    private Game Game;

    // amount of the item purchased
    private int _NumUnits = 0;
    // the current price of the item purchased
    private float CurrentPrice = 0f;

    // basic property for units
    public int NumUnits
    {
        get
        {
            return _NumUnits;
        }
    }

    // basic property for the name
    public string UnitName
    {
        get
        {
            return _UnitName;
        }
    }

    // method for handling the removal of money from the player's wallet
    public float Purchase(float money)
    {
        float moneyLeft = 0f;

        // checks if the player can buy it
        if (money >= CurrentPrice)
        {
            // if we can, buy it, increase the price, and have the player make more money as well
            moneyLeft = (money - CurrentPrice);
            _NumUnits += 1;

            CurrentPrice += (CurrentPrice * 0.5f);
        }
        else
        {
            // if the player doesn't have the money, return a value the code will recognize as incorrect
            moneyLeft = -1;
        }

        return moneyLeft;
    }

    public override void _Ready()
    {
        // ties our game reference to the actual node
        Game = GetTree().GetRoot().GetNode<Game>("Game");
        // ties the button release event to the OnButtonRelease method
        Connect("button_up", this, nameof(OnButtonRelease));
        // sets our current price to the base price initially
        CurrentPrice = BasePrice;
    }

    public override void _PhysicsProcess(float delta)
    {
        // sets our text to the name, cost, and how much it makes the player
        Text = $"{UnitName} (Cost: {CurrentPrice:C2}, makes {CashPerSecond:C2}/s)";
    }

    public void OnButtonRelease()
    {
        // calls our purchase method
        float money = Purchase(Game.Money);

        // if we don't have the money, the method returns -1, so stop if this is the case
        if (money != -1f)
        {
            // sets the games amount of money to the new amount
            Game.Money = money;
            // increases the players profit amount
            Game.ProfitRate += CashPerSecond;

            // gives the player more bad points if this item isn't good
            if (IsBad)
            {
                Game.IncreaseBadPoints();
            }
        }
    }

    // default constructor for the wbutton, as i am unsure on whether or not the editor expects an empty constructor
    public WButton() : this("Default", 1f, 1f, false) { }

    // basic constructor
    public WButton(string unitName, float basePrice, float cashPerSecond, bool isBad) : base()
    {
        this._UnitName = unitName;
        this.BasePrice = basePrice;
        this.CashPerSecond = cashPerSecond;
        this.IsBad = isBad;
    }
}