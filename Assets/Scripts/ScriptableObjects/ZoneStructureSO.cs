using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New zone structure", menuName = "CityBuilder/StructureData/ZoneStructure")]
public class ZoneStructureSO : StructureBaseSO
{
    public GameObject[] prefabVariants;
    public GameObject[] upgradePrefabVariants;
    public int upgradedHappinessThreshold;
    public int upgradedIncome;
    public int upgradedUpkeep;
    public ZoneType zoneType;
    public int maxFacilitySearchRange;
}

public enum ZoneType
{
    Residentaial,
    Agridcultural,
    Commercial
}
