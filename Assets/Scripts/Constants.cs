using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : Singleton<Constants> {
    [Tooltip("Game Values")]
    // TODO?

    [Header("Sound Names")]
    public string[] PLANT_SEED;
    public string[] WATER_PLANT;
    public string[] HARVEST_PLANT;
    public string[] FERTILIZE_PLANT;

    public string COLLECT_SEEDS;
    public string LOONIE;
}
