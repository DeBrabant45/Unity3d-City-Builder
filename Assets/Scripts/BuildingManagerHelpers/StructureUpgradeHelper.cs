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
            PrepareStructureForUpgrade(stuctureGameObject.Key);
            foreach (var structureData in _newStructureData)
            {
                if(structureData.Key == stuctureGameObject.Key)
                {
                    structureDataType = structureData.Value.GetType();
                    _grid.PlaceStructureOnTheGrid(stuctureGameObject.Value, structureData.Key, GameObject.Instantiate(structureData.Value));
                    StructureEconomyManager.CheckStructureTypeForUpgradePreparation(structureDataType, structureData.Value, stuctureGameObject.Key, _grid);
                }
            }
        }
    }

    private void PrepareStructureForUpgrade(Vector3Int gridPosition)
    {
        var structureData = _grid.GetStructureDataFromTheGrid(gridPosition);
        if (structureData != null)
        {
            Type dataType = structureData.GetType();
            if (dataType == typeof(ZoneStructureSO) && ((ZoneStructureSO)structureData).zoneType == ZoneType.Residential)
            {
                _resourceManager.AddToPopulation(4);
            }
            if (dataType == typeof(ZoneStructureSO))
            {
                structureData.SetUpgradedIncome(StructureUpgradeIncome(structureData));
            }
            if(dataType == typeof(SingleFacilitySO))
            {
                ((SingleFacilitySO)structureData).SetUpgradedMaxCustomers();
            }
            structureData.upgradeActive = true;
        }
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);
        var inputStructure = _grid.GetStructureDataFromTheGrid(inputPosition);

        if (_grid.IsCellTaken(gridPosition) == true && inputStructure.upgradable == true && inputStructure.upgradeActive == false)
        {
            var structureBase = _grid.GetStructureDataFromTheGrid(gridPosition);
            var structure = _grid.GetStructureFromTheGrid(gridPosition);
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            if (_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructureUpgradePlacementAt(gridPositionInt, structure);
                _resourceManager.ReduceShoppingCartAmount(structureBase.upgradePlacementCost);
            } 
            else
            {
                AddOldStructureForUpgrade(gridPositionInt, structure);
                PlaceUpgradedStructureAt(gridPosition, gridPositionInt, structureBase);
                _resourceManager.AddToShoppingCartAmount(structureBase.upgradePlacementCost);
            }

        }
    }

    private void RevokeStructureUpgradePlacementAt(Vector3Int gridPositionInt, GameObject structure)
    {
        _placementManager.ResetBuildingLook(structure);
        var upGradedStructure = _structuresToBeModified[gridPositionInt];
        _placementManager.DestroySingleStructure(upGradedStructure);
        _structuresToBeModified.Remove(gridPositionInt);
        _oldStructuresBeforeUpgrade.Remove(gridPositionInt);
        _newStructureData.Remove(gridPositionInt);
        structure.SetActive(true);
    }

    private void AddOldStructureForUpgrade(Vector3Int gridPositionInt, GameObject structure)
    {
        structure.SetActive(false);
        _oldStructuresBeforeUpgrade.Add(gridPositionInt, structure);
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
            foreach (var item in _oldStructuresBeforeUpgrade.Values)
            {
                item.SetActive(true);
                _placementManager.DestroySingleStructure(item);
            }
        }
        _oldStructuresBeforeUpgrade.Clear();
    }

    private void PlaceUpgradedStructureAt(Vector3 gridPosition, Vector3Int gridPositionInt, StructureBaseSO structureBase)
    {
        structureBase.prefab = _structureRepository.GetUpgradeBuildingPrefab(structureBase);
        _newStructureData.Add(gridPositionInt, structureBase);
        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, structureBase.prefab));
    }

    public int StructureUpgradeIncome(StructureBaseSO structureData)
    {
        return _structureRepository.GetStructureUpgradeIncome(structureData);
    }

    private void ResetHelpersData()
    {
        _structuresToBeModified.Clear();
        _newStructureData.Clear();
    }
}
