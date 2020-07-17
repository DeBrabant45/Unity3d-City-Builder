using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StrucutreEconomyManager
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

}
