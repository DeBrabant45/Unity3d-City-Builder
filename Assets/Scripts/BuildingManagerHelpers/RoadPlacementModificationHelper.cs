using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    private Dictionary<Vector3Int, GameObject> _existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if(_grid.IsCellTaken(gridPosition) == false)
        {
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            var roadStructure = RoadManager.GetCorrectRoadPrefab(gridPosition, _structureData, _structuresToBeModified, _grid);
            if(_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeRoadPlacementAt(gridPositionInt);
                _resourceManager.ReduceShoppingCartAmount(_structureData.placementCost);
            }
            else
            {
                PlaceNewRoadAt(roadStructure, gridPosition, gridPositionInt);
                _resourceManager.AddToShoppingCartAmount(_structureData.placementCost);
            }
            AdjustNeighborsRoadStructures(gridPosition);
        }
    }

    private void AdjustNeighborsRoadStructures(Vector3 gridPosition)
    {
        AdjustNeighborsRoad(gridPosition, Direction.Up);
        AdjustNeighborsRoad(gridPosition, Direction.Down);
        AdjustNeighborsRoad(gridPosition, Direction.Left);
        AdjustNeighborsRoad(gridPosition, Direction.Right);
    }

    private void AdjustNeighborsRoad(Vector3 gridPosition, Direction direction)
    {
        var neighborGridPosition = _grid.GetPositionOfNeighborIfExists(gridPosition, direction);
        if(neighborGridPosition.HasValue)
        {
            var neighborPositionInt = neighborGridPosition.Value;
            AdjustStructureIfIsInDictionary(neighborGridPosition, neighborPositionInt);
            AdjustStructureIfIsOnGrid(neighborGridPosition, neighborPositionInt);
        }
    }

    private void AdjustStructureIfIsOnGrid(Vector3Int? neighborGridPosition, Vector3Int neighborPositionInt)
    {
        if (RoadManager.CheckIfNeighborHasRoadOnTheGrid(_grid, neighborPositionInt))
        {
            var neighborStructureData = _grid.GetStructureDataFromTheGrid(neighborGridPosition.Value);
            if (neighborStructureData != null && neighborStructureData.GetType() == typeof(RoadStructureSO) && _existingRoadStructuresToBeModified.ContainsKey(neighborPositionInt) == false)
            {
                _existingRoadStructuresToBeModified.Add(neighborPositionInt, _grid.GetStructureFromTheGrid(neighborGridPosition.Value));
            }
        }
    }

    private void AdjustStructureIfIsInDictionary(Vector3Int? neighborGridPosition, Vector3Int neighborPositionInt)
    {
        if (RoadManager.CheckIfNeighborHasRoadWithinDictionary(neighborPositionInt, _structuresToBeModified))
        {
            RevokeRoadPlacementAt(neighborPositionInt);
            var neighborStructure = RoadManager.GetCorrectRoadPrefab(neighborGridPosition.Value, _structureData, _structuresToBeModified, _grid);
            PlaceNewRoadAt(neighborStructure, neighborGridPosition.Value, neighborPositionInt);
        }
    }

    private void PlaceNewRoadAt(RoadStructureHelper roadStructure, Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation));
    }

    private void RevokeRoadPlacementAt(Vector3Int gridPositionInt)
    {
        var structure = _structuresToBeModified[gridPositionInt];
        _placementManager.DestroySingleStructure(structure);
        _structuresToBeModified.Remove(gridPositionInt);
    }

    public override void CancelModifications()
    {
        base.CancelModifications();
        _existingRoadStructuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        RoadManager.ModifyRoadCellsOnTheGrid(_existingRoadStructuresToBeModified, _structureData, _structuresToBeModified, _grid, _placementManager);
        base.ConfirmModifications();
    }

}
