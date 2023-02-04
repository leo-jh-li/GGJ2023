using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEntity : MonoBehaviour {
    [Header("References")]
    // public GameObject temp_wateredIcon;
    public GameObject temp_harvestableIcon;
    // The GardenBed in which this plant is planted
    private GardenBed m_gardenBed;
    [SerializeField, Tooltip("List of all the children of StageSprites.")]
    private List<GameObject> m_stages;
    [SerializeField] private GameObject m_textPopupPrefab;

    [Header("Values")]
    [SerializeField] private int m_pointsValue;
    // This plant's current level of maturity (starts at 0 when freshly planted)
    private int m_growthLevel;
    // The growth level at which this plant is harvestable
    private int m_harvestLevel;

    // Returns a randomized number of seeds to gain after harvesting a plant
    public static int GenerateSeedHarvest() {
        // TODO: handle rare seeds...
        int harvestedSeeds = Random.Range(0, 4) + Random.Range(0, 3);
        if (GameManager.instance.seedQuantity == 0 && harvestedSeeds == 0) {
            // Floor the seed harvest at 1 if player has no seeds (note that they could have plants in the garden though)
            harvestedSeeds = 1;
        }
        return harvestedSeeds;
    }

    private void Awake() {
        // Calculate harvestLevel
        m_harvestLevel = m_stages.Count - 1;
        // Deactivate all stage sprites except for the first
        for (int i = 1; i < m_stages.Count; i++) {
            m_stages[i].SetActive(false);
        }
    }

    public void Initialize(GardenBed gardenBed) {
        m_gardenBed = gardenBed;
    }

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            // Call interaction behaviour on garden bed
            m_gardenBed.TryInteract();
        }
    }

    public bool CanWater() {
        return m_growthLevel < m_harvestLevel;
    }

    public bool CanHarvest() {
        return !CanWater();
    }

    public void Grow() {
        // TODO: grow
        // transform.localScale += new Vector3(0f, 1f, 0f);
        m_stages[m_growthLevel].SetActive(false);
        m_growthLevel++;
        m_stages[m_growthLevel].SetActive(true);
        if (CanHarvest()) {
            temp_harvestableIcon.SetActive(true);
        }
    }

    private int CalculatePointsValue() {
        return m_gardenBed.IsFertilized() ? m_pointsValue * 2 : m_pointsValue;
    }

    public void Harvest() {
        int points = CalculatePointsValue();
        TextPopup text = Instantiate(m_textPopupPrefab, transform.position, Quaternion.identity).GetComponent<TextPopup>();
        text.SetText($"+{points}");
        GameManager.instance.AddScore(points);
        Destroy(gameObject);
    }
}
