using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : Singleton<UIManager> {
    [SerializeField] private GameObject m_gameCanvas;
    [SerializeField] private TextMeshProUGUI m_dayDisplay;
    [SerializeField] private TextMeshProUGUI m_movesDisplay;
    [SerializeField, Tooltip("The text colour when player has 1 move left for the day.")]
    private Color m_lastMoveColour;
    private string m_lastMoveColorHex;
    [SerializeField] private TextMeshProUGUI m_scoreDisplay;
    [SerializeField] private TextMeshProUGUI m_looniesDisplay;
    [SerializeField] private Transform m_seedTool;
    [SerializeField] private Transform m_rareSeedTool;
    [SerializeField] private FavouredPlantDisplay m_favouredPlantDisplay;
    [SerializeField, Tooltip("Interactable buttons to disable on game end.")]
    private Button[] m_buttons;
    [SerializeField] private GameObject m_gameOverScreen;
    [SerializeField] private TextMeshProUGUI m_finalScore;

    private void Awake () {
        m_lastMoveColorHex = ColorUtility.ToHtmlStringRGB(m_lastMoveColour);
    }

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
        // Make text red if there's one move left
        if (remainingMoves == 1) {
            m_movesDisplay.text = $"Moves: <color=#{m_lastMoveColorHex}>{remainingMoves}";
        } else {
            m_movesDisplay.text = $"Moves: {remainingMoves}";
        }
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

    public void UpdateInDemandPlant(PlantType plantType, bool playAnimation) {
        StartCoroutine(m_favouredPlantDisplay.UpdateInDemandPlant(plantType, playAnimation));
    }

    public void OnGameOver() {
        m_gameCanvas.SetActive(false);
        foreach (Button button in m_buttons) {
            button.interactable = false;
        }
        m_gameOverScreen.SetActive(true);
        m_finalScore.text = GameManager.instance.score.ToString();
    }
}
