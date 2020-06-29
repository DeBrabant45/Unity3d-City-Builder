using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StructureRepository : MonoBehaviour
{
    public CollectionSO modelDataCollection;

    public List<string> GetZoneNames()
    {
        return modelDataCollection.zoneStructures.Select(zone => zone.buildingName).ToList();
    }

    public List<string> GetSingleStructureNames()
    {
        return modelDataCollection.singleStructures.Select(facility => facility.buildingName).ToList();
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
                structurePrefabToReturn = GetRaodBuildingPrefab();
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

    private GameObject GetZoneBuildingPrefabByName(string structureName)
    {
        var foundStructure = modelDataCollection.zoneStructures.Where(structure => structure.buildingName == structureName).FirstOrDefault();
        if(foundStructure != null)
        {
            return foundStructure.prefab;
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

    private GameObject GetRaodBuildingPrefab()
    {
        return modelDataCollection.roadStructure.prefab;
    }

}

public enum StructureType
{
    Zone,
    SingleStructure,
    Road
}
