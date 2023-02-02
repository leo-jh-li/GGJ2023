using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private TextMeshProUGUI m_dayDisplay;
    [SerializeField] private TextMeshProUGUI m_movesDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;

    private void OnEnable() {
        GameManager.instance.OnDayChange += UpdateDay;
        GameManager.instance.OnRemainingMovesChange += UpdateMoves;
        GameManager.instance.OnScoreChange += UpdateScore;
    }

    private void OnDisable() {
        GameManager.instance.OnDayChange -= UpdateDay;
        GameManager.instance.OnRemainingMovesChange -= UpdateMoves;
        GameManager.instance.OnScoreChange -= UpdateScore;
    }

    public void UpdateDay(int day) {
        m_dayDisplay.text = $"Day {day}";
    }

    public void UpdateMoves(int remainingMoves) {
        m_movesDisplay.text = $"Moves: {remainingMoves}";
    }

    public void UpdateScore(int score) {
        m_scoreDisplay.text = $"Score: {score}";
    }
}
