using System;   
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class GameManager : MonoBehaviour
{
    private List<Player>  _players = new List<Player>();
    private List<Bet> _activeBets = new List<Bet>();
    private Player _activePlayer;
    
    public Cyclist[] cyclists;

    public Cyclist betCyclist;
    public string betOther;
    public int betAmount;
    
    private bool _openForBets = false;

    public GameObject uiCanvas;
    public TextMeshProUGUI betAmountDisplay;
    public TMP_Dropdown betOtherSelect;
    
    
    [ContextMenu("Place Bet Test")]
    public void PlaceBetButton()
    {
        PlaceBet(betAmount, betCyclist, AccidentType.Collision, betOther);
    }
    
    [ContextMenu("Begin Round Test")]
    public void BeginRoundTest()
    {
        BeginRaceRound();
    }

    public void IncreaseBetButton()
    {
        if (betAmount >= _activePlayer.cashBalance) return;
        betAmount += 10;
        betAmountDisplay.text = betAmount.ToString();
    }

    public void DecreaseBetButton()
    {
        if(betAmount <= 0) return;
        betAmount -= 10;
        betAmountDisplay.text = betAmount.ToString();
    }

    public void SelectCyclistButton(int idNum)
    {
        betCyclist = cyclists[idNum];
    }

    public void SelectColissionTargetButton()
    {
        betOther = betOtherSelect.options[betOtherSelect.value].text;
        Debug.Log(betOtherSelect.options[betOtherSelect.value].text);
    }


    void Start()
    {
        foreach (var cyclist in cyclists)
        {
            cyclist.OnCollision += HandleCollision;
            cyclist.OnArrival += CheckIfRoundOver;
        }
        _players.Add(new Player("Player1", 0, 200));
        //_players.Add(new Player("Player2", 1, 200));
       // _players.Add(new Player("Player3", 2, 200));
        //_players.Add(new Player("Player4", 3, 200));
        
        BeginBettingRound();
    }
    
    private void BeginBettingRound()
    {
        foreach (var cyclist in cyclists)
        {
            cyclist.Initialize();
        }
        uiCanvas.SetActive(true);
        _openForBets = true;
        Debug.Log("Betting is open!");
        PassTurn(0);
    }
    
    private void PlaceBet(int amount, Cyclist cyclist, AccidentType accidentType, string otherPartyTag = null)
    {
        if (!_openForBets)
        {
            Debug.Log("Can't place bet now, betting has closed for this round");
            return;
        }
        if (amount > _activePlayer.cashBalance)
        {
            Debug.Log("Insufficient cash");
            return;
        }
        
        Bet newBet = new Bet(_activePlayer, amount, cyclist, accidentType, otherPartyTag); // calls the constructor
        _activeBets.Add(newBet);
        _activePlayer.cashBalance -= amount;
        Debug.Log("Bet placed: "+ _activePlayer.playerName + " bets " + amount + " on " + cyclist.tag + " colliding with " + otherPartyTag);
        
        if (_activePlayer.playerID >= _players.Count - 1)
        {
            Debug.Log("All bets are placed and betting is closed, starting race round");
            _openForBets = false;
            BeginRaceRound();
            return;
        }
        PassTurn(_activePlayer.playerID+1);
    }

    
    private void PassTurn(int id)
    {
        _activePlayer = _players[id];
        betAmount = 0;
        betAmountDisplay.text = betAmount.ToString();
        Debug.Log("It is " + _activePlayer.playerName + "s turn to place a bet! Your cash balance is: " + _activePlayer.cashBalance);
    }
    
    private void BeginRaceRound()
    {
        uiCanvas.SetActive(false);
        foreach (var cyclist in cyclists)
        {
            cyclist.isAlive = true;
        }
    }
    
    void HandleCollision(Cyclist cyclist, string otherTag)
    {
        Debug.Log(cyclist.tag + " collided with " + otherTag);
        foreach (var bet in _activeBets)
        {
            if (bet.cyclist == cyclist && bet.otherTag == otherTag) bet.player.cashBalance += bet.amount * bet.odds ;
        }
        CheckIfRoundOver();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckIfRoundOver()
    {
        foreach (var cyclist in cyclists)
        {
            if (cyclist.isAlive)
            {
                return;
            }
        }
        Debug.Log("Race round is over!");
        BeginBettingRound();
    }
    

}