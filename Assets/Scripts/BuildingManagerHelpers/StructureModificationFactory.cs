using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StructureModificationFactory
{
    private readonly SingleStructurePlacementHelper _singleStructurePlacementHelper;
    private readonly StructureRemovalHelper _structureRemovalHelper;
    private readonly RoadPlacementModificationHelper _roadStructurePlacementHelper;

    public StructureModificationFactory(StructureRepository structureRepository, GridStructure grid, IPlacementManager placementManager)
    {
        _singleStructurePlacementHelper = new SingleStructurePlacementHelper(structureRepository, grid, placementManager);
        _structureRemovalHelper = new StructureRemovalHelper(structureRepository, grid, placementManager);
        _roadStructurePlacementHelper = new RoadPlacementModificationHelper(structureRepository, grid, placementManager);
    }

    public StructureModificationHelper GetHelper(Type classType)
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
