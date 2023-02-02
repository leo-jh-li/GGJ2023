using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLibrary : MonoBehaviour {
    [SerializeField] private List<PlantEntity> m_plantsList;

    // Returns a random PlantEntity from the list
    public PlantEntity GetRandomPlant() {
        return m_plantsList[Random.Range(0, m_plantsList.Count)];
    }
}
