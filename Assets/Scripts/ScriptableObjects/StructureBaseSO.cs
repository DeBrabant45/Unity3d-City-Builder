﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureBaseSO : ScriptableObject
{
    [SerializeField]
    protected int income;

    private SingleFacilitySO _powerProvider = null;
    private SingleFacilitySO _waterProvider = null;
    private RoadStructureSO _roadProvider = null;

    public string buildingName;
    public GameObject prefab;
    public int placementCost;
    public int upkeepCost;
    public bool requireRoadAccess;
    public bool requirePower;
    public bool requireWater;
    public int structureRange = 1;

    public SingleFacilitySO PowerProvider { get => _powerProvider; }
    public SingleFacilitySO WaterProvider { get => _waterProvider; }
    public RoadStructureSO RoadProvider { get => _roadProvider; }

    public virtual int GetIncome()
    {
        return income;
    }

    public void RemoveWaterFacility()
    {
        _waterProvider = null;
    }

    public bool HasPower()
    {
        return _powerProvider != null;
    }

    public void RemoveRoadProivder()
    {
        _roadProvider = null;
    }

    public bool HasWater()
    {
        return _waterProvider != null;
    }

    public void RemovePowerFacility()
    {
        _powerProvider = null;
    }

    public bool HasRoadAccess()
    {
        return _roadProvider != null;
    }

    public void PrepareStructure(IEnumerable<StructureBaseSO> structuresInRange)
    {
        AddRoadProvider(structuresInRange);
    }

    public void AddPowerFacility(SingleFacilitySO facility)
    {
        if(_powerProvider == null)
        {
            _powerProvider = facility;
        }
    }

    public void AddWaterFacility(SingleFacilitySO facility)
    {
        if(_waterProvider == null)
        {
            _waterProvider = facility;
        }
    }

    public void PrepareForRemoval()
    {
        if(_powerProvider != null)
        {
            _powerProvider.RemoveProvider(this);
        }
        if (_waterProvider != null)
        {
            _waterProvider.RemoveProvider(this);
        }
    }

    private void AddRoadProvider(IEnumerable<StructureBaseSO> structuresInRange)
    {
        if(_roadProvider != null)
        {
            return;
        }
        foreach (var nearByStructure in structuresInRange)
        {
            if(nearByStructure.GetType() == typeof(RoadStructureSO))
            {
                _roadProvider = (RoadStructureSO)nearByStructure;
                return;
            }
        }
    }
}
