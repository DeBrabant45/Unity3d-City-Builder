using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StructureRepositoryEditModeTests
    {
        StructureRepository structureRepo;
        GameObject testRoad;
        GameObject testSingleStructure;
        GameObject testZone;
        [OneTimeSetUp]
        public void Init()
        {
            structureRepo = Substitute.For<StructureRepository>();
            CollectionSO collection = ScriptableObject.CreateInstance<CollectionSO>();
            testRoad = new GameObject();
            testSingleStructure = new GameObject();
            testZone = new GameObject();
            var road = ScriptableObject.CreateInstance<RoadStructureSO>();
            road.buildingName = "Road";
            road.prefab = testRoad;
            var facility = ScriptableObject.CreateInstance<SingleFacilitySO>();
            facility.buildingName = "PowerPlant";
            facility.prefab = testSingleStructure;
            var zone = new ZoneStructureSO();
            zone.buildingName = "Commercial";
            zone.prefab = testZone;
            collection.roadStructure = road;
            collection.singleStructures = new List<SingleStructureBaseSO>();
            collection.singleStructures.Add(facility);
            collection.zoneStructures = new List<ZoneStructureSO>();
            collection.zoneStructures.Add(zone);
            structureRepo.modelDataCollection = collection;
        }
        // A Test behaves as an ordinary method
        [Test]
        public void StructureRepositoryEditModeGetRoadPrefabPasses()
        {
            GameObject returnObject = structureRepo.GetBuildingPrefabByName("Road", StructureType.Road);
            Assert.AreEqual(testRoad, returnObject);
            // Use the Assert class to test conditions
        }

        // A Test behaves as an ordinary method
        [Test]
        public void StructureRepositoryEditModeGetSingleStructurePrefabPasses()
        {
            GameObject returnObject = structureRepo.GetBuildingPrefabByName("PowerPlant", StructureType.SingleStructure);
            Assert.AreEqual(testSingleStructure, returnObject);
            // Use the Assert class to test conditions
        }

        // A Test behaves as an ordinary method
        [Test]
        public void StructureRepositoryEditModeGetSingleStructurePrefabNullPasses()
        {
            Assert.That(() => structureRepo.GetBuildingPrefabByName("PowerPlant2", StructureType.SingleStructure),
                Throws.Exception);
        }

        [Test]
        public void StructureRepositoryEditModeGetZonePrefabPasses()
        {
            GameObject returnObject = structureRepo.GetBuildingPrefabByName("Commercial", StructureType.Zone);
            Assert.AreEqual(testZone, returnObject);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void StructureRepositoryEditModeGetZonePrefabNullPasses()
        {
            Assert.That(() => structureRepo.GetBuildingPrefabByName("Commercial2", StructureType.Zone),
                Throws.Exception);
        }
    }
}
