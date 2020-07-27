using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureUpgradeHelper : StructureModificationHelper
{
    public StructureUpgradeHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {

    }

    public override void CancelModifications()
    {
        this._placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
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
                //_resourceManager.AddMoneyAmount(_resourceManager.RemovalPrice);
                RevokeStructureUpgradePlacementAt(gridPositionInt, structure);
            }
            else if (_resourceManager.CanIBuyIt(_resourceManager.RemovalPrice))
            {
                AddStructureForUpgrade(gridPositionInt, structure);
                //_resourceManager.SpendMoney(_resourceManager.RemovalPrice);
            }

        }
    }

    private void RevokeStructureUpgradePlacementAt(Vector3Int gridPositionInt, GameObject structure)
    {
        _placementManager.ResetBuildingLook(structure);
        _structuresToBeModified.Remove(gridPositionInt);
    }

    private void AddStructureForUpgrade(Vector3Int gridPositionInt, GameObject structure)
    {
        _structuresToBeModified.Add(gridPositionInt, structure);
        _placementManager.SetBuildingForUpgrade(structure);
    }
}
