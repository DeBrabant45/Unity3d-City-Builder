using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class PlacementMangerTests
    {
        private Material _materialTransparent;
        private PlacementManager _placementManager;
        private GameObject _testGameObject;
        private Vector3 _gridPosition1 = Vector3.zero;
        private Vector3 _gridPosition2 = new Vector3(3, 0, 3);

        [SetUp]
        public void Init()
        {
            GameObject ground = new GameObject();
            ground.transform.position = Vector3.zero;
            _testGameObject = TestHelpers.GetAGameObjectWithMaterial();
            _materialTransparent = new Material(Shader.Find("Standard"));

            _placementManager = Substitute.For<PlacementManager>();
            _placementManager.ground = ground.transform;
            _placementManager.transparentMaterial = _materialTransparent;
        }

        [UnityTest]
        public IEnumerator PlacementMangerTestsCreateGhostStructurePasses()
        {
            GameObject ghostObject = _placementManager.CreateGhostStructure(_gridPosition1, _testGameObject);
            yield return new WaitForEndOfFrame();
            foreach(var renderer in ghostObject.GetComponentsInChildren<MeshRenderer>())
            {
                Assert.AreEqual(renderer.material.color, Color.green);
            }
        }

        [UnityTest]
        public IEnumerator PlacementMangerPlaceStructureOnTheMapMaterialPasses()
        {
            GameObject ghostObject = _placementManager.CreateGhostStructure(_gridPosition1, _testGameObject);
            _placementManager.PlaceStructuresOnTheMap(new List<GameObject>() { ghostObject });
            yield return new WaitForEndOfFrame();
            foreach(var renderer in ghostObject.GetComponentsInChildren<MeshRenderer>())
            {
                Assert.AreEqual(renderer.material.color, Color.blue);
            }
        }

        [UnityTest]
        public IEnumerator PlacementMangerRemovalStructurePasses()
        {
            _placementManager.SetBuildingForRemoval(_testGameObject);
            yield return new WaitForEndOfFrame();
            foreach (var renderer in _testGameObject.GetComponentsInChildren<MeshRenderer>())
            {
                Assert.AreEqual(renderer.material.color, Color.red);
            }
        }
    }
}
