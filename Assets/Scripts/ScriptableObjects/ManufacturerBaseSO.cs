using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manufactory", menuName = "CityBuilder/StructureData/Manufactory")]
public class ManufacturerBaseSO : StructureBaseSO
{
    [SerializeField]
    private int _materialAmount;
    [SerializeField]
    private int _materialBuildTimer;
    [SerializeField]
    private ManufactureType _manufactureType = ManufactureType.None;
    private Stopwatch _stopwatch = new Stopwatch();

    public ManufactureType ManufactureType { get => _manufactureType; }

    public int GetMaterialAmount()
    {
        if(MaterialPlaceTimer() == true)
        {
            return _materialAmount;
        }
        return 0;
    }

    private bool MaterialPlaceTimer()
    {
        _stopwatch.Start();
        while (_stopwatch.Elapsed >= TimeSpan.FromSeconds(_materialBuildTimer))
        {
            _stopwatch.Stop();
            _stopwatch.Reset();
            return true;
        }

        return false;
    }

    public int GetMaterialBuildTimer()
    {
        return _materialBuildTimer;
    }
}

public enum ManufactureType
{
    Wood,
    Steel,
    None
}
