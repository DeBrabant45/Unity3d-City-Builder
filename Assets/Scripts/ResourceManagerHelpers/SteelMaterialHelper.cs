using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteelMaterialHelper
{
    private int _steelAmount;

    public SteelMaterialHelper(int steelAmount)
    {
        this._steelAmount = steelAmount;
    }

    public int SteelAmount
    {
        get => _steelAmount;
        set
        {
            if (value < 0)
            {
                _steelAmount = 0;
            }
            else
            {
                _steelAmount = value;
            }
        }
    }

    public void ReduceSteelAmount(int amount)
    {
        SteelAmount -= amount;
    }

    public void AddWoodAmount(int amount)
    {
        SteelAmount += amount;
    }

    public void CalculateSteelAmount(IEnumerable<StructureBaseSO> buildings)
    {
        CollectSteelAmount(buildings);
    }

    private void CollectSteelAmount(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            if (structure.GetType() == typeof(ManufacturerBaseSO) && ((ManufacturerBaseSO)structure).ManufactureType == ManufactureType.Steel)
            {
                SteelAmount += ((ManufacturerBaseSO)structure).GetMaterialAmount();
            }
        }
    }
}
