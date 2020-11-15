using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneyHelper
{
    private int _moneyAmount;

    public MoneyHelper(int moneyAmount)
    {
        this._moneyAmount = moneyAmount;
    }

    public int MoneyAmount
    {
        get => _moneyAmount;
        set
        {
            if(value <= 0)
            {
                _moneyAmount = 0;
                throw new MoneyException("Not enough money");
            }
            else
            {
                _moneyAmount = value;
            }
        }
    }

    public void ReduceMoneyAmount(int amount)
    {
        MoneyAmount -= amount;
    }

    public void AddMoneyAmount(int amount)
    {
        MoneyAmount += amount;
    }

    public void CalculateMoneyAmount(IEnumerable<StructureBaseSO> buildings)
    {
        CollectIncome(buildings);
        ReduceUpkeep(buildings);
    }

    private void ReduceUpkeep(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            MoneyAmount -= structure.upkeepCost;
        }
    }

    private void CollectIncome(IEnumerable<StructureBaseSO> buildings)
    {
        foreach (var structure in buildings)
        {
            MoneyAmount += structure.GetIncome();
        }
    }
}
