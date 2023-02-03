using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fertilizer : DroppableTool {

    protected override void InteractWithGardenBed(GardenBed bed) {
        if (bed.CanFertilizePlant()) {
            bed.TryFertilizePlant();
            Destroy(gameObject);
            return;
        }
        ReturnItem();
    }

    protected override void ReturnItem() {
        // Return unused fertilizer to collection
        GameManager.instance.fertilizerQuantity++;
        Destroy(gameObject);
    }
}