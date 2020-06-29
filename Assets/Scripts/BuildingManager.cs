using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingManager
{
    private GridStructure _grid;
    private PlacementManager _placementManager;
    private StructureRepository _structureRepository;
    private Dictionary<Vector3Int, GameObject> _structuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public BuildingManager(int cellSize, int width, int length, PlacementManager placementManager, StructureRepository structureRepository)
    {
        _grid = new GridStructure(cellSize, width, length);
        this._placementManager = placementManager;
        this._structureRepository = structureRepository;
    }

    public void PrepareStructureForPlacement(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        GameObject buildingPrefab = this._structureRepository.GetBuildingPrefabByName(structureName, structureType);
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if(!_grid.IsCellTaken(gridPosition))
        {
            if(_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructurePlacementAt(gridPositionInt);
            }
            else
            {
               PlaceNewStructureAt(gridPosition, buildingPrefab, gridPositionInt);
            }
           
        }
    }

    private void RevokeStructurePlacementAt(Vector3Int gridPositionInt)
    {
        var structure = _structuresToBeModified[gridPositionInt];
        _placementManager.DestroySingleStructure(structure);
        _structuresToBeModified.Remove(gridPositionInt);
    }

    private void PlaceNewStructureAt(Vector3 gridPosition, GameObject buildingPrefab, Vector3Int gridPositionInt)
    {
        _structuresToBeModified.Add(gridPositionInt,_placementManager.CreateGhostStructure(gridPosition, buildingPrefab)); 
    }

    public void ConfirmPlacement()
    {
        _placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        foreach(var keyValuePair in _structuresToBeModified)
        {
            _grid.PlaceStructureOnTheGrid(keyValuePair.Value, keyValuePair.Key);
        }
        _structuresToBeModified.Clear();
    }

    public void CancelPlacement()
    {
        _placementManager.RemoveStructures(_structuresToBeModified.Values);
        _structuresToBeModified.Clear();
    }

    public void PrepareStructureForRemovalAt(Vector3 inputPosition)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if(_grid.IsCellTaken(gridPosition) == true)
        {
            var structure = _grid.GetStructureFromTheGrid(gridPosition);
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            if(_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructureRemovalPlacementAt(gridPositionInt, structure);
            }
            else
            {
                AddStructureForRemoval(gridPositionInt, structure);
            }

        }
    }

    private void RevokeStructureRemovalPlacementAt(Vector3Int gridPositionInt, GameObject structure)
    {
        _placementManager.ResetBuildingMaterial(structure);
        _structuresToBeModified.Remove(gridPositionInt);
    }

    private void AddStructureForRemoval(Vector3Int gridPositionInt, GameObject structure)
    {
        _structuresToBeModified.Add(gridPositionInt, structure);
        _placementManager.SetBuildingForRemoval(structure);
    }

    public void CancelRemoval()
    {
        this._placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        _structuresToBeModified.Clear();
    }

    public void ConfirmRemoval()
    {
        foreach(var gridPosition in _structuresToBeModified.Keys)
        {
            _grid.RemoveStrucutreFromTheGrid(gridPosition);
        }
        this._placementManager.RemoveStructures(_structuresToBeModified.Values);
        _structuresToBeModified.Clear();
    }

    public GameObject CheckForStructureInGrid(Vector3 inputPosition)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if(_grid.IsCellTaken(gridPosition))
        {
            return _grid.GetStructureFromTheGrid(gridPosition);
        }
        return null;
    }

    public GameObject CheckForStructureInDictionary(Vector3 inputPosition)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if(_structuresToBeModified.ContainsKey(gridPositionInt))
        {
            return _structuresToBeModified[gridPositionInt];
        }
        return null;
    }
}
