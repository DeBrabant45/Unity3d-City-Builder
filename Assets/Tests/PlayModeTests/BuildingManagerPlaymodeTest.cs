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
        public IEnumerator BuildingManagerPlayModeRemovalConfirmationTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            _buildingManager.ConfirmRemoval();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeDemolishConfirmationTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            _buildingManager.ConfirmRemoval();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));
        }

        

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeRemovalNoConfirmationTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(_buildingManager.CheckForStructureInGrid(inputPosition));

        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeRemovalCancelTest()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            _buildingManager.CancelRemoval();
            yield return new WaitForEndOfFrame();
            Assert.IsNotNull(_buildingManager.CheckForStructureInGrid(inputPosition));
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodePlacementCancelTests()
        {
            Vector3 inputPosition = PreparePlacement();
            _buildingManager.CancelRemoval();
            yield return new WaitForEndOfFrame();
            Assert.IsNull(_buildingManager.CheckForStructureInGrid(inputPosition));

        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodePlacementConfirmationPassTests()
        {
            Vector3 inputPosition = PreparePlacement();
            _buildingManager.ConfirmPlacement();
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
            _buildingManager.ConfirmPlacement();
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInGrid(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.blue);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangeRemovalPrepareTests()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            Material material = AccessMaterial(() => _buildingManager.CheckForStructureInDictionary(inputPosition));
            yield return new WaitForEndOfFrame();
            Assert.AreEqual(material.color, Color.red);
        }

        [UnityTest]
        public IEnumerator BuildingManagerPlaymodeMaterialChangeRemovalCancelTests()
        {
            Vector3 inputPosition = PreparePlacement();
            PrepareRemoval(inputPosition);
            _buildingManager.CancelRemoval();
            Material material = AccessMaterial(()=> _buildingManager.CheckForStructureInGrid(inputPosition));
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
            _buildingManager.PrepareStructureForPlacement(inputPosition, structureName, StructureType.Road);
            return inputPosition;
        }

        private void PrepareRemoval(Vector3 inputPosition)
        {
            _buildingManager.ConfirmRemoval();
            _buildingManager.PrepareStructureForRemovalAt(inputPosition);
        }
    }
}
