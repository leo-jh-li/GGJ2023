using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEntity : MonoBehaviour {
    // public GameObject temp_wateredIcon;
    public GameObject temp_harvestableIcon;
    [SerializeField] private int m_pointsValue;
    private int m_growthLevel;
    // The level at which this plant is harvestable
    private int m_harvestLevel = 3;
    // The GardenBed in which this plant is planted
    private GardenBed m_gardenBed;

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
        m_growthLevel++;
        transform.localScale += new Vector3(0f, 1f, 0f);
        if (CanHarvest()) {
            temp_harvestableIcon.SetActive(true);
        }
    }

    public void Harvest() {
        GameManager.instance.AddScore(m_pointsValue);
        // Grant seeds from harvesting
        // TODO: make more sophisticated, also guarantee seeds/increase seeds if player has low/no seeds
        GameManager.instance.seedQuantity += Random.Range(0, 6);
        // TODO: temp
        Destroy(gameObject);
    }
}
