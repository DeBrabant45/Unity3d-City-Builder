using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureRemovalHelper : StructureModificationHelper
{
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
    }

    private void AddStructureForRemoval(Vector3Int gridPositionInt, GameObject structure)
    {
        _structuresToBeModified.Add(gridPositionInt, structure);
        _placementManager.SetBuildingForRemoval(structure);
    }
}
