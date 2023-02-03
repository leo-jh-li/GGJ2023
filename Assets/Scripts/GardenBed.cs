using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBed : MonoBehaviour {
    
    [Header("References")]
    // The plant planted here, or null if there isn't one
    private PlantEntity m_plantEntity;
    [SerializeField] private GameObject TEMP_wateredBedSprite;

    private bool m_watered;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            TryInteract();
        }
    }

    public bool HasPlant() {
        return m_plantEntity != null;
    }

    public void PlantSeed() {
        // TODO: plant, randomize seed, possibly handle rare seeds
        PlantEntity plant = Instantiate(GameManager.instance.GetRandomPlant(), transform);
        m_plantEntity = plant;
        plant.Initialize(this);
        GameManager.instance.OnPerformMove();
    }

    // Tries to perform context-sensitive interaction with this PlantEntity and returns true iff the interaction was successful.
    public void TryInteract() {
        if (!CanInteract()) { return; }
        Interact();
    }

    private bool CanInteract() {
        // TODO: check player has actions, game is active (e.g. no animations/waits going on, game not paused etc.)
        if (!HasPlant() ||
            m_watered ||
            GameManager.instance.remainingMoves <= 0 ) { return false; }
        return true;
    }

    private bool CanWaterPlant() {
        return m_plantEntity.CanWater();
    }

    private void Water() {
        Debug.Log($"Watered { gameObject.name }");
        // TODO: temp
        SetWatered(true);
    }

    private void SetWatered(bool watered) {
        // TODO: remove
        // temp_wateredIcon.SetActive(watered);
        TEMP_wateredBedSprite.SetActive(watered);
        m_watered = watered;
    }

    // TODO: temp?
    private bool CanHarvestPlant() {
        return m_plantEntity.CanHarvest();
    }

    private void HarvestPlant() {
        m_plantEntity.Harvest();
        m_plantEntity = null;
    }

    private void Interact() {
        if (CanWaterPlant()) {
            Water();
        } else if (CanHarvestPlant()) {
            HarvestPlant();
        } else {
            return;
        }
        GameManager.instance.OnPerformMove();
    }

    public void OnNewDay() {
        if (m_watered) {
            m_plantEntity.Grow();
        }
        SetWatered(false);
    }
}