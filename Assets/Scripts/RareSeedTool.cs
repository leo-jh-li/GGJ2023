using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RareSeedTool : SeedTool {

    protected override void OnEnable() {
        GameManager.instance.OnRareSeedQuantityChange += UpdateSeedQuantity;
    }

    protected override void OnDisable() {
        GameManager.instance.OnRareSeedQuantityChange -= UpdateSeedQuantity;
    }

    protected override bool CanRetrieveSeed() {
        return GameManager.instance.rareSeedQuantity > 0;
    }

    public override void RetrieveSeed() {
        if (!CanRetrieveSeed()) { return; }
        GameManager.instance.rareSeedQuantity--;
        Instantiate(m_seedEntityPrefab, Utils.GetMouseWorldPos(), Quaternion.identity);
    }
}
