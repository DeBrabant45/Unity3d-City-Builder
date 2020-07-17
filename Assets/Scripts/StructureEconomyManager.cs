using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureEconomyManager
{
    private static void PrepareNewStructure(Vector3Int gridPosition, GridStructure grid)
    {
        var structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        var strucutresAaroundThisStructure = grid.GetStructuresDataInRange(gridPosition, structureData.structureRange);

        structureData.PrepareStructure(strucutresAaroundThisStructure);
    }

    public static void PrepareZoneStructure(Vector3Int gridPosition, GridStructure grid)
    {
        PrepareNewStructure(gridPosition, grid);
        ZoneStructureSO zoneData = (ZoneStructureSO)grid.GetStructureDataFromTheGrid(gridPosition);
        if(DoesStructureRequireAnyResources(zoneData))
        {
            var structuresAroundPositions = grid.GetStructurePositionInRange(gridPosition, zoneData.maxFacilitySearchRange);
            foreach (var structurePositionNearBy in structuresAroundPositions)
            {
                var data = grid.GetStructureDataFromTheGrid(structurePositionNearBy);
                if(data.GetType() == typeof(SingleFacilitySO))
                {
                    SingleFacilitySO facility = (SingleFacilitySO)data;
                    if((facility.facilityType == FacilityType.Power && zoneData.HasPower() == false && zoneData.requirePower)
                        || (facility.facilityType == FacilityType.Power && zoneData.HasPower() == false && zoneData.requirePower))
                    {
                        if(grid.ArePositionsInRange(gridPosition, structurePositionNearBy, facility.singleStructureRange))
                        {
                            if(facility.IsFull() == false)
                            {
                                facility.AddClient(new StructureBaseSO[] { zoneData });
                                if (DoesStructureRequireAnyResources(zoneData) == false)
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
            }
        }
    }

    private static bool DoesStructureRequireAnyResources(ZoneStructureSO zoneData)
    {
        return(zoneData.requirePower && zoneData.HasPower() == false) || (zoneData.requireWater && zoneData.HasWater() == false);
    }

    public static void PrepareRoadStructure(Vector3Int gridPosition, GridStructure grid)
    {
        RoadStructureSO roadData = (RoadStructureSO)grid.GetStructureDataFromTheGrid(gridPosition);
        var strucutresAaroundRoad = grid.GetStructuresDataInRange(gridPosition, roadData.structureRange);
        roadData.PrepareStructure(strucutresAaroundRoad);
    }

    public static void PrepareFacilityStructure(Vector3Int gridPosition, GridStructure grid)
    {
        PrepareNewStructure(gridPosition, grid);
        SingleFacilitySO facilityData = (SingleFacilitySO)grid.GetStructureDataFromTheGrid(gridPosition);
        var structuresAaroundFacility = grid.GetStructuresDataInRange(gridPosition, facilityData.singleStructureRange);
        facilityData.AddClient(structuresAaroundFacility);
    }

    public static IEnumerable<StructureBaseSO> PrepareFacilityRemoval(Vector3Int gridPosition, GridStructure grid)
    {
        SingleFacilitySO facilityData = (SingleFacilitySO)grid.GetStructureDataFromTheGrid(gridPosition);
        return facilityData.PrepareForRemoval();
    }

    public static IEnumerable<StructureBaseSO> PrepareRoadRemoval(Vector3Int gridPosition, GridStructure grid)
    {
        RoadStructureSO roadData = (RoadStructureSO)grid.GetStructureDataFromTheGrid(gridPosition);
        var structureAroundRoad = grid.GetStructuresDataInRange(gridPosition, roadData.structureRange);
        return roadData.PrepareRoadForRemoval(structureAroundRoad);
    }

    public static void PrepareStructureForRemoval(Vector3Int gridPosition, GridStructure grid)
    {
        var structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        structureData.PrepareForRemoval();
    }

    public static void CheckStructureTypeForCreationPreparation(Type structureType, Vector3Int gridPosition, GridStructure grid)
    {
        if (structureType == typeof(ZoneStructureSO))
        {
            PrepareZoneStructure(gridPosition, grid);
        }
        else if (structureType == typeof(RoadStructureSO))
        {
            PrepareRoadStructure(gridPosition, grid);
        }
        else if (structureType == typeof(SingleFacilitySO))
        {
            PrepareFacilityStructure(gridPosition, grid);
        }
    }

    public static void CheckStructureTypeForRemovalPreparation(Type structureType, Vector3Int gridPosition, GridStructure grid)
    {
        if (structureType == typeof(ZoneStructureSO))
        {
            PrepareStructureForRemoval(gridPosition, grid);
        }
        else if (structureType == typeof(RoadStructureSO))
        {
            PrepareRoadRemoval(gridPosition, grid);
        }
        else if (structureType == typeof(SingleFacilitySO))
        {
            PrepareFacilityRemoval(gridPosition, grid);
        }
    }

}
