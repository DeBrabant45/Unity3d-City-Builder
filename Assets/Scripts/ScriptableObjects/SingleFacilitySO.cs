using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New facility", menuName = "CityBuilder/StructureData/Facility")]
public class SingleFacilitySO : SingleStructureBaseSO
{
    private HashSet<StructureBaseSO> _customers = new HashSet<StructureBaseSO>();

    public int maxCustomers;
    public int upkeepPerCustomer;
    public GameObject upgradePrefab;
    public int maxCustomersUpgraded;
    public FacilityType facilityType = FacilityType.None;

    public void RemoveClient(StructureBaseSO client)
    {
        if(_customers.Contains(client))
        {
            if(facilityType == FacilityType.Water)
            {
                client.RemoveWaterFacility();
            }
            if (facilityType == FacilityType.Power)
            {
                client.RemovePowerFacility();
            }
            if(facilityType == FacilityType.Silo)
            {
                client.RemoveSiloFacility();
            }

            _customers.Remove(client);
        }
    }

    public int SetUpgradedMaxCustomers()
    {
        return maxCustomers = maxCustomersUpgraded;
    }

    public override int GetIncome()
    {
        return _customers.Count * income;
    }

    public int GetNumberOfCustomers()
    {
        return _customers.Count;
    }

    public void AddClient(IEnumerable<StructureBaseSO> structuresAroundFacility)
    {
        foreach (var nearByStructure in structuresAroundFacility)
        {
            if(maxCustomers > _customers.Count && nearByStructure != this)
            {
                if(facilityType == FacilityType.Power && nearByStructure.requirePower)
                {
                    if (nearByStructure.AddPowerFacility(this))
                        _customers.Add(nearByStructure);
                }
                if(facilityType == FacilityType.Water && nearByStructure.requireWater)
                {
                    if (nearByStructure.AddWaterFacility(this))
                        _customers.Add(nearByStructure);
                }
                if (facilityType == FacilityType.Silo && nearByStructure.requireSilo)
                {
                    if (nearByStructure.AddSiloFacility(this))
                        _customers.Add(nearByStructure);
                }
            }
        }
    }

    public bool IsFull()
    {
        return GetNumberOfCustomers() >= maxCustomers;
    }

    public override IEnumerable<StructureBaseSO> PrepareForRemoval()
    {
        base.PrepareForRemoval();
        List<StructureBaseSO> tempList = new List<StructureBaseSO>(_customers);
        foreach (var clientStructure in tempList)
        {
            RemoveClient(clientStructure);
        }
        return tempList;
    }
}

public enum FacilityType
{
    Power,
    Water,
    Silo,
    None
}
