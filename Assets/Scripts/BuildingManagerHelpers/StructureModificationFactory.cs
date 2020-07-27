using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureModificationFactory
{
    private static StructureModificationHelper _singleStructurePlacementHelper;
    private static StructureModificationHelper _structureRemovalHelper;
    private static StructureModificationHelper _roadStructurePlacementHelper;
    private static StructureModificationHelper _zonePlacementHelper;
    private static StructureModificationHelper _strutureUpgradeHelper;

    public static void PrepareFactory(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager, IResourceManager resourceManager)
    {
        _singleStructurePlacementHelper = new SingleStructurePlacementHelper(structureRepository, grid, placementManager, resourceManager);
        _structureRemovalHelper = new StructureRemovalHelper(structureRepository, grid, placementManager, resourceManager);
        _strutureUpgradeHelper = new StructureUpgradeHelper(structureRepository, grid, placementManager, resourceManager);
        _roadStructurePlacementHelper = new RoadPlacementModificationHelper(structureRepository, grid, placementManager, resourceManager);
        _zonePlacementHelper = new ZonePlacementHelper(structureRepository, grid, placementManager, Vector3.zero, resourceManager);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerRemoveBuildingState))
        {
            return _structureRemovalHelper;
        }
        else if(classType == typeof(PlayerBuildingZoneState))
        {
            return _zonePlacementHelper;
        }
        else if(classType == typeof(PlayerBuildingRoadState))
        {
            return _roadStructurePlacementHelper;
        }
        else if(classType == typeof(PlayerUpgradeBuildingState))
        {
            return _strutureUpgradeHelper;
        }
        else
        {
            return _singleStructurePlacementHelper;
        }
    }
}
