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
    [Tooltip("Chance for a harvested seed to become rare.")]
    public float rareSeedChance;
    [Tooltip("The max number of rare seeds that can drop in a game.")]
    public int maxRareSeedDrops;
    [Tooltip("The current number of rare seeds that have dropped for the player this game.")]
    public int totalRareSeedDrops;
    [Tooltip("When seeds are harvested on or past this day and the player hasn't harvested a single rare seed yet, then guarantee a rare seed drop in the harvest.")]
    public int chosenGuaranteedRareSeedDay;
    [Tooltip("The range of the possible guaranteed rare seed days that can be chosen. Note range is max exclusive.")]
    public Vector2Int chosenGuaranteedRareSeedDayRange;

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

    [SerializeField, Tooltip("The number of normal seeds the player begins the game with.")]
    private int m_startingSeedQuantity = 0;
    private int m_seedQuantity;
    public int seedQuantity {
        get {return m_seedQuantity;}
        set {
            if (m_seedQuantity == value) return;
            m_seedQuantity = value;
            if (OnSeedQuantityChange != null)
                OnSeedQuantityChange(m_seedQuantity);
        }
    }
    public delegate void OnSeedQuantityChangeDelegate(int newVal);
    public event OnSeedQuantityChangeDelegate OnSeedQuantityChange;

    private int m_rareSeedQuantity;
    public int rareSeedQuantity {
        get {return m_rareSeedQuantity;}
        set {
            if (m_rareSeedQuantity == value) return;
            m_rareSeedQuantity = value;
            if (OnRareSeedQuantityChange != null)
                OnRareSeedQuantityChange(m_rareSeedQuantity);
        }
    }
    public delegate void OnRareSeedQuantityChangeDelegate(int newVal);
    public event OnRareSeedQuantityChangeDelegate OnRareSeedQuantityChange;

    [SerializeField, Tooltip("The number of fertilizer the player begins the game with.")]
    private int m_startingFertilizerQuantity = 0;
    private int m_fertilizerQuantity;
    public int fertilizerQuantity {
        get {return m_fertilizerQuantity;}
        set {
            if (m_fertilizerQuantity == value) return;
            m_fertilizerQuantity = value;
            if (OnFertilizerQuantityChange != null)
                OnFertilizerQuantityChange(m_fertilizerQuantity);
        }
    }
    public delegate void OnFertilizerQuantityChangeDelegate(int newVal);
    public event OnFertilizerQuantityChangeDelegate OnFertilizerQuantityChange;

    private void Start() {
        m_numberOfDays = 28;
        day = 1;
        score = 0;
        RefreshMoves();
        seedQuantity = m_startingSeedQuantity;
        rareSeedQuantity = 0;
        // TODO: test - remove
        // rareSeedQuantity = 3;
        fertilizerQuantity = m_startingFertilizerQuantity;
        // Randomize guaranteed rare seed day
        chosenGuaranteedRareSeedDay = Random.Range(chosenGuaranteedRareSeedDayRange.x, chosenGuaranteedRareSeedDayRange.y);
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

    private bool IsFinalDay() {
        return day == m_numberOfDays;
    }

    private void EndDay() {
        Debug.Log($"End day {day}");
        foreach (GardenBed gardenBed in m_gardenBeds) {
            gardenBed.OnNewDay();
        }
        if (!IsFinalDay()) {
            day++;
            RefreshMoves();
        } else {
            EndGame();
        }
    }

    private void EndGame() {
        // TODO
        UIManager.instance.ShowGameOver();
        Debug.Log("game over");
    }

    public void DebugSkipMove() {
        OnPerformMove();
    }

    public void DebugEndDay() {
        EndDay();
    }

    public PlantEntity GetRandomPlant() {
        return m_plantLibrary.GetRandomPlant();
    }

    public PlantEntity GetRarePlant() {
        return m_plantLibrary.GetRarePlant();
    }
}
