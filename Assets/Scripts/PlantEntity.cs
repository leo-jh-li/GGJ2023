using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantEntity : MonoBehaviour {
    private bool m_watered;
    public GameObject temp_wateredIcon;
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
            TryInteract();
        }
    }

    // Tries to perform context-sensitive interaction with this PlantEntity and returns true iff the interaction was successful.
    private void TryInteract() {
        if (!CanInteract()) { return; }
        Interact();
    }

    private bool CanInteract() {
        // TODO: check player has actions, game is active (e.g. no animations/waits going on, game not paused etc.)
        if (m_watered || GameManager.instance.remainingMoves <= 0) { return false; }
        return true;
    }

    private bool CanWater() {
        return m_growthLevel < m_harvestLevel;
    }

    private void Water() {
        Debug.Log($"Watered { gameObject.name }");
        // TODO: temp
        SetWatered(true);
    }

    private void SetWatered(bool watered) {
        m_gardenBed.SetWatered(watered);
        // TODO: remove
        temp_wateredIcon.SetActive(watered);
        m_watered = watered;
    }

    // TODO: temp?
    private bool CanHarvest() {
        return !CanWater();
    }

    private void Harvest() {
        GameManager.instance.AddScore(m_pointsValue);
        // TODO: temp
        Destroy(gameObject);
        m_gardenBed.OnHarvest();
    }

    private void Interact() {
        // TODO: perform interaction
        if (CanWater()) {
            Water();
        } else if (CanHarvest()) {
            Harvest();
        } else {
            return;
        }
        GameManager.instance.OnPerformMove();
    }

    private void Grow() {
        // TODO: grow
        m_growthLevel++;
        transform.localScale += new Vector3(0f, 1f, 0f);
        if (CanHarvest()) {
            temp_harvestableIcon.SetActive(true);
        }
    }

    public void OnNewDay() {
        if (m_watered) {
            Grow();
        }
        SetWatered(false);
    }
}
