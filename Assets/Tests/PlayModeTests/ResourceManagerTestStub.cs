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

    public void CalculateTownIncome()
    {

    }

    public bool CanIBuyIt(int amount)
    {
        return true;
    }

    public int HowManyStructureCanIPlace(int placementCost, int count)
    {
        return 0;
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {

    }

    public bool SpendMoney(int amount)
    {
        return true;
    }
}
