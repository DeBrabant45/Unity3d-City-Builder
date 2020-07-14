using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class PlayerStatusTest
    {
        private UIController _uIController;
        private GameManager _gameManagerComponent;

        [SetUp]
        public void Init()
        {
            GameObject gameManagerObject = new GameObject();
            var cameraMovementComponent = gameManagerObject.AddComponent<CameraMovement>();
            gameManagerObject.AddComponent<ResourceManagerTestStub>();

            _uIController = Substitute.For<UIController>();
            StructureRepository repository = Substitute.For<StructureRepository>();

            _gameManagerComponent = gameManagerObject.AddComponent<GameManager>();
            _gameManagerComponent.resourceManagerGameObject = gameManagerObject;
            _gameManagerComponent.cameraMovement = cameraMovementComponent;
            _gameManagerComponent.placementManagerGameObject = gameManagerObject;
            _gameManagerComponent.structureRepository = repository;
            _gameManagerComponent.uIController = _uIController;

        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerSelectionStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //awake
            yield return new WaitForEndOfFrame(); //start
            Assert.IsTrue(_gameManagerComponent.State is PlayerSelectionState);

        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingSingleStructureStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //awake
            yield return new WaitForEndOfFrame(); //start
            _gameManagerComponent.State.OnBuildSingleStructure(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(_gameManagerComponent.State is PlayerBuildingSingleStructureState);

        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingRoadStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //awake
            yield return new WaitForEndOfFrame(); //start
            _gameManagerComponent.State.OnBuildRoad(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(_gameManagerComponent.State is PlayerBuildingRoadState);

        }

        [UnityTest]
        public IEnumerator PlayerStatusPlayerBuildingZoneStateTestWithEnumeratorPasses()
        {
            // Use the Assert class to test conditions.
            // Use yield to skip a frame.
            yield return new WaitForEndOfFrame(); //awake
            yield return new WaitForEndOfFrame(); //start
            _gameManagerComponent.State.OnBuildZone(null);
            yield return new WaitForEndOfFrame();
            Assert.IsTrue(_gameManagerComponent.State is PlayerBuildingZoneState);

        }
    }
}
