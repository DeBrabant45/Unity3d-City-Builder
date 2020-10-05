using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StructureRepository : MonoBehaviour
{
    public CollectionSO modelDataCollection;

    public List<StructureBaseSO> GetResidentialInfo()
    {
        return modelDataCollection.residentialStructures;
    }

    public List<StructureBaseSO> GetCommercialStoreInfo()
    {
        return modelDataCollection.commercialStoreStructures;
    }    
    
    public List<StructureBaseSO> GetCommercialBusinessInfo()
    {
        return modelDataCollection.commercialBusinessStructures;
    }

    public List<StructureBaseSO> GetAgricultureInfo()
    {
        return modelDataCollection.agricultureStructures;
    }

    public List<StructureBaseSO> GetRoadInfo()
    {
        return modelDataCollection.roadStructures;
    }

    public List<StructureBaseSO> GetEmergencyInfo()
    {
        return modelDataCollection.emergencyStructures;
    }

    public List<StructureBaseSO> GetUtilitiesInfo()
    {
        return modelDataCollection.utilitiesStructures;
    }

    public List<StructureBaseSO> GetManufactureInfo()
    {
        return modelDataCollection.manufactureStructures;
    }

    public List<StructureBaseSO> GetGovernmentStructureInfo()
    {
        return modelDataCollection.governmentStructures;
    }

    public List<string> GetZoneNames()
    {
        return modelDataCollection.zoneStructures.Select(zone => zone.buildingName).ToList();
    }

    public List<string> GetSingleStructureNames()
    {
        return modelDataCollection.singleStructures.Select(facility => facility.buildingName).ToList();
    }

    public List<string> GetManufacturerNames()
    {
        return modelDataCollection.manufacturers.Select(factory => factory.buildingName).ToList();
    }

    public string GetRoadStructureName()
    {
        return modelDataCollection.roadStructure.buildingName;
    }

    public GameObject GetBuildingPrefabByName(string structureName, StructureType structureType)
    {
        GameObject structurePrefabToReturn = null;
        switch(structureType)
        {
            case StructureType.Zone:
                structurePrefabToReturn = GetZoneBuildingPrefabByName(structureName);
                break;
            case StructureType.SingleStructure:
                structurePrefabToReturn = GetSingleStructureBuildingPrefabByName(structureName);
                break;
            case StructureType.Road:
                structurePrefabToReturn = GetRoadBuildingPrefab();
                break;
            default:
                throw new System.Exception("No such type" + structureType);
        }

        if(structurePrefabToReturn == null)
        {
            throw new Exception("No prefab for that name " + structureName);
        }

        return structurePrefabToReturn;
    }

    //public int GetStructureUpgradeIncome(StructureBaseSO structureData)
    //{
    //    int upgradeAmountToReturn = 0;
    //    Type structureDataType = structureData.GetType();
    //    if (structureDataType == typeof(ZoneStructureSO))
    //    {
    //        upgradeAmountToReturn = ((ZoneStructureSO)structureData).upgradedIncome;
    //    }

    //    return upgradeAmountToReturn;
    //}

    private GameObject GetZoneBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.zoneStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if(foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    public StructureBaseSO GetStructureData(string structureName, StructureType structureType)
    {
        switch (structureType)
        {
            case StructureType.Manufacturer:
                return modelDataCollection.manufacturers.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.Zone:
                return modelDataCollection.zoneStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.SingleStructure:
                return modelDataCollection.singleStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
            case StructureType.Road:
                return modelDataCollection.roadStructure;
            case StructureType.None:
                return null;
        }

        return null;
    }

    private GameObject GetSingleStructureBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.singleStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if(foundStructure != null)
        {
            return foundStructure.prefab;
        }
        return null;
    }

    private GameObject GetRoadBuildingPrefab()
    {
        return modelDataCollection.roadStructure.prefab;
    }

    public GameObject SetRandomResidentaialPrefab()
    {
        var zones = modelDataCollection.zoneStructures;
        var residentialPrefabVariantsCount = zones.Select(zone => zone.prefabVariants).Count();
        var residentialPrefabVariantsRandomRange = UnityEngine.Random.Range(0, residentialPrefabVariantsCount);
        return zones.Select(zone => zone.prefabVariants[residentialPrefabVariantsRandomRange]).FirstOrDefault();
    }
}

public enum StructureType
{
    Zone,
    SingleStructure,
    Manufacturer,
    Road,
    None
}
