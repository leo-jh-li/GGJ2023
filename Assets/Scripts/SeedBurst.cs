using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedBurst : MonoBehaviour {

    [SerializeField] private HarvestedSeed m_harvestedSeedPrefab;

    // Creates seeds and makes them burst out from this position
    public void CreateSeedBurst(int quantityToDrop) {
        for (int i = 0 ; i < quantityToDrop; i++) {
            // TODO: temp: spawn randomly around plant
            Vector3 rand = new Vector3(Random.Range(-0.5f,0.5f), Random.Range(-0.5f,0.5f), 0);
            Quaternion randomRotation = Quaternion.Euler(0, 0, Random.Range(0, 360));
            HarvestedSeed seed = Instantiate(m_harvestedSeedPrefab, transform.position + rand, randomRotation);
            // TODO: rotate seeds randomly?
            // TODO: make seeds look rare if necessary?
            // TODO: add force in randomdirection. make simulate with a generated arc instead of using an rb?
            // TODO: make magnitudes approximately evenly spaced out?

            // itemDrop.Initialize(itemsToDrop[i]);
            // itemDrop.ApplyInitialForce(i, itemsToDrop.Count);
        }
    }

    /*
    // Applies appropriate force to this item given its index within a burst of items
    private void ApplyInitialForce(int itemIndex, int burstQuantity, bool droppedByPlayer = false, bool dropperFacingLeft = false) {
        Vector2 force = Vector2.zero;
        force.y = m_spawnForce;
        if (droppedByPlayer) {
            // If dropped by player, launch forward
            force.x = (dropperFacingLeft ? -1 : 1) * m_spawnForce * 0.65f;
        } else {
            // Add horizontal force
            force.x = Mathf.Lerp(-m_itemBurstRangeX, m_itemBurstRangeX, (float) (itemIndex + 1) / (burstQuantity + 1));
        }
        m_rb.AddForce(force, ForceMode2D.Impulse);
    }
    */

}
