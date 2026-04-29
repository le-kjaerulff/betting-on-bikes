using System;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private List<Player>  players = new List<Player>();
    private List<Bet> _activeBets = new List<Bet>();
    private Player activePlayer;
    
    public Cyclist[] cyclists;

    public Cyclist betCyclist;
    public string betOther;
    public float betAmount;
    
    [ContextMenu("Place Test Bet")]
    void PlaceTestBet()
    {
        PlaceBet(betAmount, betCyclist, AccidentType.Collision, betOther);
    }
    
    [ContextMenu("Begin Round Test")]
    void BeginRoundTest()
    {
        BeginRound();
    }
    
    void Start()
    {
        players.Add(new Player("Mattia", 0, 200));
        //players.Add(new Player("Ciro", 1, 200));
        activePlayer = players[0];
        Debug.Log("Active player: " + activePlayer.playerName);
        foreach (var cyclist in cyclists)
        {
            cyclist.OnCollision += HandleCollision;
        }
    }
    
    void PlaceBet(float amount, Cyclist cyclist, AccidentType accidentType, string otherPartyTag = null)
    {
        Bet newBet = new Bet(activePlayer, amount, cyclist, accidentType, otherPartyTag); // calls the constructor
        _activeBets.Add(newBet);
        Debug.Log("Bet placed: "+ activePlayer.playerName + " bets " + amount + " on " + cyclist.tag + " colliding with " + otherPartyTag);
    }
    
    void BeginRound()
    {
        foreach (var cyclist in cyclists)
        {
            cyclist.isAlive = true;
        }
    }
    
    
    void HandleCollision(Cyclist cyclist, string otherTag)
    {
        Debug.Log(cyclist.tag + " collided with " + otherTag);
        // check active bets here and pay out
    }
    
    
    
}