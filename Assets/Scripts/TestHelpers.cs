using NSubstitute;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        collection.singleStructures = new List<SingleStructureBaseSO>();
        collection.singleStructures.Add(facility);
        structureRepository.modelDataCollection = collection;

        return structureRepository;
    }

    public static StructureRepository CreateStructureRepositoryContainingZoneStructure()
    {
        StructureRepository structureRepository = Substitute.For<StructureRepository>();
        CollectionSO collection = new CollectionSO();
        ZoneStructureSO zone = new ZoneStructureSO();
        GameObject TestPrefab = new GameObject();
        GameObject TestPrefab2 = new GameObject();
        zone.buildingName = "Commercial";
        zone.prefab = GetAGameObjectWithMaterial();
        zone.upgradable = true;
        zone.upgradeActive = false;
        zone.prefab = TestPrefab;
        zone.upgradePrefab = TestPrefab2;
        zone.upgradePrefabVariants = new GameObject[1];
        collection.zoneStructures = new List<ZoneStructureSO>();
        collection.zoneStructures.Add(zone);
        structureRepository.modelDataCollection = collection;

        return structureRepository;
    }

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
