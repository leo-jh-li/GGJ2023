using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedTool : MonoBehaviour {
    [SerializeField] private SeedToolItem m_seedEntityPrefab;
    [SerializeField] private TextMeshProUGUI m_seedQuantityDisplay;

    private void OnEnable() {
        GameManager.instance.OnSeedQuantityChange += UpdateSeedQuantity;
    }

    private void OnDisable() {
        GameManager.instance.OnSeedQuantityChange -= UpdateSeedQuantity;
    }

    private void UpdateSeedQuantity(int quantity) {
        m_seedQuantityDisplay.text = quantity.ToString();
    }

    private bool CanRetrieveSeed() {
        return GameManager.instance.seedQuantity > 0;
    }

    public void RetrieveSeed() {
        if (!CanRetrieveSeed()) { return; }
        GameManager.instance.seedQuantity--;
        Instantiate(m_seedEntityPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }
}
