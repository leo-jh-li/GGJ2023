using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

// Seed Tool for normal seeds
public class SeedTool : MonoBehaviour {
    [SerializeField] protected SeedToolItem m_seedEntityPrefab;
    [SerializeField] private TextMeshProUGUI m_seedQuantityDisplay;
    [SerializeField] private Animation m_animation;

    protected virtual void OnEnable() {
        GameManager.instance.OnSeedQuantityChange += UpdateSeedQuantity;
    }

    protected virtual void OnDisable() {
        GameManager.instance.OnSeedQuantityChange -= UpdateSeedQuantity;
    }

    protected void UpdateSeedQuantity(int quantity) {
        m_seedQuantityDisplay.text = quantity.ToString();
    }

    protected virtual bool CanRetrieveSeed() {
        return GameManager.instance.seedQuantity > 0;
    }

    public virtual void RetrieveSeed() {
        if (!CanRetrieveSeed()) { return; }
        GameManager.instance.seedQuantity--;
        Instantiate(m_seedEntityPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }

    // Plays animation for when a seed enters the seed collection
    public void PlayStoreSeedAnimation() {
        m_animation.Play();
    }
}
