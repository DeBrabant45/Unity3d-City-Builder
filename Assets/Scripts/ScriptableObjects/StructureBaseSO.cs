using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public abstract class StructureBaseSO : ScriptableObject
{
    [SerializeField]
    protected int income;

    private SingleFacilitySO _powerProvider = null;
    private SingleFacilitySO _waterProvider = null;
    private SingleFacilitySO _siloProvider = null;
    private SingleFacilitySO _healthcareProvider = null;
    private RoadStructureSO _roadProvider = null;

    public string buildingName;
    public GameObject prefab;
    public int placementCost;
    public int upkeepCost;

    public bool requireRoadAccess;
    public bool requirePower;
    public bool requireWater;
    public bool requireSilo;
    public bool requireHealthcare;

    public bool upgradable = false;
    public bool fullyUpgraded = false;
    public GameObject[] upgradeLevelPrefabs;
    public int[] upgradePlacementCost;
    public int[] upgradeIncome;
    [SerializeField]
    private int upgradeLevel;

    public int structureRange = 1;
    public int maxFacilitySearchRange;

    public SingleFacilitySO PowerProvider { get => _powerProvider; }
    public SingleFacilitySO WaterProvider { get => _waterProvider; }
    public SingleFacilitySO SiloProvider { get => _siloProvider; }
    public SingleFacilitySO HealthcareProvider { get => _healthcareProvider; }
    public RoadStructureSO RoadProvider { get => _roadProvider; }
    public int UpgradeLevel { get => upgradeLevel; }

    public virtual int GetIncome()
    {
        return income;
    }

    public virtual int GetUpgradedIncome()
    {
        if(upgradeLevel < upgradeIncome.Length)
        {
            return income = upgradeIncome[upgradeLevel];
        }
        return income;
    }

    public void IncreaseUpgradeLevel()
    {
        if(upgradeLevel < upgradeLevelPrefabs.Length)
        {
            upgradeLevel++;
            IsFullyUpgraded();
        }
    }

    public int GetUpgradePlacementCost()
    {
        if(upgradeLevel < upgradePlacementCost.Length)
        {
            return upgradePlacementCost[upgradeLevel];
        }
        return placementCost;
    }

    public GameObject GetUpgradedPrefab()
    {
        if(upgradeLevel < upgradeLevelPrefabs.Length)
        {
            return upgradeLevelPrefabs[upgradeLevel];
        }
        return null;
    }

    public bool IsFullyUpgraded()
    {
        if(upgradeLevel == upgradeLevelPrefabs.Length)
        {
            return fullyUpgraded = true;
        }
        return fullyUpgraded = false;
    }

    public bool HasPower()
    {
        return _powerProvider != null;
    }

    public bool HasWater()
    {
        return _waterProvider != null;
    }

    public bool HasRoadAccess()
    {
        return _roadProvider != null;
    }

    public bool HasSilo()
    {
        return _siloProvider != null;
    }

    public bool HasHealthcare()
    {
        return _healthcareProvider != null;
    }

    public bool HasFullyUpgraded()
    {
        return fullyUpgraded;
    }

    public void RemovePowerFacility()
    {
        _powerProvider = null;
    }
    public void RemoveWaterFacility()
    {
        _waterProvider = null;
    }

    public void RemoveSiloFacility()
    {
        _siloProvider = null;
    }

    internal void RemoveHealthcareFacility()
    {
        _healthcareProvider = null;
    }

    public void RemoveRoadProivder()
    {
        _roadProvider = null;
    }

    public void PrepareStructure(IEnumerable<StructureBaseSO> structuresInRange)
    {
        AddRoadAccessToClientStructure(structuresInRange);
    }

    public bool AddPowerFacility(SingleFacilitySO facility)
    {
        if(_powerProvider == null)
        {
            _powerProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddWaterFacility(SingleFacilitySO facility)
    {
        if(_waterProvider == null)
        {
            _waterProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddSiloFacility(SingleFacilitySO facility)
    {
        if(_siloProvider == null)
        {
            _siloProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddHealthcareFacility(SingleFacilitySO facility)
    {
        if(_healthcareProvider == null)
        {
            _healthcareProvider = facility;
            return true;
        }
        return false;
    }

    public virtual IEnumerable<StructureBaseSO> PrepareForRemoval()
    {
        if(_powerProvider != null)
        {
            _powerProvider.RemoveClient(this);
        }
        if (_waterProvider != null)
        {
            _waterProvider.RemoveClient(this);
        }
        if (_siloProvider != null)
        {
            _siloProvider.RemoveClient(this);
        }
        if(_healthcareProvider != null)
        {
            _healthcareProvider.RemoveClient(this);
        }

        return null;
    }

    private void AddRoadAccessToClientStructure(IEnumerable<StructureBaseSO> structuresInRange)
    {
        if(_roadProvider != null)
        {
            return;
        }
        foreach (var nearByStructure in structuresInRange)
        {
            if (nearByStructure.GetType() == typeof(RoadStructureSO))
            {
                _roadProvider = (RoadStructureSO)nearByStructure;
                return;
            }
        }
    }
}
