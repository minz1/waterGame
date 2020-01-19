using Godot;
using System;

public class WButton : Button
{
    //

    // static properties of each button
    private readonly float BasePrice;
    private readonly float CashPerSecond;
    private readonly string _UnitName;
    private readonly bool IsBad;
    private Game Game;

    // more dynamic properties of each button
    private int _NumUnits = 0;
    private float CurrentPrice = 0f;

    public int NumUnits
    {
        get
        {
            return _NumUnits;
        }
    }

    public string UnitName
    {
        get
        {
            return _UnitName;
        }
    }

    public float Purchase(float money)
    {
        float moneyLeft = 0f;

        if (money >= CurrentPrice)
        {
            moneyLeft = (money - CurrentPrice);
            _NumUnits += 1;

            CurrentPrice += (CurrentPrice * 0.5f);
        }
        else
        {
            moneyLeft = -1;
        }

        return moneyLeft;
    }

    public override void _Ready()
    {
        Game = GetTree().GetRoot().GetNode<Game>("Game");
        Connect("button_up", this, nameof(OnButtonRelease));
        CurrentPrice = BasePrice;
    }

    public override void _PhysicsProcess(float delta)
    {
        Text = $"{UnitName} (Cost: {CurrentPrice:C2}, makes {CashPerSecond:C2}/s)";
    }

    public void OnButtonRelease()
    {
        float money = Purchase(Game.Money);

        if (money != -1f)
        {
            Game.Money = money;
            Game.ProfitRate += CashPerSecond;

            if (IsBad)
            {
                Game.IncreaseBadPoints();
            }
        }
    }

    public WButton() : this("Default", 1f, 1f, false) { }

    public WButton(string unitName, float basePrice, float cashPerSecond, bool isBad) : base()
    {
        this._UnitName = unitName;
        this.BasePrice = basePrice;
        this.CashPerSecond = cashPerSecond;
        this.IsBad = isBad;
    }
}