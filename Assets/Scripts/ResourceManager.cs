using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField]
    private int _startMoneyAmount = 5000;
    [SerializeField]
    private int _removalPrice = 20;
    [SerializeField]
    private float _moneyCalculationInterval = 2;
    private MoneyHelper _moneyHelper;
    private BuildingManager _buildingManager;

    public UIController uIController;
    public int StartMoneyAmount { get => _startMoneyAmount; }
    public float MoneyCalculationInterval { get => _moneyCalculationInterval; }
    int IResourceManager.RemovalPrice { get => _removalPrice; }


    // Start is called before the first frame update
    void Start()
    {
        _moneyHelper = new MoneyHelper(_startMoneyAmount);
        UpdateMoneyValueUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this._buildingManager = buildingManager;
        InvokeRepeating("CalculateTownIncome", 0, MoneyCalculationInterval);
    }

    public bool SpendMoney(int amount)
    {
        if (CanIBuyIt(amount))
        {
            try
            {
                _moneyHelper.ReduceMoneyAmount(amount);
                UpdateMoneyValueUI();
                return true;
            }
            catch (MoneyException)
            {
                ReloadGame();
            }
        }

        return false;
    }

    private void ReloadGame()
    {
        Debug.Log("Game Over");
    }

    public bool CanIBuyIt(int amount)
    {
        if (_moneyHelper.MoneyAmount >= amount)
        {
            return true;
        }

        return false;
    }

    public void CalculateTownIncome()
    {
        try
        {
            _moneyHelper.CalculateMoneyAmount(_buildingManager.GetAllStructures());
            UpdateMoneyValueUI();
        }
        catch (MoneyException)
        {

            ReloadGame();
        }
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void AddMoneyAmount(int amount)
    {
        _moneyHelper.AddMoneyAmount(amount);
        UpdateMoneyValueUI();
    }

    private void UpdateMoneyValueUI()
    {
        uIController.SetMoneyValue(_moneyHelper.MoneyAmount);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public int HowManyStructureCanIPlace(int placementCost, int numberOfStructures)
    {
        int amount = (int)(_moneyHelper.MoneyAmount / placementCost);
        return amount > numberOfStructures ? numberOfStructures : amount;
    }
}
