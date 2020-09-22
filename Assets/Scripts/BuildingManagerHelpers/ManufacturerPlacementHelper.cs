using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManufacturerPlacementHelper : StructureModificationHelper
{
    public ManufacturerPlacementHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        GameObject buildingPrefab = _structureData.prefab;
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);

        if (!_grid.IsCellTaken(gridPosition))
        {
            if (_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructurePlacementAt(gridPositionInt);
                _resourceManager.ReduceMoneyFromShoppingCartAmount(_structureData.placementCost);
            }
            else
            {
                PlaceNewStructureAt(gridPosition, buildingPrefab, gridPositionInt);
                _resourceManager.AddMoneyToShoppingCartAmount(_structureData.placementCost);
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
        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }

    public override void CancelModifications()
    {
        base.CancelModifications();
    }
}
