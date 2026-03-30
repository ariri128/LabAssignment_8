using UnityEngine;

public class Upgrade
{
    public string upgradeName;
    public float cost;
    public float revenueMultiplier;
    public bool purchased;

    public Upgrade(string name, float cost, float multiplier)
    {
        this.upgradeName = name;
        this.cost = cost;
        this.revenueMultiplier = multiplier;
        this.purchased = false;
    }
}
