using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SeedToolItem : DroppableTool {

    protected override void InteractWithGardenBed(GardenBed bed) {
        if (!bed.HasPlant()) {
            bed.PlantSeed();
            Destroy(gameObject);
            return;
        }
        ReturnItem();
    }

    protected override void ReturnItem() {
        Debug.Log ("Seed returned");
        // Return unused seed to collection
        GameManager.instance.seedQuantity++;
        Destroy(gameObject);
    }
}
