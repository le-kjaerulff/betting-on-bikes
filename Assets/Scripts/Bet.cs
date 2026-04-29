using UnityEngine;

public class Bet
{
    public Player player;
    public float amount;
    public Cyclist cyclist;
    public AccidentType accidentType;
    public string otherTag;

    public Bet(Player player, float amount, Cyclist cyclist, AccidentType accidentType, string otherTag = null)
    {
        this.player = player;
        this.amount = amount;
        this.cyclist = cyclist;
        this.accidentType = accidentType;
        this.otherTag = otherTag;
    }
}
