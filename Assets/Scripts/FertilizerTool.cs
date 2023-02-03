using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FertilizerTool : MonoBehaviour {
    [SerializeField] private Fertilizer m_fertilizerPrefab;
    [SerializeField] private TextMeshProUGUI m_fertilizerQuantityDisplay;

    private void OnEnable() {
        GameManager.instance.OnFertilizerQuantityChange += UpdateFertilizerQuantity;
    }

    private void OnDisable() {
        GameManager.instance.OnFertilizerQuantityChange -= UpdateFertilizerQuantity;
    }

    private void UpdateFertilizerQuantity(int quantity) {
        m_fertilizerQuantityDisplay.text = quantity.ToString();
    }

    private bool CanRetrieveFertilizer() {
        return GameManager.instance.fertilizerQuantity > 0;
    }

    public void RetrieveFertilizer() {
        if (!CanRetrieveFertilizer()) { return; }
        GameManager.instance.fertilizerQuantity--;
        Instantiate(m_fertilizerPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }
}
