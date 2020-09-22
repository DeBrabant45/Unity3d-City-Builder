using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManagerTestStub : MonoBehaviour, IResourceManager
{
    public float MoneyCalculationInterval { get; }

    public int StartMoneyAmount { get; }

    public int RemovalPrice { get; }

    public float WoodCalculationInterval { get; }

    public void AddMoneyAmount(int amount)
    {

    }

    public void AddMoneyToShoppingCartAmount(int amount)
    {

    }

    public void AddToPopulation(int amount)
    {

    }

    public void CalculateTownIncome()
    {

    }

    public int ShoppingCartMoneyAmount()
    {
        return 0;
    }

    public void ClearShoppingCartAmount()
    {

    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {

    }

    public void ReduceMoneyFromShoppingCartAmount(int amount)
    {

    }

    public void ReducePopulation(int amount)
    {

    }

    public void SpendMoney(int amount)
    {

    }

    public void SetUpgradedPopulationAmount(int pastAmount, int newAmount)
    {

    }

    public void CalculateTownTotalWoodAmount()
    {

    }

    public void AddWoodToShoppingCartAmount(int amount)
    {

    }

    public void ReduceWoodFromShoppingCartAmount(int amount)
    {

    }

    public void AddSteelToShoppingCartAmount(int amount)
    {

    }

    public void ReduceSteelFromShoppingCartAmount(int amount)
    {

    }

    public bool CanIBuyIt(int moneyAmount, int steelAmount, int woodAmount)
    {
        throw new System.NotImplementedException();
    }

    public int ShoppingCartWoodAmount()
    {
        throw new System.NotImplementedException();
    }

    public int ShoppingCartSteelAmount()
    {
        throw new System.NotImplementedException();
    }
}
