using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New zone structure", menuName = "CityBuilder/StructureData/ZoneStructure")]
public class ZoneStructureSO : StructureBaseSO
{
    [SerializeField]
    private int residentsAmount;

    public GameObject[] prefabVariants;
    public int[] upgradedResidentsAmount;
    public ZoneType zoneType;

    public int ResidentsAmount { get => residentsAmount; }

    public int GetResidentsAmount()
    {
        return residentsAmount;
    }

    public int SetUpgradedResidentsAmount()
    {
        if(UpgradeLevel < upgradedResidentsAmount.Length)
        {
            return residentsAmount = upgradedResidentsAmount[UpgradeLevel];
        }
        return residentsAmount;
    }
}

public enum ZoneType
{
    Residential,
    Agridcultural,
    Commercial,
    Entertainment,
    Park
}
