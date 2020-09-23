using System.Collections;
using System.Collections.Generic;
using NSubstitute;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StructureRemovalHelperTest
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
                _tempObject = new GameObject();
                placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_tempObject);
                _grid = new GridStructure(3, 10, 10);

                _grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition1, null);
                _grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition2, null);

                IResourceManager resourceManager = Substitute.For<IResourceManager>();
                resourceManager.CanIBuyIt(default, default, default).Returns(true);

                _structureModificationHelper = new StructureRemovalHelper(structureRepository, _grid, placementManager, resourceManager);
            }

            // A Test behaves as an ordinary method
            [Test]
            public void SingleStructureModificationHelperSelectForRemovalPasses()
            {
                _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
                GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
                Assert.AreEqual(_tempObject, objectInDictionary);
            }

            // A Test behaves as an ordinary method
            [Test]
            public void SingleStructureModificationHelperCancelRemovalPasses()
            {
                _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
                _structureModificationHelper.CancelModifications();
                Assert.IsTrue(_grid.IsCellTaken(_gridPosition1));
            }            
            
            // A Test behaves as an ordinary method
            [Test]
            public void SingleStructureModificationHelperConfirmRemovalPasses()
            {
                _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
                GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
                _structureModificationHelper.ConfirmModifications();
                Assert.IsFalse(_grid.IsCellTaken(_gridPosition1));
            }
        }
    }
}
