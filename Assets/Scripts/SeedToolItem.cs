using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToolItem : DroppableTool {
    [SerializeField, Tooltip("True iff this seed is rare.")]
    private bool m_isRare;

    protected override void InteractWithGardenBed(GardenBed bed) {
        if (!bed.HasPlant()) {
            bed.PlantSeed(m_isRare);
            Destroy(gameObject);
            return;
        }
        ReturnItem();
    }

    protected override void ReturnItem() {
        // Return unused seed to collection
        if (m_isRare) {
            GameManager.instance.rareSeedQuantity++;
        } else {
            GameManager.instance.seedQuantity++;
        }
        Destroy(gameObject);
    }
}
