using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    [TestFixture]
    public class GridStructureTests
    {
        GridStructure _grid;

        [SetUp]
        public void Init()
        {
            _grid = new GridStructure(3, 100, 100);
        }
        #region GridPositionTests
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateGridPositionPasses()
        {
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFloatsPasses()
        {
            Vector3 position = new Vector3(2.9f, 0, 2.9f);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFails()
        {
            Vector3 position = new Vector3(3.1f, 0, 0);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            //Assert
            Assert.AreNotEqual(Vector3.zero, returnPosition);
        }
        #endregion

        [Test]
        public void PlaceStructure303AndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _grid.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_grid.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureMINAndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _grid.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_grid.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureMAXAndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(297, 0, 0297);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _grid.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_grid.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructure303AndCheckIsTakenNullObjectFails()
        {
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            GameObject testGameObject = null;
            _grid.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsFalse(_grid.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFails()
        {
            Vector3 position = new Vector3(303, 0, 303);
            //Act
            Vector3 returnPosition = _grid.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _grid.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.Throws<IndexOutOfRangeException>(() => _grid.IsCellTaken(position));
        }

        [Test]
        public void GetAllPositionsFromTo()
        {
            Vector3Int startPosition = new Vector3Int(0, 0, 0);
            Vector3Int endPosition = new Vector3Int(6, 0, 3);

            var returnValues = _grid.GetAllPositionsFromTo(startPosition, endPosition);
            Assert.IsTrue(returnValues.Count == 6);
            Assert.IsTrue(returnValues.Contains(new Vector3Int(0, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(3, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(6, 0, 0)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(0, 0, 3)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(3, 0, 3)));
            Assert.IsTrue(returnValues.Contains(new Vector3Int(6, 0, 3)));

        }

        [Test]
        public void GetDataStrucutureTest()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(0, 0, 0), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(99, 0, 0), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(0, 0, 99), singleStructure);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(99, 0, 99), singleStructure);
            var list = _grid.GetAllStrucutures().ToList();
            Assert.IsTrue(list.Count == 4);
        }

        [Test]
        public void GetDataStrucutureInRange1Contains4Test()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 6), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 6), road);
            var list = _grid.GetStructuresDataInRange(new Vector3(6, 0, 6), 1).ToList();
            Assert.IsTrue(list.Count == 4);
        }

        [Test]
        public void GetDataStrucutureInRange1Contains2Test()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 3), singleStructure);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 9), singleStructure);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 9), road);
            var list = _grid.GetStructuresDataInRange(new Vector3(6, 0, 6), 1).ToList();
            Assert.IsTrue(list.Count == 2);
            Assert.IsTrue(list[0] == singleStructure);
            Assert.IsTrue(list[1] == singleStructure);
        }

        [Test]
        public void GetDataStrucutureInRange1Contains3Test()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 3), singleStructure);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 9), singleStructure);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 6), road);
            var list = _grid.GetStructuresDataInRange(new Vector3(6, 0, 6), 1).ToList();
            Assert.IsTrue(list.Count == 3);
            Assert.IsTrue(list.Contains(singleStructure));
            Assert.IsTrue(list.Contains(road));
        }

        [Test]
        public void GetDataStrucutureInRange1Contains0Test()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 9), road);
            var list = _grid.GetStructuresDataInRange(new Vector3(6, 0, 6), 1).ToList();
            Assert.IsTrue(list.Count == 0);
        }

        [Test]
        public void GetDataStructureInRange2Contains8Test()
        {
            RoadStructureSO road = ScriptableObject.CreateInstance<RoadStructureSO>();
            SingleStructureBaseSO singleStructure = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject gameObject = new GameObject();
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 3), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(6, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 9), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(3, 0, 6), road);
            _grid.PlaceStructureOnTheGrid(gameObject, new Vector3(9, 0, 6), road);
            var list = _grid.GetStructuresDataInRange(new Vector3(6, 0, 6), 2).ToList();
            Assert.IsTrue(list.Count == 8);
        }
    }
}
