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
    public FacilityType facilityType = FacilityType.None;

    public void RemoveClient(StructureBaseSO client)
    {
        if(_customers.Contains(client))
        {
            if(facilityType == FacilityType.Water)
            {
                client.RemoveWaterFacility();
            }
            if (facilityType == FacilityType.Water)
            {
                client.RemovePowerFacility();
            }

            _customers.Remove(client);
        }
    }

    public override int GetIncome()
    {
        return _customers.Count * income;
    }

    public int GetNumberOfCustomers()
    {
        return _customers.Count;
    }

    public void AddClient(IEnumerable<StructureBaseSO> structuresAaroundFacility)
    {
        foreach (var nearByStructure in structuresAaroundFacility)
        {
            if(maxCustomers > _customers.Count && nearByStructure != this)
            {
                if(facilityType == FacilityType.Water && nearByStructure.requireWater)
                {
                    if (nearByStructure.AddWaterFacility(this))
                        _customers.Add(nearByStructure);
                }
                if(facilityType == FacilityType.Water && nearByStructure.requireWater)
                {
                    if (nearByStructure.AddWaterFacility(this))
                        _customers.Add(nearByStructure);
                }
            }
        }
    }

    public override IEnumerable<StructureBaseSO> PrepareForRemoval()
    {
        base.PrepareForRemoval();
        foreach (var clientStructure in _customers)
        {
            RemoveClient(clientStructure);
        }
        return _customers;
    }
}

public enum FacilityType
{
    Power,
    Water,
    None
}
