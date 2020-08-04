using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureUpgradeHelper : StructureModificationHelper
{
    private Dictionary<Vector3Int, GameObject> _oldStructuresBeforeUpgrade = new Dictionary<Vector3Int, GameObject>();

    public StructureUpgradeHelper(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager) 
        : base(structureRepository, grid, placementManager, resourceManager)
    {

    }

    public override void CancelModifications()
    {
        base.CancelModifications();
        SetOldStructuresBackToActive();
        _structuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {

        foreach (var gridPosition in _structuresToBeModified.Keys)
        {
            PrepareStructureForUpgrade(gridPosition);
        }
        DestroyOldStructuresForUpgrade();
        base.ConfirmModifications();
        _structuresToBeModified.Clear();
    }


    private void PrepareStructureForUpgrade(Vector3Int gridPosition)
    {
        var data = _grid.GetStructureDataFromTheGrid(gridPosition);
        if (data != null)
        {
            Type dataType = data.GetType();
            if (dataType == typeof(ZoneStructureSO) && ((ZoneStructureSO)data).zoneType == ZoneType.Residentaial)
            {
                _resourceManager.AddToPopulation(4);
            }
            data.upgradeActive = true;
            data.SetUpgradedIncome(GetStructureUpgradeIncome(data));
            StructureEconomyManager.CheckStructureTypeForCreationPreparation(dataType, gridPosition, _grid);
        }
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        Vector3 gridPosition = _grid.CalculateGridPosition(inputPosition);

        if (_grid.IsCellTaken(gridPosition) == true && _grid.GetStructureDataFromTheGrid(inputPosition).upgradable == true)
        {
            _structureData = _grid.GetStructureDataFromTheGrid(gridPosition);
            var structure = _grid.GetStructureFromTheGrid(gridPosition);
            var gridPositionInt = Vector3Int.FloorToInt(gridPosition);
            if (_structuresToBeModified.ContainsKey(gridPositionInt))
            {
                RevokeStructureUpgradePlacementAt(gridPositionInt, structure);
                _resourceManager.AddMoneyAmount(_structureData.upgradePlacementCost);
            } 
            else if (_resourceManager.CanIBuyIt(_structureData.upgradePlacementCost))
            {
                AddOldStructureForUpgrade(gridPositionInt, structure);
                PlaceUpgradedStructureAt(gridPosition, gridPositionInt);
                _resourceManager.SpendMoney(_structureData.upgradePlacementCost);
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

    private void PlaceUpgradedStructureAt(Vector3 gridPosition, Vector3Int gridPositionInt)
    {
        GameObject buildingPrefab = null;
        foreach (var structure in _structureRepository.modelDataCollection.zoneStructures)
        {
            if(_grid.GetStructureDataFromTheGrid(gridPosition).buildingName == structure.buildingName)
            {
                buildingPrefab = structure.upgradePrefabVariants[0];                
            }
        }

        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }

    public int GetStructureUpgradeIncome(StructureBaseSO structure)
    {
        int upgradeAmountToReturn = 0;
        
        foreach (var zone in _structureRepository.modelDataCollection.zoneStructures)
        {
            if (structure.GetType() == typeof(ZoneStructureSO) && ((ZoneStructureSO)structure).zoneType == zone.zoneType)
            {
                upgradeAmountToReturn = zone.upgradedIncome;
            }
        }

        return upgradeAmountToReturn;
    }
}
