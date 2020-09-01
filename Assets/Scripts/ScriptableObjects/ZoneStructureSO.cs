using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New zone structure", menuName = "CityBuilder/StructureData/ZoneStructure")]
public class ZoneStructureSO : StructureBaseSO
{
    public GameObject upgradePrefab;
    public GameObject[] prefabVariants;
    public GameObject[] upgradePrefabVariants;

    [SerializeField]
    protected int residentsAmount;
    [SerializeField]
    protected int upgradedResidentsAmount;

    public int upgradedHappinessThreshold;
    public int upgradedIncome;
    public int upgradedUpkeep;
    public ZoneType zoneType;


    public int GetResidentsAmount()
    {
        return residentsAmount;
    }

    public int SetUpgradedResidentsAmount()
    {
        if (upgradeActive == true)
        {
            residentsAmount = upgradedResidentsAmount;
        }
        return residentsAmount;
    }
}

public enum ZoneType
{
    Residential,
    Agridcultural,
    Commercial
}
