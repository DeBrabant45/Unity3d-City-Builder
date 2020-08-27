using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class StructureModificationHelper
{
    protected StructureRepository _structureRepository;
    protected GridStructure _grid;
    protected IPlacementManager _placementManager;
    protected Dictionary<Vector3Int, GameObject> _structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
    protected StructureBaseSO _structureData;
    protected IResourceManager _resourceManager;

    public StructureModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager)
    {
        this._structureRepository = structureRepository;
        this._grid = grid;
        this._placementManager = placementManager;
        this._resourceManager = resourceManager;
        _structureData = ScriptableObject.CreateInstance<NullStructureSO>();
    }

    public virtual void ConfirmModifications()
    {
        _placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        Type structureType = _structureData.GetType();
        foreach (var keyValuePair in _structuresToBeModified)
        {
            _grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(_structureData));
            StructureEconomyManager.CheckStructureTypeForCreationPreparation(structureType, keyValuePair.Key, _grid);
        }
        ResetHelpersData();
    }

    public virtual void CancelModifications()
    {
        _placementManager.RemoveStructures(_structuresToBeModified.Values);
        _resourceManager.ClearShoppingCartAmount();
        ResetHelpersData();
    }

    private void ResetHelpersData()
    {
        _structuresToBeModified.Clear();
        _structureData = ScriptableObject.CreateInstance<NullStructureSO>();
    }

    public virtual void StopContinuousPlacement()
    {

    }

    public virtual void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        if(_structureData.GetType() == typeof(NullStructureSO) && structureType != StructureType.None)
        {
            _structureData = this._structureRepository.GetStructureData(structureName, structureType);
        }
    }

    public GameObject AccessStructureInDictionary(Vector3 gridPosition)
    {
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (_structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return _structuresToBeModified[gridPositionInt];
        }
        return null;
    }

    public void AddStructureToBeModified(Vector3Int gridPositionInt, GameObject structureGameObject)
    {
        _structuresToBeModified.Add(gridPositionInt, structureGameObject);
    }

    public void RemoveStructureToBeModified(Vector3Int gridPositionInt)
    {
        _structuresToBeModified.Remove(gridPositionInt);
    }
}
