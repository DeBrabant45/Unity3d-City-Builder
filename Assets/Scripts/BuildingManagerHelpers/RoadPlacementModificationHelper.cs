using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadPlacementModificationHelper : StructureModificationHelper
{
    private Dictionary<Vector3Int, GameObject> _existingRoadStructuresToBeModified = new Dictionary<Vector3Int, GameObject>();

    public RoadPlacementModificationHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager) : base(structureRepository, grid, placementManager)
    {
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if(_grid.IsCellTaken(gridPosition) == false)
        {
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            var roadStructure = GetCorrectRoadPrefab(gridPosition);
            if(_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeRoadPlacementAt(gridPositionInt);
            }
            else
            {
                PlaceNewRoadAt(roadStructure, gridPosition, gridPositionInt);
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
            var neighborStructure = GetCorrectRoadPrefab(neighborGridPosition.Value);
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

    private RoadStructureHelper GetCorrectRoadPrefab(Vector3 gridPosition)
    {
        var neighborStatus = RoadManager.GetRoadNeighborStatus(gridPosition, _grid, _structuresToBeModified);
        RoadStructureHelper roadToReturn = null;
        roadToReturn = RoadManager.CheckIfStraightRoadFits(neighborStatus, roadToReturn, _structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfCornerRoadFits(neighborStatus, roadToReturn, _structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfThreewayRoadFits(neighborStatus, roadToReturn, _structureData);
        if (roadToReturn != null)
            return roadToReturn;
        roadToReturn = RoadManager.CheckIfFourwayRoadFits(neighborStatus, roadToReturn, _structureData);
        if (roadToReturn != null)
            return roadToReturn;

        return roadToReturn;
    }

    public override void CancelModifications()
    {
        base.CancelModifications();
        _existingRoadStructuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        foreach (var keyValuePair in _existingRoadStructuresToBeModified)
        {
            _grid.RemoveStrucutreFromTheGrid(keyValuePair.Key);
            _placementManager.DestroySingleStructure(keyValuePair.Value);
            var roadStructure = GetCorrectRoadPrefab(keyValuePair.Key);
            var structure = _placementManager.PlaceStructureOnTheMap(keyValuePair.Key, roadStructure.RoadPrefab, roadStructure.RoadPrefabRotation);
            _grid.PlaceStructureOnTheGrid(structure, keyValuePair.Key, _structureData);
        }
        _existingRoadStructuresToBeModified.Clear();
        base.ConfirmModifications();
    }

}
