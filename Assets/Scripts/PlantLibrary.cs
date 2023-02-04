using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour {
    [SerializeField] private List<PlantEntity> m_plantsList;
    [SerializeField] private PlantEntity m_rarePlant;

    // Returns a random PlantEntity from the list
    public PlantEntity GetRandomPlant() {
        return m_plantsList[Random.Range(0, m_plantsList.Count)];
    }

    // Returns the rare plant
    public PlantEntity GetRarePlant() {
        return m_rarePlant;
    }
}
