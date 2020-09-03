using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New zone structure", menuName = "CityBuilder/StructureData/ZoneStructure")]
public class ZoneStructureSO : StructureBaseSO
{
    public GameObject[] prefabVariants;
    //public GameObject upgradePrefab;
    //public GameObject[] upgradePrefabVariants;
    public int[] upgradedResidentsAmount;

    [SerializeField]
    protected int residentsAmount;

    //public int upgradedHappinessThreshold;
    //public int upgradedIncome;
    //public int upgradedUpkeep;
    public ZoneType zoneType;


    public int GetResidentsAmount()
    {
        return residentsAmount;
    }

    public int SetUpgradedResidentsAmount()
    {
        if(upgradedResidentsAmount.Length > 0)
        {
            return residentsAmount = upgradedResidentsAmount[upgradeLevel];
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
