using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    [SerializeField] private Button m_buyFertilizerButton;
    [SerializeField] private TextMeshProUGUI m_fertilizerCostText;
    [SerializeField] private int m_fertilizerCost;

    private void Awake() {
        m_fertilizerCostText.text = m_fertilizerCost.ToString();
        UpdateAvailability();
    }

    private void OnEnable() {
        GameManager.instance.OnLooniesChange += UpdateAvailability;
    }

    private void OnDisable() {
        GameManager.instance.OnLooniesChange -= UpdateAvailability;
    }

    private void UpdateAvailability() {
        m_buyFertilizerButton.interactable = CanBuyFertilizer();
    }

    private void UpdateAvailability(int loonies) {
        m_buyFertilizerButton.interactable = CanBuyFertilizer();
    }

    private bool CanBuyFertilizer() {
        return GameManager.instance.loonies >= m_fertilizerCost;
    }

    public void TryBuyFertilizer() {
        if (CanBuyFertilizer()) {
            GameManager.instance.loonies -= m_fertilizerCost;
            AudioManager.instance.Play(Constants.instance.LOONIE);
            GameManager.instance.fertilizerQuantity++;
            UpdateAvailability();
        }
    }
}
