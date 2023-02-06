using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RandomRangeInt {
    public int min;
    [Tooltip("Min and max inclusive.")]
    public int max;

    public RandomRangeInt(int min, int max) {
        this.min = min;
        this.max = max;
    }

    public int GetRandom() {
        return Random.Range(min, max + 1);
    }

    public override string ToString() {
        return "RandomRange(" + min + ", " + max + ")";
    }
}