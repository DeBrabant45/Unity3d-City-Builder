using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class GridStructureTests
    {
        GridStructure _structure;
        [OneTimeSetUp]
        public void Init()
        {
            _structure = new GridStructure(3, 100, 100);
        }
        #region GridPositionTests
        // A Test behaves as an ordinary method
        [Test]
        public void CalculateGridPositionPasses()
        {
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFloatsPasses()
        {
            Vector3 position = new Vector3(2.9f, 0, 2.9f);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            //Assert
            Assert.AreEqual(Vector3.zero, returnPosition);
        }

        [Test]
        public void CalculateGridPositionFails()
        {
            Vector3 position = new Vector3(3.1f, 0, 0);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            //Assert
            Assert.AreNotEqual(Vector3.zero, returnPosition);
        }
        #endregion
        
        [Test]
        public void PlaceStructure303AndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _structure.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_structure.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureMINAndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(0, 0, 0);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _structure.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_structure.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureMAXAndCheckIsTakenPasses()
        {
            Vector3 position = new Vector3(297, 0, 0297);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject("TestGameObject");
            _structure.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsTrue(_structure.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructure303AndCheckIsTakenNullObjectFails()
        {
            Vector3 position = new Vector3(3, 0, 3);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            GameObject testGameObject = null;
           _structure.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.IsFalse(_structure.IsCellTaken(position));
        }

        [Test]
        public void PlaceStructureAndCheckIsTakenIndexOutOfBoundsFails()
        {
            Vector3 position = new Vector3(303, 0, 303);
            //Act
            Vector3 returnPosition = _structure.CalculateGridPosition(position);
            GameObject testGameObject = new GameObject ("TestGameObject");
           _structure.PlaceStructureOnTheGrid(testGameObject, position, null);
            //Assert
            Assert.Throws<IndexOutOfRangeException>(()=> _structure.IsCellTaken(position));
        }
    }
}
