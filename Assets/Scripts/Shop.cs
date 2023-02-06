using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Shop : MonoBehaviour {
    [SerializeField] private Button m_buyFertilizerButton;
    [SerializeField] private TextMeshProUGUI m_fertilizerCostText;
    [SerializeField] private int m_fertilizerCost;
    [SerializeField] private Image m_fertilizerIcon;
    [SerializeField] private Color m_unpurchaseableColour;
    [SerializeField] private Animation m_animation;

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
        bool purchaseable = CanBuyFertilizer();
        m_fertilizerIcon.color = purchaseable ? Color.white : m_unpurchaseableColour;
        m_buyFertilizerButton.interactable = purchaseable;
    }

    private void UpdateAvailability(int loonies) {
        UpdateAvailability();
    }

    private bool CanBuyFertilizer() {
        return GameManager.instance.loonies >= m_fertilizerCost;
    }

    public void TryBuyFertilizer() {
        if (CanBuyFertilizer()) {
            m_animation.Play();
            GameManager.instance.loonies -= m_fertilizerCost;
            AudioManager.instance.Play(Constants.instance.LOONIE);
            GameManager.instance.fertilizerQuantity++;
            UpdateAvailability();
        }
    }
}
