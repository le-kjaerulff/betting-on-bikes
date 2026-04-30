using UnityEngine;

public class Bet
{
    public Player player;
    public int amount;
    public Cyclist cyclist;
    public AccidentType accidentType;
    public string otherTag;
    public int odds = 2;

    public Bet(Player player, int amount, Cyclist cyclist, AccidentType accidentType, string otherTag = null)
    {
        this.player = player;
        this.amount = amount;
        this.cyclist = cyclist;
        this.accidentType = accidentType;
        this.otherTag = otherTag;
    }
}
