using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class ZoneCalculatorTests
    {
        [Test]
        public void ZoneCalculatorTestCalculateZoneNoPreviousPlacement()
        {
            HashSet<Vector3Int> newPositionsList = new HashSet<Vector3Int>();
            Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
            GameObject structure = new GameObject();
            Queue<GameObject> gameObjectsToReuse = new Queue<GameObject>();

            newPositionsList.Add(new Vector3Int(0, 0, 0));
            newPositionsList.Add(new Vector3Int(3, 0, 0));
            newPositionsList.Add(new Vector3Int(6, 0, 0));


            ZoneCalculator.CalculateZone(newPositionsList, structuresToBeModified, gameObjectsToReuse);
            Assert.IsTrue(structuresToBeModified.Count == 0);
            Assert.IsTrue(gameObjectsToReuse.Count == 0);
            Assert.IsTrue(structure.activeSelf == true);
            Assert.IsTrue(newPositionsList.Count == 3);

        }

        [Test]
        public void ZoneCalculatorTestCalculateZoneLargerThanPreviousPlacement()
        {
            HashSet<Vector3Int> newPositionsList = new HashSet<Vector3Int>();
            Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
            GameObject structure = new GameObject();
            Queue<GameObject> gameObjectsToReuse = new Queue<GameObject>();

            newPositionsList.Add(new Vector3Int(0, 0, 0));
            newPositionsList.Add(new Vector3Int(3, 0, 0));
            newPositionsList.Add(new Vector3Int(6, 0, 0));


            structuresToBeModified.Add(Vector3Int.zero, structure);
            structuresToBeModified.Add(new Vector3Int(12, 0, 0), structure);

            ZoneCalculator.CalculateZone(newPositionsList, structuresToBeModified, gameObjectsToReuse);
            Assert.IsTrue(structuresToBeModified.Count == 1);
            Assert.IsTrue(structuresToBeModified.ContainsKey(Vector3Int.zero));
            Assert.IsTrue(gameObjectsToReuse.Count == 1);
            Assert.IsTrue(structure.activeSelf == false);
            Assert.IsTrue(newPositionsList.Count == 2);
            Assert.IsTrue(newPositionsList.Contains(Vector3Int.zero) == false);

        }

        [Test]
        public void ZoneCalculatorTestCalculateZoneSmallerhanPreviousPlacement()
        {
            HashSet<Vector3Int> newPositionsList = new HashSet<Vector3Int>();
            Dictionary<Vector3Int, GameObject> structuresToBeModified = new Dictionary<Vector3Int, GameObject>();
            GameObject structure = new GameObject();
            Queue<GameObject> gameObjectsToReuse = new Queue<GameObject>();

            newPositionsList.Add(new Vector3Int(0, 0, 0));
            newPositionsList.Add(new Vector3Int(3, 0, 0));
            newPositionsList.Add(new Vector3Int(6, 0, 0));


            structuresToBeModified.Add(Vector3Int.zero, structure);
            structuresToBeModified.Add(new Vector3Int(3, 0, 0), structure);
            structuresToBeModified.Add(new Vector3Int(6, 0, 0), structure);
            structuresToBeModified.Add(new Vector3Int(9, 0, 0), structure);

            ZoneCalculator.CalculateZone(newPositionsList, structuresToBeModified, gameObjectsToReuse);
            Assert.IsTrue(structuresToBeModified.Count == 3);
            Assert.IsTrue(structuresToBeModified.ContainsKey(new Vector3Int(3, 0, 0)));
            Assert.IsTrue(structuresToBeModified.ContainsKey(new Vector3Int(6, 0, 0)));
            Assert.IsTrue(gameObjectsToReuse.Count == 1);
            Assert.IsTrue(structure.activeSelf == false);
            Assert.IsTrue(newPositionsList.Count == 0);

        }

        [Test]
        public void ZoneCalculatorTestPrepareStartAndEndPositionBOttomLeftTopRight()
        {
            Vector3 startPosition = Vector3.zero;
            Vector3 endPosition = new Vector3(3, 0, 3);
            Vector3Int minPoint = Vector3Int.FloorToInt(startPosition);
            Vector3Int maxPoint = Vector3Int.FloorToInt(endPosition);

            ZoneCalculator.PrepareStartAndEndPoints(startPosition, endPosition, ref minPoint, ref maxPoint, Vector3.zero);
            Assert.AreEqual(Vector3Int.FloorToInt(startPosition), minPoint);
            Assert.AreEqual(Vector3Int.FloorToInt(endPosition), maxPoint);
        }
        [Test]
        public void ZoneCalculatorTestPrepareStartAndEndPositionTopRightBOttomLeft()
        {
            Vector3 startPosition = new Vector3(3, 0, 3);
            Vector3 endPosition = Vector3.zero;
            Vector3Int minPoint = Vector3Int.FloorToInt(startPosition);
            Vector3Int maxPoint = Vector3Int.FloorToInt(endPosition);

            ZoneCalculator.PrepareStartAndEndPoints(startPosition, endPosition, ref minPoint, ref maxPoint, Vector3.zero);
            Assert.AreEqual(Vector3Int.FloorToInt(startPosition), maxPoint);
            Assert.AreEqual(Vector3Int.FloorToInt(endPosition), minPoint);
        }
        [Test]
        public void ZoneCalculatorTestPrepareStartAndEndPositionTopRightBottomLeft()
        {
            Vector3 startPosition = new Vector3(0, 0, 3);
            Vector3 endPosition = new Vector3(3, 0, 0);
            Vector3Int minPoint = Vector3Int.FloorToInt(startPosition);
            Vector3Int maxPoint = Vector3Int.FloorToInt(endPosition);

            ZoneCalculator.PrepareStartAndEndPoints(startPosition, endPosition, ref minPoint, ref maxPoint, Vector3.zero);
            Assert.AreEqual(new Vector3Int((int)startPosition.x, 0, (int)endPosition.z), minPoint);
            Assert.AreEqual(new Vector3Int((int)endPosition.x, 0, (int)startPosition.z), maxPoint);
        }

        [Test]
        public void ZoneCalculatorTestPrepareStartAndEndPositionBottomLeftTopRight()
        {
            Vector3 startPosition = new Vector3(3, 0, 0);
            Vector3 endPosition = new Vector3(0, 0, 3);
            Vector3Int minPoint = Vector3Int.FloorToInt(startPosition);
            Vector3Int maxPoint = Vector3Int.FloorToInt(endPosition);

            ZoneCalculator.PrepareStartAndEndPoints(startPosition, endPosition, ref minPoint, ref maxPoint, Vector3.zero);
            Assert.AreEqual(new Vector3Int((int)startPosition.x, 0, (int)endPosition.z), maxPoint);
            Assert.AreEqual(new Vector3Int((int)endPosition.x, 0, (int)startPosition.z), minPoint);
        }

        [Test]
        public void ZoneCalculatorTestCheckIfpositionHasChangedFalse()
        {
            GridStructure grid = new GridStructure(3, 10, 10);
            Vector3 gridPosition = new Vector3(3, 0, 3);
            Vector3 previousPosition = new Vector3(4, 0, 4);
            Assert.IsFalse(ZoneCalculator.CheckIfPositionHasChanged(gridPosition, previousPosition, grid));
        }

        [Test]
        public void ZoneCalculatorTestCheckIfpositionHasChangedTrue()
        {
            GridStructure grid = new GridStructure(3, 10, 10);
            Vector3 gridPosition = new Vector3(3, 0, 3);
            Vector3 previousPosition = new Vector3(2, 0, 2);
            Assert.IsTrue(ZoneCalculator.CheckIfPositionHasChanged(gridPosition, previousPosition, grid));
        }
    }
}
