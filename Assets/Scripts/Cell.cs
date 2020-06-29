using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell 
{
    private GameObject _structureModel = null;
    private bool _isTaken = false;

    public bool IsTaken {get => _isTaken;}
    
    public void SetConstruction(GameObject structureModel)
    {
        if(structureModel == null)
            return;
             
        this._structureModel = structureModel;
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
    }
}
