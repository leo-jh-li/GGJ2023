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
    private List<Sprite> m_plantImages;
    [SerializeField] private Image m_inDemandPlantImage;

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

    public void UpdateDay(int day) {
        m_dayDisplay.text = $"Day {day}";
    }

    public void UpdateMoves(int remainingMoves) {
        m_movesDisplay.text = $"Moves: {remainingMoves}";
    }

    public void UpdateScore(int score) {
        m_scoreDisplay.text = $"Score: {score}";
    }

    public void UpdateLoonies(int loonies) {
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
        m_inDemandPlantImage.sprite = m_plantImages[(int) plantType];
    }

    // TODO: temp
    public void ShowGameOver() {
        TEMP_gameOver.SetActive(true);
    }
}
