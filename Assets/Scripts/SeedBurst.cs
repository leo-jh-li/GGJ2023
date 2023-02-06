using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBurst : MonoBehaviour {

    [SerializeField] private HarvestedSeed m_harvestedSeedPrefab;

    // Creates seeds and makes them burst out from this position
    public void CreateSeedBurst(int normalSeeds, int rareSeeds) {
        Debug.Log($"CreateSeedBurst: {normalSeeds}, {rareSeeds}");
        int quantityToDrop = normalSeeds + rareSeeds;
        for (int i = 0 ; i < quantityToDrop; i++) {
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            HarvestedSeed seed = Instantiate(m_harvestedSeedPrefab, transform.position, randomRotation);

            // Make seed rare as appropriate
            if (i >= normalSeeds) {
                seed.SetToRare();
            }
        }
    }
}
