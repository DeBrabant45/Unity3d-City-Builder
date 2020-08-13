using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartHelper
{
    private int _cartAmount;

    public int CartAmount { get => _cartAmount; private set => _cartAmount = value; }

    public void AddCartAmount(int amount)
    {
        CartAmount += amount;
    }

    public void ReduceCartAmount(int amount)
    {
        if (CartAmount > 0)
        {
            CartAmount -= amount;
        }
    }
}
