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
        RemoveOldStructuresFromUpgrade();
        _structuresToBeModified.Clear();
    }

    public override void ConfirmModifications()
    {
        base.ConfirmModifications();
        foreach (var gridPosition in _structuresToBeModified.Keys)
        {
            PrepareStructureForUpgrade(gridPosition);
        }

        if(_oldStructuresBeforeUpgrade != null)
        {
            foreach (var item in _oldStructuresBeforeUpgrade.Values)
            {
                item.SetActive(true);
                _placementManager.DestroySingleStructure(item);
            }
        }

        _structuresToBeModified.Clear();
        _oldStructuresBeforeUpgrade.Clear();
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
            //StructureEconomyManager.CheckStructureTypeForRemovalPreparation(dataType, gridPosition, _grid);
        }
    }

    public override void PrepareStructureForModification(Vector3 inputPosition, string structureName, StructureType structureType)
    {
        base.PrepareStructureForModification(inputPosition, structureName, structureType);
        GameObject buildingPrefab = _structureRepository.modelDataCollection.zoneStructures.Select(zone => zone.prefabVariants[0]).FirstOrDefault();
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
                AddOldStructureForUpgrade(gridPositionInt, structure);
                PlaceUpgradedStructureAt(gridPosition, buildingPrefab, gridPositionInt);
                //_resourceManager.SpendMoney(_resourceManager.RemovalPrice);
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

    private void RemoveOldStructuresFromUpgrade()
    {
        foreach (var oldStructure in _oldStructuresBeforeUpgrade.Values)
        {
            oldStructure.SetActive(true);
        }

        _oldStructuresBeforeUpgrade.Clear();
    }

    private void PlaceUpgradedStructureAt(Vector3 gridPosition, GameObject buildingPrefab, Vector3Int gridPositionInt)
    {
        _structuresToBeModified.Add(gridPositionInt, _placementManager.CreateGhostStructure(gridPosition, buildingPrefab));
    }
}
