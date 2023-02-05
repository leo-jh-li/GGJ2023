using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI m_text;
    // Jam energy code since visibility is actually determined by the animation
    [SerializeField] private float m_popupDuration;
    [SerializeField] private GameObject m_loonieIcon;
    [SerializeField] private GameObject m_fertilizerBonus;

    private void Start() {
        StartCoroutine(DestroyAfterDuration());
    }

    public void InitializePopup(int points, bool fertilized, bool inDemand) {
        m_text.text = $"+{points}";
        if (fertilized) {
            m_fertilizerBonus.SetActive(true);
        }
        if (inDemand) {
            m_loonieIcon.SetActive(true);
        }
        
    }

    private IEnumerator DestroyAfterDuration() {
        yield return new WaitForSeconds(m_popupDuration);
        Destroy(gameObject);
    }
}
