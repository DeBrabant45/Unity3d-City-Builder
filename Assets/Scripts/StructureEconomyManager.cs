using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class StructureEconomyManager
{
    private static void PrepareNewStructure(Vector3Int gridPosition, GridStructure grid)
    {
        var structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        var strucutresAroundThisStructure = grid.GetStructuresDataInRange(gridPosition, structureData.structureRange);

        structureData.PrepareStructure(strucutresAroundThisStructure);
    }

    public static void PrepareZoneStructure(Vector3Int gridPosition, GridStructure grid)
    {
        PrepareNewStructure(gridPosition, grid);
        StructureBaseSO structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        AddFacilityTypeToStructure(gridPosition, grid, structureData);
    }

    public static void PrepareRoadStructure(Vector3Int gridPosition, GridStructure grid)
    {
        RoadStructureSO roadData = (RoadStructureSO)grid.GetStructureDataFromTheGrid(gridPosition);
        var structuresAroundRoad = grid.GetStructuresDataInRange(gridPosition, roadData.structureRange);
        roadData.PrepareRoad(structuresAroundRoad);
    }

    public static void PrepareFacilityStructure(Vector3Int gridPosition, GridStructure grid)
    {
        PrepareNewStructure(gridPosition, grid);
        SingleFacilitySO facilityData = (SingleFacilitySO)grid.GetStructureDataFromTheGrid(gridPosition);
        var structuresAroundFacility = grid.GetStructuresDataInRange(gridPosition, facilityData.singleStructureRange);
        facilityData.AddClient(structuresAroundFacility);
        AddFacilityTypeToStructure(gridPosition, grid, facilityData);
    }

    private static void PrepareManufactureStructure(Vector3Int gridPosition, GridStructure grid)
    {
        PrepareNewStructure(gridPosition, grid);
        StructureBaseSO structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        AddFacilityTypeToStructure(gridPosition, grid, structureData);
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

    private static void PrepareManufactureRemoval(Vector3Int gridPosition, GridStructure grid)
    {
        var structureData = grid.GetStructureDataFromTheGrid(gridPosition);
        structureData.PrepareForRemoval();
    }

    public static void PrepareZoneStructureForUpgrade(Vector3Int gridPosition, GridStructure grid, StructureBaseSO structureData)
    {
        structureData.PrepareForRemoval();
        PrepareZoneStructure(gridPosition, grid);
    }

    public static void PrepareFacilityStructureForUpgrade(Vector3Int gridPosition, GridStructure grid, StructureBaseSO structureData)
    {
        structureData.PrepareForRemoval();
        PrepareFacilityStructure(gridPosition, grid);
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
        else
        {
            PrepareManufactureStructure(gridPosition, grid);
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
        else
        {
            PrepareManufactureRemoval(gridPosition, grid);
        }
    }

    public static void CheckStructureTypeForUpgradePreparation(Type structureType, StructureBaseSO structureData, Vector3Int gridPosition, GridStructure grid)
    {
        if (structureType == typeof(ZoneStructureSO))
        {
            PrepareZoneStructureForUpgrade(gridPosition, grid, structureData);
        }
        else if (structureType == typeof(SingleFacilitySO))
        {
            PrepareFacilityStructureForUpgrade(gridPosition, grid, structureData);
        }
    }

    private static bool DoesStructureRequireAnyResources(StructureBaseSO structureData)
    {
        return (structureData.requirePower && structureData.HasPower() == false) 
            || (structureData.requireWater && structureData.HasWater() == false) 
            || (structureData.requireSilo && structureData.HasSilo() == false)
            || (structureData.requireHealthcare && structureData.HasHealthcare() == false)
            || (structureData.requireLawEnforcement && structureData.HasLawEnforcement() == false) 
            || (structureData.requireFireProtection && structureData.HasFireProtection() == false) 
            || (structureData.requirePostalService && structureData.HasPostalService() == false) 
            || (structureData.requireBankService && structureData.HasBankingService() == false) 
            || (structureData.requireGarbageService && structureData.HasGarbageService() == false); 
    }

    private static void AddFacilityTypeToStructure(Vector3Int gridPosition, GridStructure grid, StructureBaseSO structureData)
    {
        if (DoesStructureRequireAnyResources(structureData))
        {
            var structuresAroundPositions = grid.GetStructurePositionInRange(gridPosition, structureData.maxFacilitySearchRange);
            foreach (var structurePositionNearBy in structuresAroundPositions)
            {
                var data = grid.GetStructureDataFromTheGrid(structurePositionNearBy);
                if (data.GetType() == typeof(SingleFacilitySO))
                {
                    SingleFacilitySO facility = (SingleFacilitySO)data;
                    if ((facility.facilityType == FacilityType.Power && structureData.HasPower() == false && structureData.requirePower)
                        || (facility.facilityType == FacilityType.Water && structureData.HasWater() == false && structureData.requireWater)
                        || (facility.facilityType == FacilityType.Silo && structureData.HasSilo() == false && structureData.requireSilo)
                        || (facility.facilityType == FacilityType.Healthcare && structureData.HasHealthcare() == false && structureData.requireHealthcare)
                        || (facility.facilityType == FacilityType.LawEnforcement && structureData.HasLawEnforcement() == false && structureData.requireLawEnforcement)
                        || (facility.facilityType == FacilityType.FireProtection && structureData.HasFireProtection() == false && structureData.requireFireProtection)
                        || (facility.facilityType == FacilityType.Postal && structureData.HasPostalService() == false && structureData.requirePostalService)
                        || (facility.facilityType == FacilityType.Banking && structureData.HasBankingService() == false && structureData.requireBankService)
                        || (facility.facilityType == FacilityType.Garbage && structureData.HasGarbageService() == false && structureData.requireGarbageService))
                    {
                        if (grid.ArePositionsInRange(gridPosition, structurePositionNearBy, facility.singleStructureRange))
                        {
                            if (facility.IsFull() == false)
                            {
                                facility.AddClient(new StructureBaseSO[] { structureData });
                                if (DoesStructureRequireAnyResources(structureData) == false)
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

}
