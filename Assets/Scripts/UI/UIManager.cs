using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private TextMeshProUGUI m_dayDisplay;
    [SerializeField] private TextMeshProUGUI m_movesDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    [SerializeField] private Transform m_seedTool;
    [SerializeField] private GameObject TEMP_gameOver;

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

    // Returns the world position the SeedTool UI is at
    public Vector3 GetSeedToolPos() {
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(m_seedTool.position);
        worldPos.z = 0;
        return worldPos;
    }

    public void SeedToolPulse() {
        m_seedTool.GetComponent<SeedTool>().PlayStoreSeedAnimation();
    }

    // TODO: temp
    public void ShowGameOver() {
        TEMP_gameOver.SetActive(true);
    }
}
