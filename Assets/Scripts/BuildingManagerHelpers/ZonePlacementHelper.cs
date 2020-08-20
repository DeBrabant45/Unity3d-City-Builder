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

        ZoneCalculator.PrepareStartAndEndPoints(_startPoint, endPoint, ref minPoint, ref maxPoint, _mapBottomLeftCorner);
        HashSet<Vector3Int> newPositionsSet = _grid.GetAllPositionsFromTo(minPoint, maxPoint);

        newPositionsSet = CalculateZoneCost(newPositionsSet);

        _previousEndPosition = endPoint;
        ZoneCalculator.CalculateZone(newPositionsSet, _structuresToBeModified, _gameObjectsToReuse);

        foreach (var positionToPlaceStructure in newPositionsSet)
        {
            if(_grid.IsCellTaken(positionToPlaceStructure))
                continue;

            GameObject structureToAdd = null;
            if(_gameObjectsToReuse.Count > 0)
            {
                var gameObjectToReuse = _gameObjectsToReuse.Dequeue();
                gameObjectToReuse.SetActive(true);
                structureToAdd = _placementManager.MoveStructureOnTheMap(positionToPlaceStructure, gameObjectToReuse, SetZonePrefab());
            }      
            else
            {
                structureToAdd = _placementManager.CreateGhostStructure(positionToPlaceStructure, SetZonePrefab());
            }

            _structuresToBeModified.Add(positionToPlaceStructure, structureToAdd);
        }
    }

    private GameObject SetZonePrefab()
    {
        if (_structureData.GetType() == typeof(ZoneStructureSO) && ((ZoneStructureSO)_structureData).zoneType == ZoneType.Residential)
        {
            return _structureRepository.SetRandomResidentaialPrefab();
        }
        else
        {
            return _structureData.prefab;
        }
    }

    private HashSet<Vector3Int> CalculateZoneCost(HashSet<Vector3Int> newPositionsSet)
    {
        _resourceManager.ReduceShoppingCartAmount(_structuresOldQty * _structureData.placementCost);
        _structuresOldQty = newPositionsSet.Count;
        _resourceManager.AddToShoppingCartAmount(_structuresOldQty * _structureData.placementCost);
        return newPositionsSet;
    }

    public override void CancelModifications()
    {
        base.CancelModifications();
        ResetZonePlacementHelper();
    }

    public override void ConfirmModifications()
    {
        if (_structureData.GetType() == typeof(ZoneStructureSO) && ((ZoneStructureSO)_structureData).zoneType == ZoneType.Residential)
        {
            _resourceManager.AddToPopulation(_structuresToBeModified.Count());
        }
        base.ConfirmModifications();
        ResetZonePlacementHelper();
    }

    private void ResetZonePlacementHelper()
    {
        _structuresOldQty = 0;
        _placementManager.RemoveStructures(_gameObjectsToReuse);
        _gameObjectsToReuse.Clear();
        _startPositionAcquired = false;
        _previousEndPosition = null;
    }

    public override void StopContinuousPlacement()
    {
        _startPositionAcquired = false;
        base.StopContinuousPlacement();
    }
}
