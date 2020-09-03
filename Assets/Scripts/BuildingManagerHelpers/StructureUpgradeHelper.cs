using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureUpgradeHelper : StructureModificationHelper
{
    private Dictionary<Vector3Int, GameObject> _oldStructuresBeforeUpgrade = new Dictionary<Vector3Int, GameObject>();
    private Dictionary<Vector3Int, StructureBaseSO> _newStructureData = new Dictionary<Vector3Int, StructureBaseSO>();

    public StructureUpgradeHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {

    }

    public override void CancelModifications()
    {
        base.CancelModifications();
        SetOldStructuresBackToActive();
        ResetHelpersData();
    }

    public override void ConfirmModifications()
    {
        DestroyOldStructuresForUpgrade();
        PlaceUpgradedStructuresOnTheMap();
        ResetHelpersData();
    }

    private void PlaceUpgradedStructuresOnTheMap()
    {
        _placementManager.PlaceStructuresOnTheMap(_structuresToBeModified.Values);
        Type structureDataType;
        foreach(var stuctureGameObject in _structuresToBeModified)
        {
            foreach (var structureData in _newStructureData)
            {
                if (structureData.Key == stuctureGameObject.Key)
                {
                    PrepareStructureForUpgrade(structureData.Value);
                    structureDataType = structureData.Value.GetType();
                    _grid.PlaceStructureOnTheGrid(stuctureGameObject.Value, structureData.Key, GameObject.Instantiate(structureData.Value));
                    StructureEconomyManager.CheckStructureTypeForUpgradePreparation(structureDataType, structureData.Value, stuctureGameObject.Key, _grid);
                }
            }
        }
    }

    private void PrepareStructureForUpgrade(StructureBaseSO structureData)
    {
        if (structureData != null)
        {
            Type dataType = structureData.GetType();
            if (dataType == typeof(ZoneStructureSO) && ((ZoneStructureSO)structureData).zoneType == ZoneType.Residential)
            {
                var zoneStructure = (ZoneStructureSO)structureData;
                structureData = zoneStructure;
                _resourceManager.SetUpgradedPopulationAmount(zoneStructure.GetResidentsAmount(), zoneStructure.SetUpgradedResidentsAmount());
            }
            if(dataType == typeof(SingleFacilitySO))
            {
                ((SingleFacilitySO)structureData).SetUpgradedMaxCustomers();
            }
            else
            {
                structureData.GetUpgradedIncome();
            }
            structureData.IncreaseUpgradeLevel();
        }
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        var inputStructure = _grid.GetStructureDataFromTheGrid(inputPosition);

        if (_grid.IsCellTaken(gridPosition) == true && inputStructure.upgradable == true && inputStructure.IsFullyUpgraded() == false)
        {
            var structureData = _grid.GetStructureDataFromTheGrid(gridPosition);
            var structureGameObject = _grid.GetStructureFromTheGrid(gridPosition);
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            if (_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructureUpgradePlacementAt(gridPositionInt, structureGameObject);
                _resourceManager.ReduceShoppingCartAmount(structureData.GetUpgradePlacementCost());
            } 
            else
            {
                AddOldStructureForUpgrade(gridPositionInt, structureGameObject);
                PlaceUpgradedGhostStructureAt(gridPosition, gridPositionInt, structureData);
                _resourceManager.AddToShoppingCartAmount(structureData.GetUpgradePlacementCost());
            }

        }
    }

    private void RevokeStructureUpgradePlacementAt(Vector3Int gridPositionInt, GameObject structureGameObject)
    {
        DestroyUpgradeStructureGameObjectAt(gridPositionInt);
        _placementManager.ResetBuildingLook(structureGameObject);
        RemoveStructureToBeModified(gridPositionInt);
        RemoveOldStructureForUpgrade(gridPositionInt, structureGameObject);
        RemoveNewStuctureDataForUpgrade(gridPositionInt);
    }

    private void PlaceUpgradedGhostStructureAt(Vector3 gridPosition, Vector3Int gridPositionInt, StructureBaseSO structureData)
    {
        //structureData.prefab = _structureRepository.GetUpgradeBuildingPrefab(structureData);
        AddNewStructureDataForUpgrade(gridPositionInt, structureData);
        AddStructureToBeModified(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, structureData.GetUpgradedPrefab()));
    }

    private void DestroyUpgradeStructureGameObjectAt(Vector3Int gridPositionInt)
    {
        var upGradedStructureGameObject = _structuresToBeModified[gridPositionInt];
        _placementManager.DestroySingleStructure(upGradedStructureGameObject);
    }

    private void AddOldStructureForUpgrade(Vector3Int gridPositionInt, GameObject structureGameObject)
    {
        structureGameObject.SetActive(false);
        _oldStructuresBeforeUpgrade.Add(gridPositionInt, structureGameObject);
    }

    private void RemoveOldStructureForUpgrade(Vector3Int gridPositionInt, GameObject structureGameObject)
    {
        structureGameObject.SetActive(true);
        _oldStructuresBeforeUpgrade.Remove(gridPositionInt);
    }

    private void AddNewStructureDataForUpgrade(Vector3Int gridPositionInt, StructureBaseSO structureData)
    {
        _newStructureData.Add(gridPositionInt, structureData);
    }

    private void RemoveNewStuctureDataForUpgrade(Vector3Int gridPositionInt)
    {
        _newStructureData.Remove(gridPositionInt);
    }

    private void SetOldStructuresBackToActive()
    {
        foreach (var oldStructure in _oldStructuresBeforeUpgrade.Values)
        {
            oldStructure.SetActive(true);
        }

        _oldStructuresBeforeUpgrade.Clear();
    }

    private void DestroyOldStructuresForUpgrade()
    {
        if (_oldStructuresBeforeUpgrade != null)
        {
            foreach (var oldStructure in _oldStructuresBeforeUpgrade.Values)
            {
                oldStructure.SetActive(true);
                _placementManager.DestroySingleStructure(oldStructure);
            }
            _oldStructuresBeforeUpgrade.Clear();
        }
    }

    //private int StructureUpgradeIncome(StructureBaseSO structureData)
    //{
    //    return _structureRepository.GetStructureUpgradeIncome(structureData);
    //}

    private void ResetHelpersData()
    {
        _structuresToBeModified.Clear();
        _newStructureData.Clear();
    }

    public GameObject AccessStructureInOldStructuresDictionary(Vector3 gridPosition)
    {
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (_oldStructuresBeforeUpgrade.ContainsKey(gridPositionInt))
        {
            return _oldStructuresBeforeUpgrade[gridPositionInt];
        }
        return null;
    }

    public StructureBaseSO AccessStructureInNewStructureDataDictionary(Vector3 gridPosition)
    {
        var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
        if (_newStructureData.ContainsKey(gridPositionInt))
        {
            return _newStructureData[gridPositionInt];
        }
        return null;
    }
}
