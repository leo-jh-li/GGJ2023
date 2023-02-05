using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextPopup : MonoBehaviour {
    [SerializeField] private RectTransform m_rectTransform;
    [SerializeField] private TextMeshProUGUI m_text;
    [SerializeField] private float m_popupDuration;
    [SerializeField] private GameObject m_loonieIcon;
    [SerializeField] private GameObject m_fertilizerBonus;

    private void Start() {
        StartCoroutine(DestroyAfterDuration());
        // Classic workaround for ContentSizeFitter not updating
        LayoutRebuilder.ForceRebuildLayoutImmediate(m_rectTransform);
    }

    public void InitializePopup(int points, bool fertilized, bool inDemand) {
        m_text.text = $"+{points}";
        m_fertilizerBonus.SetActive(fertilized);
        m_loonieIcon.SetActive(inDemand);
    }

    private IEnumerator DestroyAfterDuration() {
        yield return new WaitForSeconds(m_popupDuration);
        Destroy(gameObject);
    }
}
