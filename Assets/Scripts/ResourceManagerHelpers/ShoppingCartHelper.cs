using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShoppingCartHelper
{
    private int _shoppingCartAmount;

    public int ShoppingCartAmount { get => _shoppingCartAmount; private set => _shoppingCartAmount = value; }

    public void AddShoppingCartAmount(int amount)
    {
        ShoppingCartAmount += amount;
    }

    public void ReduceShoppingCartAmount(int amount)
    {
        if (ShoppingCartAmount > 0)
        {
            ShoppingCartAmount -= amount;
        }
    }

    public void ClearCartAmount()
    {
        _shoppingCartAmount = 0;
    }
}
