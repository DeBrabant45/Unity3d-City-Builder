using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NullStructureSO : StructureBaseSO
{
    private void OnEnable()
    {
        buildingName = "Nullalbe Object";
        prefab = null;
        placementCost = 0;
        upkeepCost = 0;
        requireRoadAccess = false;
        requirePower = false;
        requireWater = false;
        upgradable = false;
        income = 0;
    }
}
