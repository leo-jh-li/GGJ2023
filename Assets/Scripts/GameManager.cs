using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Linq;

public class GameManager : Singleton<GameManager> {

    [Header("References")]
    [SerializeField] private GameObject m_menu;
    [SerializeField] private GameObject m_game;
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
    public RandomRangeInt chosenGuaranteedRareSeedDayRange;
    [Tooltip("The number of days a plant stays in demand before the plant is changed.")]
    public int inDemandDuration;
    // List of PlantTypes that have yet to be in demand this game
    private List<PlantType> m_inDemandQueue;
    public PlantType inDemandPlant;

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

    private int m_loonies;
    public int loonies {
        get {return m_loonies;}
        set {
            if (m_loonies == value) return;
            m_loonies = value;
            if (OnLooniesChange != null)
                OnLooniesChange(m_loonies);
        }
    }
    public delegate void OnLooniesChangeDelegate(int newVal);
    public event OnLooniesChangeDelegate OnLooniesChange;

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

    private void Awake() {
        // Populate in demand queue with every plant that can be in demand
        m_inDemandQueue = new List<PlantType>(new PlantType[] { PlantType.ONE_STAGE, PlantType.TWO_STAGE, PlantType.THREE_STAGE, PlantType.FOUR_STAGE });
        Utils.Shuffle<PlantType>(m_inDemandQueue);
    }

    public void StartGame() {
        m_menu.SetActive(false);
        m_game.SetActive(true);
        InitializeValues();
        OnNewDay();
    }

    private void InitializeValues() {
        score = 0;
        m_numberOfDays = 28;
        seedQuantity = m_startingSeedQuantity;
        rareSeedQuantity = 0;
        fertilizerQuantity = m_startingFertilizerQuantity;
        // Randomize guaranteed rare seed day
        chosenGuaranteedRareSeedDay = chosenGuaranteedRareSeedDayRange.GetRandom();
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

    public void AddLoonie(int value) {
        loonies += value;
    }

    private bool IsFinalDay() {
        return day == m_numberOfDays;
    }

    private PlantType PopFavouredPlant() {
        PlantType ret = m_inDemandQueue[0];
        m_inDemandQueue.RemoveAt(0);
        return ret;
    }

    private bool IsDayToChangeFavouredPlant(int day) {
        return (day - 1) % inDemandDuration == 0;
    }

    public int LastDayOfCurrentInDemandPlant() {
        int lastDay = day + 1;
        while (!(IsDayToChangeFavouredPlant(lastDay))) {
            lastDay++;
        }
        lastDay -= 1;
        return lastDay;
    }

    private void HandleFavouredPlant() {
        if (IsDayToChangeFavouredPlant(day)) {
            if (m_inDemandQueue.Count == 0) {
                Debug.LogWarning("No valid PlantType to become in demand.");
                return;
            }

            PlantType nextInDemand = PopFavouredPlant();
            inDemandPlant = nextInDemand;
            // Don't play animation if it's day 1
            UIManager.instance.UpdateInDemandPlant(inDemandPlant, day != 1);

            Debug.Log($"New favoured plant: {inDemandPlant}");
        }
    }

    private void OnNewDay() {
        day++;
        RefreshMoves();
        HandleFavouredPlant();
    }

    public void EndDay() {
        if (day > m_numberOfDays) { return; }
        Debug.Log($"End day {day}");
        foreach (GardenBed gardenBed in m_gardenBeds) {
            gardenBed.OnNewDay();
        }
        if (!IsFinalDay()) {
            OnNewDay();
        } else {
            EndGame();
        }
    }

    private void EndGame() {
        UIManager.instance.OnGameOver();
    }

    public void ReturnToMenu() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public PlantEntity GetRandomPlant() {
        return m_plantLibrary.GetRandomPlant();
    }

    public PlantEntity GetRarePlant() {
        return m_plantLibrary.GetRarePlant();
    }
}
