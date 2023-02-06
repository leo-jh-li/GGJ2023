using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBed : MonoBehaviour {
    
    [Header("References")]
    // The plant planted here, or null if there isn't one
    private PlantEntity m_plantEntity;
    [SerializeField] private GameObject m_wateredBed;
    [SerializeField] private GameObject m_harvestableParticleSystem;
    [SerializeField] private GameObject m_fertilizedParticleSystem;
    [SerializeField] private SeedBurst m_seedBurst;

    private bool m_watered;
    private bool m_fertilized;

    private void OnMouseOver() {
        if (Input.GetMouseButtonDown(0)) {
            TryInteract();
        }
    }

    public bool HasPlant() {
        return m_plantEntity != null;
    }

    public void PlantSeed(bool seedIsRare) {
        PlantEntity plantToPlant = seedIsRare ? GameManager.instance.GetRarePlant() : GameManager.instance.GetRandomPlant();
        PlantEntity plant = Instantiate(plantToPlant, transform);
        m_plantEntity = plant;
        plant.Initialize(this);
        AudioManager.instance.Play(Constants.instance.PLANT_SEED);
        GameManager.instance.OnPerformMove();
    }

    // Tries to perform context-sensitive interaction with this PlantEntity and returns true iff the interaction was successful.
    public void TryInteract() {
        if (!CanInteract()) { return; }
        Interact();
    }

    private bool CanInteract() {
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
        AudioManager.instance.Play(Constants.instance.WATER_PLANT);
        SetWatered(true);
    }

    private void SetWatered(bool watered) {
        m_wateredBed.SetActive(watered);
        m_watered = watered;
    }

    private void SetFertilized(bool fertilized) {
        m_fertilized = fertilized;
        m_fertilizedParticleSystem.SetActive(fertilized);
    }

    public bool IsFertilized() {
        return m_fertilized;
    }

    public bool CanFertilizePlant() {
        return HasPlant() && !IsFertilized();
    }

    public void TryFertilizePlant() {
        if (!CanFertilizePlant()) { return; }
        SetFertilized(true);
        AudioManager.instance.Play(Constants.instance.FERTILIZE_PLANT);
        GameManager.instance.OnPerformMove();
    }

    private bool CanHarvestPlant() {
        if (m_plantEntity == null) {
            return false;
        }
        return m_plantEntity.CanHarvest();
    }

    private void HarvestPlant() {
        m_plantEntity.Harvest();
        m_plantEntity = null;
        SetFertilized(false);
        m_harvestableParticleSystem.SetActive(false);

        // Spawn seeds from harvesting
        Vector2Int seedsHarvested = PlantEntity.GenerateSeedHarvest();
        m_seedBurst.CreateSeedBurst(seedsHarvested.x, seedsHarvested.y);
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
        if (CanHarvestPlant()) {
            m_harvestableParticleSystem.SetActive(true);
        }
        SetWatered(false);
    }
}
