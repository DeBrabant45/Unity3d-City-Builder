using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TestHelpers
{
    public static StructureRepository CreateStructureRepositoryContainingRoad()
    {
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        CollectionSO collection = new CollectionSO();
        RoadStructureSO road = new RoadStructureSO();
        road.buildingName = "Road";
        road.prefab = GetAGameObjectWithMaterial();
        collection.roadStructure = road;
        structureRepository.modelDataCollection = collection;

        return structureRepository;
    }

    public static StructureRepository CreateStructureRepositoryContainingFacility()
    {
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        CollectionSO collection = ScriptableObject.CreateInstance<CollectionSO>();
        SingleFacilitySO facility = ScriptableObject.CreateInstance<SingleFacilitySO>();
        facility.buildingName = "Water Tower";
        facility.prefab = GetAGameObjectWithMaterial();
        foreach(var item in collection.singleStructures)
        {
            if(facility.buildingName == item.buildingName)
            {
                facility = (SingleFacilitySO)item;
            }
        }
        //collection.singleStructures = facility;
        structureRepository.modelDataCollection = collection;

        return structureRepository;
    }

    //public static StructureRepository CreateStructureRepositoryContainingZoneStructure()
    //{
    //    StructureRepository structureRepository = Substitute.For<StructureRepository>();
    //    CollectionSO collection = new CollectionSO();
    //    ZoneStructureSO zone = new ZoneStructureSO();
    //    zone.buildingName = "Residential";
    //    zone.prefab = GetAGameObjectWithMaterial();
    //    collection.zoneStructures = zone;
    //    structureRepository.modelDataCollection.zoneStructures= collection;

    //    return structureRepository;
    //}

    public static GameObject GetAGameObjectWithMaterial()
    {
        GameObject roadChild = new GameObject("WaterPlant", typeof(MeshRenderer));
        var renderer = roadChild.GetComponent<MeshRenderer>();
        var materialBlue = Resources.Load("BlueMaterial", typeof(Material)) as Material;
        renderer.material = materialBlue;
        GameObject roadPrefab = new GameObject("WaterPlant");
        roadChild.transform.SetParent(roadPrefab.transform);

        return roadPrefab;
    }
}
