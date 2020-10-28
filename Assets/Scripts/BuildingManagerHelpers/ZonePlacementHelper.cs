using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ZonePlacementHelper : StructureModificationHelper
{
    private Vector3 _mapBottomLeftCorner;
    private Vector3 _startPoint;
    private Vector3? _previousEndPosition = null;
    private bool _startPositionAcquired = false;
    private Queue<GameObject> _gameObjectsToReuse = new Queue<GameObject>();
    private int _structuresOldQty = 0; 

    public ZonePlacementHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, Vector3 mapBottomLeftCorner, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {
        this._mapBottomLeftCorner = mapBottomLeftCorner;
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
                _resourceManager.ReduceSteelFromShoppingCartAmount(_structureData.requiredSteelAmount);
                _resourceManager.ReduceWoodFromShoppingCartAmount(_structureData.requiredWoodAmount);
            }
            else
            {
                PlaceNewStructureAt(gridPosition, buildingPrefab, gridPositionInt);
                _resourceManager.AddMoneyToShoppingCartAmount(_structureData.placementCost);
                _resourceManager.AddSteelToShoppingCartAmount(_structureData.requiredSteelAmount);
                _resourceManager.AddWoodToShoppingCartAmount(_structureData.requiredWoodAmount);
            }

        }
    }

    private void PlaceNewStructureAt(Vector3 gridPosition, GameObject buildingPrefab, Vector3Int gridPositionInt)
    {
        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }

    private void RevokeStructurePlacementAt(Vector3Int gridPositionInt)
    {
        var structure = _structuresToBeModified[gridPositionInt];
        _placementManager.DestroySingleStructure(structure);
        _structuresToBeModified.Remove(gridPositionInt);
    }

    public override void CancelModifications()
    {
        base.CancelModifications();
    }

    public override void ConfirmModifications()
    {
        if (_structureData.GetType() == typeof(ZoneStructureSO) && ((ZoneStructureSO)_structureData).zoneType == ZoneType.Residential)
        {
            _resourceManager.AddToPopulation(((ZoneStructureSO)_structureData).GetResidentsAmount() * _structuresToBeModified.Count);
        }
        base.ConfirmModifications();
    }

}
