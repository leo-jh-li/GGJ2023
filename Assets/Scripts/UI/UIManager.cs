using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private TextMeshProUGUI m_dayDisplay;
    [SerializeField] private TextMeshProUGUI m_movesDisplay;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    [SerializeField] private TextMeshProUGUI m_looniesDisplay;
    [SerializeField] private Transform m_seedTool;
    [SerializeField] private Transform m_rareSeedTool;
    [SerializeField] private GameObject TEMP_gameOver;
    [SerializeField, Tooltip("List of images that correspond to each PlantType that can be in demand (indices must match the enum values).")]
    private List<GameObject> m_plantImages;

    private void OnEnable() {
        GameManager.instance.OnDayChange += UpdateDay;
        GameManager.instance.OnRemainingMovesChange += UpdateMoves;
        GameManager.instance.OnScoreChange += UpdateScore;
        GameManager.instance.OnLooniesChange += UpdateLoonies;
    }

    private void OnDisable() {
        GameManager.instance.OnDayChange -= UpdateDay;
        GameManager.instance.OnRemainingMovesChange -= UpdateMoves;
        GameManager.instance.OnScoreChange -= UpdateScore;
        GameManager.instance.OnLooniesChange -= UpdateLoonies;
    }

    private void UpdateDay(int day) {
        m_dayDisplay.text = $"Day {day}";
    }

    private void UpdateMoves(int remainingMoves) {
        m_movesDisplay.text = $"Moves: {remainingMoves}";
    }

    private void UpdateScore(int score) {
        m_scoreDisplay.text = $"Score: {score}";
    }

    private void UpdateLoonies(int loonies) {
        m_looniesDisplay.text = $"Loonies: {loonies}";
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

    public void UpdateInDemandPlant(PlantType plantType) {
        // Deactivate all current plant images
        for (int i = 0; i < m_plantImages.Count; i++) {
            Debug.Log($"i: {i}");
            GameObject plantImage = m_plantImages[i];
            if (plantImage == null) { continue; }
            plantImage.SetActive(false);
        }
        // Activate in demand plant
        m_plantImages[(int) plantType].SetActive(true);
    }

    // TODO: temp
    public void ShowGameOver() {
        TEMP_gameOver.SetActive(true);
    }
}
