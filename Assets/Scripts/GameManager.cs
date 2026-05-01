using System;   
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Unity.Cinemachine;


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
    
    public TextMeshProUGUI playerNameDisplay;
    public TextMeshProUGUI playerBankDisplay;
    public Sprite[] playerAvatars;
    public Image avatarImage;

    public TextMeshProUGUI playerNameSummary;
    public TextMeshProUGUI amountSummary;
    public TextMeshProUGUI cyclistsSummary;
    public TextMeshProUGUI accidentSummary;
    
    private CinemachineTargetGroup _targetGroup;
    
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
        amountSummary.text = "Betting " +  betAmount.ToString() + " on";
    }

    public void DecreaseBetButton()
    {
        if(betAmount <= 0) return;
        betAmount -= 10;
        betAmountDisplay.text = betAmount.ToString();
        amountSummary.text = "Betting " +  betAmount.ToString() + " on";
    }

    public void SelectCyclistButton(int idNum)
    {
        betCyclist = cyclists[idNum];
        cyclistsSummary.text =  betCyclist.name; 
    }

    public void SelectColissionTargetButton()
    {
        betOther = betOtherSelect.options[betOtherSelect.value].text;
        Debug.Log(betOtherSelect.options[betOtherSelect.value].text);
        accidentSummary.text = "colliding with " + betOther;
        
    }


    void Start()
    {
        _targetGroup = GameObject.Find("Target Group").GetComponent<CinemachineTargetGroup>();
        foreach (var cyclist in cyclists)
        {
            cyclist.OnCollision += HandleCollision;
            cyclist.OnArrival += CheckIfRoundOver;
        }
        _players.Add(new Player("Player 1", 0, 200));
        _players.Add(new Player("Player 2", 1, 200));
        _players.Add(new Player("Player 3", 2, 200));
       _players.Add(new Player("Player 4", 3, 200));
        
        BeginBettingRound();
    }
    
    private void BeginBettingRound()
    {
        _activeBets.Clear();
        foreach (var cyclist in cyclists)
        {
            cyclist.Initialize();
        }
        foreach (var target in _targetGroup.Targets)
        {
            target.Weight = 1;
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
        if (amount == 0 || cyclist == null || otherPartyTag == null)
        {
            Debug.Log("Invalid bet");
            return;
        }
        
        Bet newBet = new Bet(_activePlayer, amount, cyclist, accidentType, otherPartyTag); // calls the constructor
        _activeBets.Add(newBet);
        _activePlayer.cashBalance -= amount;
        Debug.Log("Bet placed: "+ _activePlayer.playerName + " bets " + amount + " on " + cyclist.id + " colliding with " + otherPartyTag);
        
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
        avatarImage.sprite = playerAvatars[_activePlayer.playerID];
        playerNameDisplay.text = _activePlayer.playerName;
        playerBankDisplay.text = _activePlayer.cashBalance.ToString();
        playerNameSummary.text = _activePlayer.playerName;
        betAmount = 0;
        betAmountDisplay.text = betAmount.ToString();
        amountSummary.text = "Betting 0 on";
        betCyclist = null;
        cyclistsSummary.text = "[Choose a cyclist]";
        betOther = null;
        betOtherSelect.value = 0;
        accidentSummary.text = "[Choose an accident]";
        
        Debug.Log("It is " + _activePlayer.playerName + "s turn to place a bet! Your cash balance is: " + _activePlayer.cashBalance);
    }
    
    private void BeginRaceRound()
    {
        uiCanvas.SetActive(false);
        foreach (var cyclist in cyclists)
        {
            cyclist.GetComponent<PolygonCollider2D>().enabled = true;
            cyclist.isAlive = true;
        }
    }
    
    void HandleCollision(Cyclist cyclist, string otherTag)
    {
        Debug.Log(cyclist.id + " collided with " + otherTag);
        for (int i = 0; i < _activeBets.Count; i++)
        {
            if (_activeBets[i].cyclist == cyclist && _activeBets[i].otherTag == otherTag)
            {
                _activeBets[i].player.cashBalance += _activeBets[i].amount * _activeBets[i].odds;
                Debug.Log(_activeBets[i].player + " won their bet! Winnings: " + _activeBets[i].amount * _activeBets[i].odds);
                _activeBets.RemoveAt(i);
            }
        }
        cyclist.isAlive = false;
        CheckIfRoundOver();
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    private void CheckIfRoundOver()
    {
        foreach (var cyclist in cyclists)
        {
            if (cyclist.isAlive)
            {
                Invoke("UpdateTargetGroup", 3.0f);
                return;
            }
        }
        Debug.Log("Race round is over!");
        BeginBettingRound();
        Invoke("InitTargetGroup", 3.0f);
    }

    void UpdateTargetGroup()
    {
        foreach (var cyclist in cyclists)
        {
            if (!cyclist.isAlive) _targetGroup.Targets[cyclist.index].Weight = 0;
        }
    }

    void InitTargetGroup()
    {
        foreach (var cyclist in cyclists)
        {
            if (!cyclist.isAlive) _targetGroup.Targets[cyclist.index].Weight = 1;
        }
    }


}