using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTestStub : MonoBehaviour, IResourceManager
{
    public float MoneyCalculationInterval { get; }

    public int StartMoneyAmount { get; }

    public int RemovalPrice { get; }

    public void AddMoneyAmount(int amount)
    {

    }

    public void AddToShoppingCartAmount(int amount)
    {

    }

    public void AddToPopulation(int amount)
    {

    }

    public void CalculateTownIncome()
    {

    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public int ShoppingCartAmount()
    {
        return 0;
    }

    public void ClearShoppingCartAmount()
    {

    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {

    }

    public void ReduceShoppingCartAmount(int amount)
    {

    }

    public void ReducePopulation(int amount)
    {

    }

    public void SpendMoney(int amount)
    {

    }
}
