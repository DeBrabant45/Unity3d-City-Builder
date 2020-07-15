public interface IResourceManager
{
    float MoneyCalculationInterval { get; }
    int StartMoneyAmount { get; }
    int RemovalPrice { get; }

    void AddMoneyAmount(int amount);
    void CalculateTownIncome();
    bool CanIBuyIt(int amount);
    bool SpendMoney(int amount);
    int HowManyStructureCanIPlace(int placementCost, int count);
    void PrepareResourceManager(BuildingManager buildingManager);
    void AddToPopulation(int amount);
    void ReducePopulation(int amount);
}