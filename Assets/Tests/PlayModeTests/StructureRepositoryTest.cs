using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StructureRepositoryTest
    {
        private StructureRepository _repository;

        [OneTimeSetUp]
        public void Init()
        {
            CollectionSO collection = ScriptableObject.CreateInstance<CollectionSO>();
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            ZoneStructureSO zone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            SingleFacilitySO facility = ScriptableObject.CreateInstance<SingleFacilitySO>();
            //SingleStructureBaseSO single = ScriptableObject.CreateInstance<SingleStructureBaseSO>();

            road.buildingName = "Road";
            zone.buildingName = "Commercial";
            facility.buildingName = "PowerPlant";

            collection.roadStructure = road;
            collection.singleStructures = new List<SingleStructureBaseSO>();
            collection.singleStructures.Add(facility);
            collection.zoneStructures.Add(zone);

            GameObject testObject = new GameObject();
            _repository = testObject.AddComponent<StructureRepository>();
            _repository.modelDataCollection = collection;
        }

        [UnityTest]
        public IEnumerator StructureRespositoryTestZonesQuantityPasses()
        {
            int quantity = _repository.GetZoneNames().Count;
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(1, quantity);
        }

        [UnityTest]
        public IEnumerator StructureRespositoryTestZonesNamePasses()
        {
            string name = _repository.GetZoneNames()[0];
            yield return new WaitForEndOfFrame();
            Assert.AreEqual("Commercial", name);
        }
    }
}
