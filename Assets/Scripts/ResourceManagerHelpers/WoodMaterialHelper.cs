using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WoodMaterialHelper 
{
    private int _woodAmount;

    public WoodMaterialHelper(int moneyAmount)
    {
        this._woodAmount = moneyAmount;
    }

    public int WoodAmount
    {
        get => _woodAmount;
        set
        {
            if (value < 0)
            {
                _woodAmount = 0;
            }
            else
            {
                _woodAmount = value;
            }
        }
    }

    public void ReduceWoodAmount(int amount)
    {
        WoodAmount -= amount;
    }

    public void AddWoodAmount(int amount)
    {
        WoodAmount += amount;
    }

    public void CalculateWoodAmount(IEnumerable<StructureBaseSO> buildings)
    {
        CollectWoodAmount(buildings);
    }

    private void CollectWoodAmount(IEnumerable<StructureBaseSO> buildings)
    {
        if(buildings.GetType() == typeof(ManufacturerBaseSO))
        {
            foreach (var structure in buildings)
            {
                WoodAmount += ((ManufacturerBaseSO)structure).GetIncome();
            }
        }
    }
}
