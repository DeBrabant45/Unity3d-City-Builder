using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
    private GameObject _structureModel = null;
    private StructureBaseSO _structureData;
    private bool _isTaken = false;

    public bool IsTaken {get => _isTaken;}
    
    public void SetConstruction(GameObject structureModel, StructureBaseSO structureData)
    {
        if(structureModel == null)
            return;
             
        this._structureModel = structureModel;
        this._structureData = structureData;
        this._isTaken = true; 
    }

    public GameObject GetStructure()
    {
        return _structureModel;
    }

    public void RemoveStructure()
    {
        _structureModel = null;
        _isTaken = false;
        _structureData = null;
    }

    public StructureBaseSO GetStructureData()
    {
        return _structureData;
    }
}
