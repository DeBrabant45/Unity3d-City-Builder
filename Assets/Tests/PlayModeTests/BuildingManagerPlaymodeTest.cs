using System;
using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class BuildingManagerPlaymodeTest
    {
        private BuildingManager _buildingManager;
        private Material _materialTransparent;

        [SetUp]
        public void InitBeforeEveryTests()
        {
            PlacementManager placementManager = Substitute.For<PlacementManager>();
            _materialTransparent = new Material(Shader.Find("Standard"));
            placementManager.transparentMaterial = _materialTransparent;
            GameObject ground = new GameObject();
            ground.transform.position = Vector3.zero;
            placementManager.ground = ground.transform;
            StructureRepository structureRepository = Substitute.For<StructureRepository>();
            CollectionSO collection = new CollectionSO();
            RoadStructureSO road = new RoadStructureSO();
            road.buildingName = "Road";
            GameObject roadChild = new GameObject("Road", typeof(MeshRenderer));
            roadChild.GetComponent<MeshRenderer>().material.color = Color.blue;
            GameObject roadPrefab = new GameObject("Road");
            roadChild.transform.SetParent(roadPrefab.transform);
            road.prefab = roadPrefab;
            collection.roadStructure = road;
            structureRepository.modelDataCollection = collection;
            _buildingManager = new BuildingManager(3, 10, 10, placementManager, structureRepository);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeRemovalConfirmationTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareDemolition(inputPosition);
            _buildingManager.PrepareBuildingManager(typeof(PlayerRemoveBuildingState));
            _buildingManager.ConfirmModification();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeRemovalNoConfirmationTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareDemolition(inputPosition);
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(_buildingManager.CheckForStructureInGrid(inputPosition));

        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeRemovalCancelTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareDemolition(inputPosition);
            _buildingManager.PrepareBuildingManager(typeof(PlayerRemoveBuildingState));
            _buildingManager.CancelModification();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(_buildingManager.CheckForStructureInGrid(inputPosition));


        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodePlacementCancelTests()
        {
            Vector3 inputPosition = PreparePlacement();
            _buildingManager.CancelModification();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));

        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodePlacementConfirmationPassTests()
        {
            Vector3 inputPosition = PreparePlacement();
            _buildingManager.ConfirmModification();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(_buildingManager.CheckForStructureInGrid(inputPosition));


        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodePlacementNoConfirmationTests()
        {
            Vector3 inputPosition = PreparePlacement();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));

        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangePlacementPrepareTests()
        {
            Vector3 inputPosition = PreparePlacement();
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInDictionary(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.green);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangePlacementConfirmTests()
        {
            Vector3 inputPosition = PreparePlacement();
            _buildingManager.ConfirmModification();
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInGrid(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.blue);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangeRemovalPrepareTests()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareDemolition(inputPosition);
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInDictionary(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.red);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangeRemovalCancelTests()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareDemolition(inputPosition);
            _buildingManager.PrepareBuildingManager(typeof(PlayerRemoveBuildingState));
            _buildingManager.CancelModification();
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInGrid(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.blue);
        }

        private Material AccessMaterial(Func<GameObject> accessMethod)
        {
            var roadObject = accessMethod();
            Material material = roadObject.GetComponentInChildren<MeshRenderer>().material;
            return material;
        }

        private Vector3 PreparePlacement()
        {
            Vector3 inputPosition = new Vector3(1, 0, 1);
            string structureName = "Road";
            _buildingManager.PrepareBuildingManager(typeof(PlayerBuildingRoadState));
            _buildingManager.PrepareStructureForModification(inputPosition, structureName, StructureType.Road);
            return inputPosition;
        }

        private void PrepareDemolition(Vector3 inputPosition)
        {
            _buildingManager.ConfirmModification();
            _buildingManager.PrepareBuildingManager(typeof(PlayerRemoveBuildingState));
            _buildingManager.PrepareStructureForRemovalAt(inputPosition);
        }
    }
}
