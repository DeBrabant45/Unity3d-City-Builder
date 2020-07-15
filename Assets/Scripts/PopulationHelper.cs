using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopulationHelper
{
    private int _population;

    public int Population { get => _population; private set => _population = value; }

    public void AddPopulation(int amount)
    {
        Population += amount;
    }

    public void ReducePopulation(int amount)
    {
        if(Population > 0)
        {
            Population -= amount;
        }
    }
}
