using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlantType {
    NONE,
    ONE_STAGE,
    TWO_STAGE,
    THREE_STAGE,
    FOUR_STAGE,
    RARE
}

public class PlantEntity : MonoBehaviour {
    [Header("References")]
    // The GardenBed in which this plant is planted
    private GardenBed m_gardenBed;
    [SerializeField, Tooltip("List of all the children of StageSprites.")]
    private List<GameObject> m_stages;
    [SerializeField] private GameObject m_textPopupPrefab;

    [Header("Values")]
    [SerializeField] private PlantType m_plantType;
    [SerializeField] private int m_pointsValue;
    // This plant's current level of maturity (starts at 0 when freshly planted)
    private int m_growthLevel;
    // The growth level at which this plant is harvestable
    private int m_harvestLevel;

    // Returns a randomized number of normal seeds and rare seeds to gain after harvesting a plant.
    // Pair of quantities is returned as a Vector2Int (x is normal quantity, y is rare quantity).
    public static Vector2Int GenerateSeedHarvest() {
        Vector2Int harvestedSeeds = Vector2Int.zero;
        harvestedSeeds.x = Random.Range(0, 3) + Random.Range(0, 2);
        if (GameManager.instance.seedQuantity == 0 && harvestedSeeds.x == 0) {
            // Floor the seed harvest at 1 if player has no seeds (note that they could have plants in the garden though)
            harvestedSeeds.x = 1;
        }

        // Apply chance to transform some normal seeds into rare seeds
        int seedsConverted = 0;
        for (int i = 0; i < harvestedSeeds.x; i++) {
            bool guaranteeRareSeed = GameManager.instance.day >= GameManager.instance.chosenGuaranteedRareSeedDay && GameManager.instance.totalRareSeedDrops == 0;
            if ((Random.value <= GameManager.instance.rareSeedChance &&
                 GameManager.instance.totalRareSeedDrops < GameManager.instance.maxRareSeedDrops) ||
                 guaranteeRareSeed) {
                seedsConverted++;
                GameManager.instance.totalRareSeedDrops++;
            }
        }

        harvestedSeeds.x -= seedsConverted;
        harvestedSeeds.y += seedsConverted;
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
        m_stages[m_growthLevel].SetActive(false);
        m_growthLevel++;
        m_stages[m_growthLevel].SetActive(true);
    }

    private int CalculatePointsValue() {
        return m_gardenBed.IsFertilized() ? m_pointsValue * 2 : m_pointsValue;
    }

    public void Harvest() {
        bool inDemand = m_plantType == GameManager.instance.inDemandPlant;
        int points = CalculatePointsValue();
        TextPopup text = Instantiate(m_textPopupPrefab, transform.position, Quaternion.identity).GetComponent<TextPopup>();
        text.InitializePopup(points, m_gardenBed.IsFertilized(), inDemand);
        GameManager.instance.AddScore(points);

        // Handle in demand plants
        if (inDemand) {
            AudioManager.instance.Play(Constants.instance.LOONIE);
            GameManager.instance.AddLoonie(1);
        } else {
            // Play harvest sound instead of loonie sound if no loonie was earned
            AudioManager.instance.Play(Constants.instance.HARVEST_PLANT);
        }

        Destroy(gameObject);
    }
}
