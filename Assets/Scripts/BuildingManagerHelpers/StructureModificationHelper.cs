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

    public StructureModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager)
    {
        this._structureRepository = structureRepository;
        this._grid = grid;
        this._placementManager = placementManager;
    }

    public GameObject AccessStructureInDictionary(Vector3 gridPosition)
    {
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if(_structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return _structuresToBeModified[gridPositionInt];
        }
        return null;
    }

    public virtual void ConfirmModifications()
    {
        _placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        foreach (var keyValuePair in _structuresToBeModified)
        {
            _grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key, GameObject.Instantiate(_structureData));
        }
        ResetHelpersData();
    }

    public virtual void CancelModifications()
    {
        _placementManager.RemoveStructures(_structuresToBeModified.Values);
        ResetHelpersData();
    }

    private void ResetHelpersData()
    {
        _structuresToBeModified.Clear();
        _structureData = null;
    }

    public virtual void StopContinuousPlacement()
    {

    }

    public virtual void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        if(_structureData == null && structureType != StructureType.None)
        {
            _structureData = this._structureRepository.GetStructureData(structureName, structureType);
        }
    }

}
