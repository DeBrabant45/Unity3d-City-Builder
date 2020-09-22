using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartHelper
{
    private int _shoppingCartMoneyAmount;
    private int _shoppingCartWoodAmount;
    private int _shoppingCartSteelAmount;

    public int ShoppingCartMoneyAmount { get => _shoppingCartMoneyAmount; }
    public int ShoppingCartWoodAmount { get => _shoppingCartWoodAmount; }
    public int ShoppingCartSteelAmount { get => _shoppingCartSteelAmount; }

    public void AddMoneyToShoppingCartAmount(int amount)
    {
        _shoppingCartMoneyAmount += amount;
    }

    public void ReduceMoneyFromShoppingCartAmount(int amount)
    {
        if (_shoppingCartMoneyAmount > 0)
        {
            _shoppingCartMoneyAmount -= amount;
        }
    }

    public void AddWoodToShoppingCartAmount(int amount)
    {
        _shoppingCartWoodAmount += amount;
    }

    public void ReduceWoodFromShoppingCartAmount(int amount)
    {
        if (_shoppingCartWoodAmount > 0)
        {
            _shoppingCartWoodAmount -= amount;
        }
    }

    public void AddSteelToShoppingCartAmount(int amount)
    {
        _shoppingCartSteelAmount += amount;
    }

    public void ReduceSteelFromShoppingCartAmount(int amount)
    {
        if (_shoppingCartSteelAmount > 0)
        {
            _shoppingCartSteelAmount -= amount;
        }
    }

    public void ClearShoppingCartAmount()
    {
        _shoppingCartMoneyAmount = 0;
        _shoppingCartSteelAmount = 0;
        _shoppingCartWoodAmount = 0;
    }
}
