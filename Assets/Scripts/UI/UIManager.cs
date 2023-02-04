using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private TextMeshProUGUI m_dayDisplay;
    [SerializeField] private TextMeshProUGUI m_movesDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    [SerializeField] private Transform m_seedTool;
    [SerializeField] private Transform m_rareSeedTool;
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
    public Vector3 GetSeedToolPos(bool seedIsRare) {
        Vector3 worldPos = seedIsRare ? Camera.main.ScreenToWorldPoint(m_rareSeedTool.position) : Camera.main.ScreenToWorldPoint(m_seedTool.position);
        worldPos.z = 0;
        return worldPos;
    }

    public void SeedToolPulse(bool seedIsRare) {
        SeedTool seedTool = seedIsRare ? m_rareSeedTool.GetComponent<SeedTool>() : m_seedTool.GetComponent<SeedTool>();
        seedTool.PlayStoreSeedAnimation();
    }

    // TODO: temp
    public void ShowGameOver() {
        TEMP_gameOver.SetActive(true);
    }
}
