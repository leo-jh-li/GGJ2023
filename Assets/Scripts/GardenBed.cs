using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GardenBed : MonoBehaviour {
    [SerializeField] private GameObject m_wateredBed;
    private bool m_hasPlant;

    public void SetWatered(bool watered) {
        m_wateredBed.SetActive(watered);
    }

    public void OnHarvest() {
        m_hasPlant = false;
    }

    public bool HasPlant() {
        return m_hasPlant;
    }

    public void PlantSeed() {
        // TODO: plant, randomize seed, possibly handle rare seeds
        // TODO: save ref?
        PlantEntity plant = Instantiate(GameManager.instance.GetRandomPlant(), transform);
        plant.Initialize(this);
        m_hasPlant = true;
        GameManager.instance.OnPerformMove();
    }
}
