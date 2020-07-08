using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRemovalHelper : StructureModificationHelper
{
    private Dictionary<Vector3Int, GameObject> _roadsToBeRemoved = new Dictionary<Vector3Int, GameObject>();

    public StructureRemovalHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager) : base(structureRepository, grid, placementManager)
    {

    }

    public override void CancelModifications()
    {
        this._placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        _structuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        foreach (var gridPosition in _structuresToBeModified.Keys)
        {
            _grid.RemoveStrucutreFromTheGrid(gridPosition);
        }

        foreach (var keyValuePair in _roadsToBeRemoved)
        {
            Dictionary<Vector3Int, GameObject> neighborDictionary = RoadManager.GetRoadNeighborsPosition(_grid, keyValuePair.Key);
            if(neighborDictionary.Count > 0)
            {
                var structureData = _grid.GetStructureDataFromTheGrid(neighborDictionary.Keys.First());
                RoadManager.ModifyRoadCellsOnTheGrid(neighborDictionary, structureData, null, _grid, _placementManager);
            }

        }

        this._placementManager.RemoveStructures(_structuresToBeModified.Values);
        _structuresToBeModified.Clear();
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if (_grid.IsCellTaken(gridPosition) == true)
        {
            var structure = _grid.GetStructureFromTheGrid(gridPosition);
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            if (_structuresToBeModified.ContainsKey(gridPositionInt))
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
        _placementManager.ResetBuildingLook(structure);
        _structuresToBeModified.Remove(gridPositionInt);
        if (RoadManager.CheckIfNeighborHasRoadOnTheGrid(_grid, gridPositionInt) && _roadsToBeRemoved.ContainsKey(gridPositionInt))
        {
            _roadsToBeRemoved.Remove(gridPositionInt);
        }
    }

    private void AddStructureForRemoval(Vector3Int gridPositionInt, GameObject structure)
    {
        _structuresToBeModified.Add(gridPositionInt, structure);
        _placementManager.SetBuildingForRemoval(structure);

        if(RoadManager.CheckIfNeighborHasRoadOnTheGrid(_grid, gridPositionInt) && _roadsToBeRemoved.ContainsKey(gridPositionInt) == false)
        {
            _roadsToBeRemoved.Add(gridPositionInt, structure);
        }
    }
}
