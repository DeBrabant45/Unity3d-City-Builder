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
    private PopulationHelper _populationHelper;

    public UIController uIController;
    public int StartMoneyAmount { get => _startMoneyAmount; }
    public float MoneyCalculationInterval { get => _moneyCalculationInterval; }
    int IResourceManager.RemovalPrice { get => _removalPrice; }


    // Start is called before the first frame update
    void Start()
    {
        _moneyHelper = new MoneyHelper(_startMoneyAmount);
        _populationHelper = new PopulationHelper();
        UpdateUI();
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
                UpdateUI();
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
        uIController.gameOverPanel.SetActive(true);
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
            UpdateUI();
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
        UpdateUI();
    }

    private void UpdateUI()
    {
        uIController.SetMoneyValue(_moneyHelper.MoneyAmount);
        uIController.SetPopulationValue(_populationHelper.Population);
    }

    public int HowManyStructureCanIPlace(int placementCost, int numberOfStructures)
    {
        int amount = (int)(_moneyHelper.MoneyAmount / placementCost);
        return amount > numberOfStructures ? numberOfStructures : amount;
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
}
