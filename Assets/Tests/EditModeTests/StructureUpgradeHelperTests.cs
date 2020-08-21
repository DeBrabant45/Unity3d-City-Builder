using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class StructureUpgradeHelperTests
    {
        private GameObject _tempObject = null;
        private GridStructure _grid;
        private StructureType _structureType = StructureType.SingleStructure;
        private string _structureName = "Water Tower";
        private Vector3 _gridPosition1 = Vector3.zero;
        private Vector3 _gridPosition2 = new Vector3(3, 0, 3);
        private StructureModificationHelper _structureModificationHelper;
        private StructureModificationHelper _structureModificationHelper1;

        [SetUp]
        public void Init()
        {
            StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingRoad();
            IPlacementManager placementManager = Substitute.For<IPlacementManager>();
            _tempObject = new GameObject();
            placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_tempObject);
            _grid = new GridStructure(3, 10, 10);

            _grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition2, null);
            //_grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition2, null);

            IResourceManager resourceManager = Substitute.For<IResourceManager>();
            resourceManager.CanIBuyIt(default).Returns(true);

            _structureModificationHelper = new SingleStructurePlacementHelper(structureRepository, _grid, placementManager, resourceManager);
            _structureModificationHelper1 = new StructureUpgradeHelper(structureRepository, _grid, placementManager, resourceManager);

        }

        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperSelectForUpgradePasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            _structureModificationHelper1.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }
    }
}
