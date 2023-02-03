using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextPopup : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI m_text;
    // Jam energy code since visibility is actually determined by the animation
    [SerializeField] private float m_popupDuration;

    private void Start() {
        StartCoroutine(DestroyAfterDuration());
    }

    public void SetText(string text) {
        m_text.text = text;
    }

    private IEnumerator DestroyAfterDuration() {
        yield return new WaitForSeconds(m_popupDuration);
        Destroy(gameObject);
    }
}
