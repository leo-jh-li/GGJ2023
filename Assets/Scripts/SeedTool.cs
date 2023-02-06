using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class SeedTool : MonoBehaviour {
    [SerializeField] protected SeedToolItem m_seedEntityPrefab;
    [SerializeField] private TextMeshProUGUI m_seedQuantityDisplay;
    [SerializeField] private Animator m_anim;

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
        m_anim.SetTrigger("Shrink");
        GameManager.instance.seedQuantity--;
        Instantiate(m_seedEntityPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }

    // Plays animation for when a seed enters the seed collection
    public void PlayStoreSeedAnimation() {
        m_anim.SetTrigger("Pulse");
    }
}
