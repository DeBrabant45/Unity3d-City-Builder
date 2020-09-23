using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class SingleStructureModificationHelperTests
    {
        private GameObject _tempObject = null;
        private GridStructure _grid;
        private StructureType _structureType = StructureType.Road;
        private string _structureName = "Road";
        private Vector3 _gridPosition1 = Vector3.zero;
        private Vector3 _gridPosition2 = new Vector3(3, 0, 3);
        private StructureModificationHelper _structureModificationHelper;

        [SetUp]
        public void Init()
        {
            StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingRoad();
            IPlacementManager placementManager = Substitute.For<IPlacementManager>();
            IResourceManager resourceManager = Substitute.For<IResourceManager>();
            resourceManager.CanIBuyIt(default, default, default).Returns(true);
            _tempObject = new GameObject();
            placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_tempObject);
            _grid = new GridStructure(3, 10, 10);
            _structureModificationHelper = new SingleStructurePlacementHelper(structureRepository, _grid, placementManager, resourceManager);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperAddPositionPasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperRemoveFromPositionPasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }
        
        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperAddToPositionTwoTimesPasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition2, _structureName, _structureType);
            GameObject objectInDictionary1 = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInDictionary2 = _structureModificationHelper.AccessStructureInDictionary(_gridPosition2);
            Assert.AreEqual(_tempObject, objectInDictionary1);
            Assert.AreEqual(_tempObject, objectInDictionary2);
        }       
        
        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperRemoveFromAllPositionPasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition2, _structureName, _structureType);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary1 = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInDictionary2 = _structureModificationHelper.AccessStructureInDictionary(_gridPosition2);
            Assert.IsNull(objectInDictionary1);
            Assert.IsNull(objectInDictionary2);
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void SingleStructureModificationHelperAddToGridPasses()
        {
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, _structureName, _structureType);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition2, _structureName, _structureType);
            _structureModificationHelper.ConfirmModifications();
            Assert.IsTrue(_grid.IsCellTaken(_gridPosition1));
            Assert.IsTrue(_grid.IsCellTaken(_gridPosition2));
        }
    }
}
