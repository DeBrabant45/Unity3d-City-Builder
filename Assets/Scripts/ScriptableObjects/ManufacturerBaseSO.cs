using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Manufactory", menuName = "CityBuilder/StructureData/Manufactory")]
public class ManufacturerBaseSO : StructureBaseSO
{
    [SerializeField]
    private int materialAmount;

    public int GetMaterialAmount()
    {
        return materialAmount;
    }
}
