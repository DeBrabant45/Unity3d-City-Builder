using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New zone structure", menuName = "CityBuilder/StructureData/ZoneStructure")]
public class ZoneStructureSO : StructureBaseSO
{
    public bool upgradable;
    public GameObject[] prefabVariants;
    public UpgradeType[] availableUpgrades;
    public ZoneType zoneType;
}

[System.Serializable]
public struct UpgradeType
{
    public GameObject[] prefabVariants;
    public int HappinessThreshold;
    public int newIncome;
    public int newUpkeep;

}

public enum ZoneType
{
    Residentaial,
    Agridcultural,
    Commercial
}
