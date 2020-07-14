using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBaseSO : ScriptableObject
{
    [SerializeField]
    protected int income;

    public string buildingName;
    public GameObject prefab;
    public int placementCost;
    public int upkeepCost;
    public bool requireRoadAccess;
    public bool requirePower;
    public bool requireWater;

    public virtual int GetIncome()
    {
        return income;
    }
}
