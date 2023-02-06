using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class FavouredPlantDisplay : MonoBehaviour {
    [SerializeField] private RectTransform m_favouredPlantDisplay;
    private bool m_favouredPlantDisplayInitialized;
    private Vector3 m_parchmentStartPos;
    [SerializeField, Tooltip("The movement made during the parchment animation.")]
    private Vector3 m_parchmentMovement;
    private Vector3 m_parchmentDestination;
    [SerializeField] private float m_parchmentSlideSpeed;
    private bool m_parchmentInMotion;
    [SerializeField] private TextMeshProUGUI m_untilDate;
    [SerializeField, Tooltip("List of images that correspond to each PlantType that can be in demand (indices must match the enum values).")]
    private List<GameObject> m_plantImages;

    private void Start() {
        // Initialize positions
        m_parchmentStartPos = m_favouredPlantDisplay.position;
        m_parchmentDestination = m_parchmentStartPos + m_parchmentMovement;
        m_favouredPlantDisplayInitialized = true;
    }

    public IEnumerator UpdateInDemandPlant(PlantType plantType, bool playAnimation) {
        bool lockMotion = false;
        if (m_parchmentInMotion) {
            lockMotion = true;
        }
        // Play animation of paper sliding out then in unless otherwise indicated (i.e., if it's day 1)
        if (playAnimation && !lockMotion) {
            m_parchmentInMotion = true;
            while (m_favouredPlantDisplay.position != m_parchmentDestination) {
                m_favouredPlantDisplay.position = Vector3.MoveTowards(m_favouredPlantDisplay.position, m_parchmentDestination, m_parchmentSlideSpeed * Time.deltaTime);
                yield return null;
            }
        }
        // Deactivate all current plant images
        for (int i = 0; i < m_plantImages.Count; i++) {
            Debug.Log("i: {i}");
            GameObject plantImage = m_plantImages[i];
            if (plantImage == null) { continue; }
            plantImage.SetActive(false);
        }
        // Activate in demand plant
        m_plantImages[(int) plantType].SetActive(true);
        m_untilDate.text = $"Until Day { GameManager.instance.NextDayToChangeFavouredPlant() }";
        if (playAnimation && !lockMotion) {
            while (m_favouredPlantDisplay.position != m_parchmentStartPos) {
                m_favouredPlantDisplay.position = Vector3.MoveTowards(m_favouredPlantDisplay.position, m_parchmentStartPos, m_parchmentSlideSpeed * Time.deltaTime);
                yield return null;
            }
            m_parchmentInMotion = false;
        }
    }

}

