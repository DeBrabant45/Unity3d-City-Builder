using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureModificationFactory
{
    private static SingleStructurePlacementHelper _singleStructurePlacementHelper;
    private static StructureRemovalHelper _structureRemovalHelper;
    private static RoadPlacementModificationHelper _roadStructurePlacementHelper;

    public static void PrepareFactory(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager)
    {
        _singleStructurePlacementHelper = new SingleStructurePlacementHelper(structureRepository, grid, placementManager);
        _structureRemovalHelper = new StructureRemovalHelper(structureRepository, grid, placementManager);
        _roadStructurePlacementHelper = new RoadPlacementModificationHelper(structureRepository, grid, placementManager);
    }

    public static StructureModificationHelper GetHelper(Type classType)
    {
        if(classType == typeof(PlayerRemoveBuildingState))
        {
            return _structureRemovalHelper;
        }
        else if(classType == typeof(PlayerBuildingRoadState))
        {
            return _roadStructurePlacementHelper;
        }
        else
        {
            return _singleStructurePlacementHelper;
        }
    }
}
