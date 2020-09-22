public interface IResourceManager
{
    float MoneyCalculationInterval { get; }
    float WoodCalculationInterval { get; }
    int StartMoneyAmount { get; }
    int RemovalPrice { get; }

    void AddMoneyAmount(int amount);
    void CalculateTownIncome();
    void CalculateTownTotalWoodAmount();
    bool CanIBuyIt(int moneyAmount, int steelAmount, int woodAmount);
    void SpendMoney(int amount);
    void PrepareResourceManager(BuildingManager buildingManager);
    void AddToPopulation(int amount);
    void ReducePopulation(int amount);
    void SetUpgradedPopulationAmount(int pastAmount, int newAmount);
    void AddMoneyToShoppingCartAmount(int amount);
    void ReduceMoneyFromShoppingCartAmount(int amount);
    void AddWoodToShoppingCartAmount(int amount);
    void ReduceWoodFromShoppingCartAmount(int amount);
    void AddSteelToShoppingCartAmount(int amount);
    void ReduceSteelFromShoppingCartAmount(int amount);
    int ShoppingCartMoneyAmount();
    int ShoppingCartWoodAmount();
    int ShoppingCartSteelAmount();
    void ClearShoppingCartAmount();
}