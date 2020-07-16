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

    public void RemoveProvider(StructureBaseSO providerStructure)
    {
        if(_customers.Contains(providerStructure))
        {
            if(facilityType == FacilityType.Water)
            {
                providerStructure.RemoveWaterFacility();
            }
            if (facilityType == FacilityType.Water)
            {
                providerStructure.RemovePowerFacility();
            }

            _customers.Remove(providerStructure);
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
}

public enum FacilityType
{
    Power,
    Water,
    None
}
