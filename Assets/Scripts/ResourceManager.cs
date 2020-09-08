using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour, IResourceManager
{
    [SerializeField]
    private int _startMoneyAmount = 2000;
    [SerializeField]
    private int _removalPrice = 20;
    [SerializeField]
    private float _moneyCalculationInterval;
    [SerializeField]
    private float _woodCalculationInterval;
    [SerializeField]
    private int _startWoodAmount = 10;
    private MoneyHelper _moneyHelper;
    private BuildingManager _buildingManager;
    private PopulationHelper _populationHelper;
    private ShoppingCartHelper _shoppingCartHelper;
    private WoodMaterialHelper _woodMaterialHelper;

    public UIController uIController;
    public int StartMoneyAmount { get => _startMoneyAmount; }
    public float MoneyCalculationInterval { get => _moneyCalculationInterval; }
    public float WoodCalculationInterval { get => _woodCalculationInterval; }
    int IResourceManager.RemovalPrice { get => _removalPrice; }


    // Start is called before the first frame update
    void Start()
    {
        _moneyHelper = new MoneyHelper(_startMoneyAmount);
        _populationHelper = new PopulationHelper();
        _shoppingCartHelper = new ShoppingCartHelper();
        _woodMaterialHelper = new WoodMaterialHelper(_startWoodAmount);
        UpdateUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this._buildingManager = buildingManager;
        InvokeRepeating("CalculateTownIncome", 0, MoneyCalculationInterval);
        InvokeRepeating("CalculateTownTotalWoodAmount", 0, WoodCalculationInterval);
    }

    public void SpendMoney(int amount)
    {
        try
        {
            _moneyHelper.ReduceMoneyAmount(amount);
            UpdateUI();
        }
        catch (MoneyException)
        {
            ReloadGame();
        }
    }

    private void ReloadGame()
    {
        uIController.gameOverPanel.SetActive(true);
    }

    private void InsufficientFundsAlertBox()
    {
        uIController.insufficientFundsPanel.SetActive(true);
    }

    public bool CanIBuyIt(int amount)
    {
        if (_moneyHelper.MoneyAmount >= amount)
        {
            SpendMoney(amount);
            ClearShoppingCartAmount();
            return true;
        }
        else
        {
            InsufficientFundsAlertBox();
            return false;
        }
    }

    public void CalculateTownIncome()
    {
        try
        {
            _moneyHelper.CalculateMoneyAmount(_buildingManager.GetAllStructures());
            UpdateUI();
        }
        catch (MoneyException)
        {
            UpdateUI();
            ReloadGame();
        }
    }

    public void CalculateTownTotalWoodAmount()
    {
        _woodMaterialHelper.CalculateWoodAmount(_buildingManager.GetAllStructures());
        UpdateUI();
    }

    private void OnDisable()
    {
        CancelInvoke();
    }

    public void AddMoneyAmount(int amount)
    {
        _moneyHelper.AddMoneyAmount(amount);
        UpdateUI();
    }

    private void UpdateUI()
    {
        uIController.SetMoneyValue(_moneyHelper.MoneyAmount);
        uIController.SetPopulationValue(_populationHelper.Population);
        uIController.SetShoppingCartValue(_shoppingCartHelper.ShoppingCartAmount);
        uIController.SetWoodValue(_woodMaterialHelper.WoodAmount);
    }

    public void SetUpgradedPopulationAmount(int pastAmount, int newAmount)
    {
        _populationHelper.ReducePopulation(pastAmount);
        _populationHelper.AddPopulation(newAmount);
        UpdateUI();
    }

    public void AddToPopulation(int amount)
    {
        _populationHelper.AddPopulation(amount);
        UpdateUI();
    }

    public void ReducePopulation(int amount)
    {
        _populationHelper.ReducePopulation(amount);
        UpdateUI();
    }

    public void AddToShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.AddShoppingCartAmount(amount);
        UpdateUI();
    }

    public void ReduceShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.ReduceShoppingCartAmount(amount);
        UpdateUI();
    }

    public int ShoppingCartAmount()
    {
        return _shoppingCartHelper.ShoppingCartAmount;
    }

    public void ClearShoppingCartAmount()
    {
        _shoppingCartHelper.ClearShoppingCartAmount();
        UpdateUI();
    }

}
