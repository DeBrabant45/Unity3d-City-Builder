using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.UI;

public abstract class StructureBaseSO : ScriptableObject
{
    [SerializeField]
    protected int income;

    private SingleFacilitySO _powerProvider = null;
    private SingleFacilitySO _waterProvider = null;
    private SingleFacilitySO _siloProvider = null;
    private SingleFacilitySO _healthcareProvider = null;
    private SingleFacilitySO _lawEnforcementProvider = null;
    private SingleFacilitySO _fireProtectionProvider = null;
    private SingleFacilitySO _postalProvider = null;
    private RoadStructureSO _roadProvider = null;

    public string buildingName;
    public Sprite buildingImage;
    public GameObject prefab;
    public int placementCost;
    public int requiredWoodAmount;
    public int requiredSteelAmount;
    public int upkeepCost;

    public bool requireRoadAccess;
    public bool requirePower;
    public bool requireWater;
    public bool requireSilo;
    public bool requireHealthcare;
    public bool requireLawEnforcement;
    public bool requireFireProtection;
    public bool requirePostalService;

    public bool upgradable = false;
    public bool fullyUpgraded = false;
    public GameObject[] upgradeLevelPrefabs;
    public int[] upgradePlacementCost;
    public int[] upgradeIncome;
    public int[] upgradeRequiredWoodAmount;
    public int[] upgradeRequiredSteelAmount;
    [SerializeField]
    private int upgradeLevel;

    public int structureRange = 1;
    public int maxFacilitySearchRange;

    public SingleFacilitySO PowerProvider { get => _powerProvider; }
    public SingleFacilitySO WaterProvider { get => _waterProvider; }
    public SingleFacilitySO SiloProvider { get => _siloProvider; }
    public SingleFacilitySO HealthcareProvider { get => _healthcareProvider; }
    public SingleFacilitySO LawEnforcementProvider { get => _lawEnforcementProvider; }
    public SingleFacilitySO FireProtectionProvider { get => _fireProtectionProvider; }
    public SingleFacilitySO PostalProvider { get => _postalProvider; }
    public RoadStructureSO RoadProvider { get => _roadProvider; }
    public int UpgradeLevel { get => upgradeLevel; }

    public virtual int GetIncome()
    {
        return income;
    }

    public int GetRequiredWoodAmountForBuild()
    {
        return requiredWoodAmount;
    }

    public int GetRequiredSteelAmountForBuild()
    {
        return requiredSteelAmount;
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

    public int GetUpgradeRequiredWoodAmount()
    {
        if(upgradeLevel < upgradeRequiredWoodAmount.Length)
        {
            return upgradeRequiredWoodAmount[upgradeLevel];
        }
        return requiredWoodAmount;
    }

    public int GetUpgradeRequiredSteelAmount()
    {
        if (upgradeLevel < upgradeRequiredSteelAmount.Length)
        {
            return upgradeRequiredSteelAmount[upgradeLevel];
        }
        return requiredSteelAmount;
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

    public bool HasLawEnforcement()
    {
        return _lawEnforcementProvider != null;
    }

    public bool HasFireProtection()
    {
        return _fireProtectionProvider != null;
    }

    public bool HasPostalService()
    {
        return _postalProvider != null;
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

    public void RemoveHealthcareFacility()
    {
        _healthcareProvider = null;
    }

    public void RemoveLawEnforcementFacility()
    {
        _lawEnforcementProvider = null;
    }

    public void RemoveFireProtectionFacility()
    {
        _fireProtectionProvider = null;
    }

    public void RemoveRoadProivder()
    {
        _roadProvider = null;
    }

    public void RemovePostalFacility()
    {
        _postalProvider = null;
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

    public bool AddLawEnforcementFacility(SingleFacilitySO facility)
    {
        if(_lawEnforcementProvider == null)
        {
            _lawEnforcementProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddFireProtectionFacility(SingleFacilitySO facility)
    {
        if(_fireProtectionProvider == null)
        {
            _fireProtectionProvider = facility;
            return true;
        }
        return false;
    }

    public bool AddPostalFacility(SingleFacilitySO facility)
    {
        if(_postalProvider == null)
        {
            _postalProvider = facility;
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
        if (_lawEnforcementProvider != null)
        {
            _lawEnforcementProvider.RemoveClient(this);
        }
        if(_fireProtectionProvider != null)
        {
            _fireProtectionProvider.RemoveClient(this);
        }
        if (_postalProvider != null)
        {
            _postalProvider.RemoveClient(this);
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
