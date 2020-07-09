using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonePlacementHelper : StructureModificationHelper
{
    private Vector3 _mapBottomLeftCorner;
    private Vector3 _startPoint;
    private Vector3? _previousEndPosition = null;
    private bool _startPositionAcquired = false;
    private Queue<GameObject> _gameObjectsToReuse = new Queue<GameObject>();

    public ZonePlacementHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, Vector3 mapBottomLeftCorner) 
        : base(structureRepository, grid, placementManager)
    {
        this._mapBottomLeftCorner = mapBottomLeftCorner;
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);

        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        if(_startPositionAcquired == false && _grid.IsCellTaken(gridPosition) == false)
        {
            _startPoint = gridPosition;
            _startPositionAcquired = true;
        }
        if(_startPositionAcquired == true && (_previousEndPosition == null || ZoneCalculator.CheckIfPositionHasChanged(gridPosition, _previousEndPosition.Value, _grid)))
        {
            PlaceNewZoneUpToPosition(gridPosition);
        }

    }

    private void PlaceNewZoneUpToPosition(Vector3 endPoint)
    {
        Vector3Int minPoint = Vector3Int.FloorToInt(_startPoint);
        Vector3Int maxPoint = Vector3Int.FloorToInt(endPoint);

        ZoneCalculator.PrepareStartAndEndPoints(_startPoint, endPoint, minPoint, maxPoint, _mapBottomLeftCorner);
        HashSet<Vector3Int> newPositionsSet = _grid.GetAllPositionsFromTo(minPoint, maxPoint);
        _previousEndPosition = endPoint;
        ZoneCalculator.CalculateZone(newPositionsSet, _structuresToBeModified, _gameObjectsToReuse);
    }
}
