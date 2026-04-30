using UnityEngine;

public class Player
{
    public string playerName;
    public int playerID;
    public int cashBalance;
    public Sprite avatar;

    public Player(string playerName, int playerID, int cashBalance)
    {
        this.playerName = playerName;
        this.playerID = playerID;
        this.cashBalance = cashBalance;
        Debug.Log("Welcome " + playerName);
    }

}
