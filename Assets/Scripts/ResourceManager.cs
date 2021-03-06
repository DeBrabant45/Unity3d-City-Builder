﻿using System;
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
    [SerializeField]
    private float _steelCalculationInterval;
    [SerializeField]
    private int _startSteelAmount = 10;

    private MoneyHelper _moneyHelper;
    private BuildingManager _buildingManager;
    private PopulationHelper _populationHelper;
    private ShoppingCartHelper _shoppingCartHelper;
    private WoodMaterialHelper _woodMaterialHelper;
    private SteelMaterialHelper _steelMaterialHelper;

    public UIController uIController;
    public int StartMoneyAmount { get => _startMoneyAmount; }
    public float MoneyCalculationInterval { get => _moneyCalculationInterval; }
    public float WoodCalculationInterval { get => _woodCalculationInterval; }
    public float SteelCalculationInterval { get => _steelCalculationInterval; }
    int IResourceManager.RemovalPrice { get => _removalPrice; }


    // Start is called before the first frame update
    void Start()
    {
        _moneyHelper = new MoneyHelper(_startMoneyAmount);
        _populationHelper = new PopulationHelper();
        _shoppingCartHelper = new ShoppingCartHelper();
        _woodMaterialHelper = new WoodMaterialHelper(_startWoodAmount);
        _steelMaterialHelper = new SteelMaterialHelper(_startSteelAmount);
        UpdateUI();
    }

    public void PrepareResourceManager(BuildingManager buildingManager)
    {
        this._buildingManager = buildingManager;
        InvokeRepeating("CalculateTownIncome", 0, MoneyCalculationInterval);
        InvokeRepeating("CalculateTownTotalWoodAmount", 0, WoodCalculationInterval);
        InvokeRepeating("CalculateTownTotalSteelAmount", 0, SteelCalculationInterval);
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
            ActivateCurrencyAmountGameOverInfo();
        }
    }

    public void SpendWood(int amount)
    {
        _woodMaterialHelper.ReduceWoodAmount(amount);
        UpdateUI();
    }

    public void SpendSteel(int amount)
    {
        _steelMaterialHelper.ReduceSteelAmount(amount);
        UpdateUI();
    }

    private void ReloadGame()
    {
        uIController.ReloadGame();
    }

    private void ActivateWoodMaterialGameOverInfo()
    {
        uIController.ActivateWoodMaterialGameOverInfo();
    }

    private void ActivateSteelMaterialGameOverInfo()
    {
        uIController.ActivateSteelMaterialGameOverInfo();
    }

    private void ActivateCurrencyAmountGameOverInfo()
    {
        uIController.ActivateCurrencyAmountGameOverInfo();
    }

    public bool CanIBuyIt(int moneyAmount, int steelAmount, int woodAmount)
    {
        if (_moneyHelper.MoneyAmount >= moneyAmount && _steelMaterialHelper.SteelAmount >= steelAmount && _woodMaterialHelper.WoodAmount >= woodAmount)
        {
            SpendMoney(moneyAmount);
            SpendSteel(steelAmount);
            SpendWood(woodAmount);
            ClearShoppingCartAmount();
            return true;
        }
        else
        {
            InsufficientFundsAlertBox();
            InsufficientMoneyAlertDisplay(moneyAmount);
            InsufficientSteelAlertDisplay(steelAmount);
            InsufficientWoodAlertDisplay(woodAmount);
            return false;
        }
    }

    private void InsufficientFundsAlertBox()
    {
        uIController.OnOpenInsufficientFundsAlertBox();
    }

    private void InsufficientSteelAlertDisplay(int steelRequested)
    {
        if (_steelMaterialHelper.SteelAmount < steelRequested)
        {
            uIController.ActivateInsufficientFundsSteelPanel();
        }
        else
        {
            uIController.DeactivateInsufficientFundsSteelPanel();
        }
    }

    private void InsufficientMoneyAlertDisplay(int moneyRequested)
    {
        if(_moneyHelper.MoneyAmount < moneyRequested)
        {
            uIController.ActivateInsufficientFundsMoneyPanel();
        }
        else
        {
            uIController.DeactivateInsufficientFundsMoneyPanel();
        }
    }

    private void InsufficientWoodAlertDisplay(int woodRequested)
    {
        if(_woodMaterialHelper.WoodAmount < woodRequested)
        {
            uIController.ActivateInsufficientFundsWoodPanel();
        }
        else
        {
            uIController.DeactivateInsufficientFundsWoodPanel();
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
            ActivateCurrencyAmountGameOverInfo();
        }
    }

    public void CalculateTownTotalWoodAmount()
    {
        if(IsThereAnyWoodManufacturesOnGrid())
        {
            _woodMaterialHelper.CalculateWoodAmount(_buildingManager.GetAllStructures());
            UpdateUI();
        }
        NoWoodAvailableGameOver();
    }

    private bool IsThereAnyWoodManufacturesOnGrid()
    {
        foreach (var structure in _buildingManager.GetAllStructures())
        {
            if (structure.GetType() == typeof(ManufacturerBaseSO) && ((ManufacturerBaseSO)structure).ManufactureType == ManufactureType.Wood)
            {
                return true;
            }
        }
        return false;
    }

    private void NoWoodAvailableGameOver()
    {
        if(_woodMaterialHelper.WoodAmount == 0 && IsThereAnyWoodManufacturesOnGrid() == false)
        {
            ReloadGame();
            ActivateWoodMaterialGameOverInfo();
        }
    }

    public void CalculateTownTotalSteelAmount()
    {
        _steelMaterialHelper.CalculateSteelAmount(_buildingManager.GetAllStructures());
        UpdateUI();
        NoSteelAvailableGameOver();
    }

    private bool IsThereAnySteelManufacturesOnGrid()
    {
        foreach (var structure in _buildingManager.GetAllStructures())
        {
            if (structure.GetType() == typeof(ManufacturerBaseSO) && ((ManufacturerBaseSO)structure).ManufactureType == ManufactureType.Steel)
            {
                return true;
            }
        }
        return false;
    }

    private void NoSteelAvailableGameOver()
    {
        if (_steelMaterialHelper.SteelAmount == 0 && IsThereAnySteelManufacturesOnGrid() == false)
        {
            ReloadGame();
            ActivateSteelMaterialGameOverInfo();
        }
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
        uIController.SetWoodValue(_woodMaterialHelper.WoodAmount);
        uIController.SetSteelValue(_steelMaterialHelper.SteelAmount);
        uIController.SetPopulationValue(_populationHelper.Population);
        uIController.SetShoppingCartMoneyValue(_shoppingCartHelper.ShoppingCartMoneyAmount);
        uIController.SetShoppingCartWoodValue(_shoppingCartHelper.ShoppingCartWoodAmount);
        uIController.SetShoppingCartSteelValue(_shoppingCartHelper.ShoppingCartSteelAmount);
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

    public void AddMoneyToShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.AddMoneyToShoppingCartAmount(amount);
        UpdateUI();
    }

    public void ReduceMoneyFromShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.ReduceMoneyFromShoppingCartAmount(amount);
        UpdateUI();
    }

    public void AddWoodToShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.AddWoodToShoppingCartAmount(amount);
        UpdateUI();
    }

    public void ReduceWoodFromShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.ReduceWoodFromShoppingCartAmount(amount);
        UpdateUI();
    }

    public void AddSteelToShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.AddSteelToShoppingCartAmount(amount);
        UpdateUI();
    }

    public void ReduceSteelFromShoppingCartAmount(int amount)
    {
        _shoppingCartHelper.ReduceSteelFromShoppingCartAmount(amount);
        UpdateUI();
    }

    public int ShoppingCartMoneyAmount()
    {
        return _shoppingCartHelper.ShoppingCartMoneyAmount;
    }

    public int ShoppingCartWoodAmount()
    {
        return _shoppingCartHelper.ShoppingCartWoodAmount;
    }

    public int ShoppingCartSteelAmount()
    {
        return _shoppingCartHelper.ShoppingCartSteelAmount;
    }

    public void ClearShoppingCartAmount()
    {
        _shoppingCartHelper.ClearShoppingCartAmount();
        UpdateUI();
    }

}
