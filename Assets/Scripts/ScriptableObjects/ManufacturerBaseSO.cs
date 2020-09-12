using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manufactory", menuName = "CityBuilder/StructureData/Manufactory")]
public class ManufacturerBaseSO : StructureBaseSO
{
    [SerializeField]
    private int materialAmount;
    [SerializeField]
    private int materialBuildTimer;
    private Stopwatch stopwatch = new Stopwatch();
    private int materialCountDown;

    public int GetMaterialAmount()
    {
        if(MaterialPlaceTimer() == true)
        {
            return materialAmount;
        }
        return 0;
    }

    private bool MaterialPlaceTimer()
    {
        stopwatch.Start();
        while (stopwatch.Elapsed >= TimeSpan.FromSeconds(materialBuildTimer))
        {
            stopwatch.Stop();
            stopwatch.Reset();
            return true;
        }

        return false;
    }

    public int GetMaterialCountDownTimer()
    {
        return materialCountDown;
    }


}
