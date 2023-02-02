using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GameManager : Singleton<GameManager> {

    [Header("References")]
    [SerializeField] private PlantLibrary m_plantLibrary;
    [SerializeField] private List<GardenBed> m_gardenBeds;

    [Header("Game Values")]
    [SerializeField, Tooltip("The number of days over which the game takes place.")]
    private int m_numberOfDays;
    [SerializeField, Tooltip("The number of the current day.")]
    private int m_day;
    public int day {
        get {return m_day;}
        set {
            if (m_day == value) return;
            m_day = value;
            if (OnDayChange != null)
                OnDayChange(m_day);
        }
    }
    public delegate void OnDayChangeDelegate(int newVal);
    public event OnDayChangeDelegate OnDayChange;

    [SerializeField, Tooltip("The number of moves the player gets to make per day.")]
    private int m_movesPerDay;
    [SerializeField, Tooltip("Number of moves that the player can still make in the current day.")]
    private int m_remainingMoves;
    public int remainingMoves {
        get {return m_remainingMoves;}
        set {
            if (m_remainingMoves == value) return;
            m_remainingMoves = value;
            if (OnRemainingMovesChange != null)
                OnRemainingMovesChange(m_remainingMoves);
        }
    }
    public delegate void OnRemainingMovesChangeDelegate(int newVal);
    public event OnRemainingMovesChangeDelegate OnRemainingMovesChange;

    private int m_score;
    public int score {
        get {return m_score;}
        set {
            if (m_score == value) return;
            m_score = value;
            if (OnScoreChange != null)
                OnScoreChange(m_score);
        }
    }
    public delegate void OnScoreChangeDelegate(int newVal);
    public event OnScoreChangeDelegate OnScoreChange;

    private void Start() {
        m_numberOfDays = 28;
        day = 1;
        score = 0;
        RefreshMoves();
    }

    private void RefreshMoves() {   
        remainingMoves = m_movesPerDay;
    }

    public void OnPerformMove() {
        remainingMoves--;
        if (remainingMoves <= 0) {
            if (day == m_numberOfDays) {
                EndGame();
            } else {
                EndDay();
            }
        }
    }

    public void AddScore(int value) {
        score += value;
    }

    private void EndDay() {
        Debug.Log($"End day {day}");
        foreach (GardenBed gardenBed in m_gardenBeds) {
            gardenBed.OnNewDay();
        }
        day++;
        RefreshMoves();
    }

    private void EndGame() {
        // TODO
    }

    public void DebugSkipMove() {
        OnPerformMove();
    }

    public PlantEntity GetRandomPlant() {
        return m_plantLibrary.GetRandomPlant();
    }
}
