using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class RoadManagerTest
    {
        private GridStructure _grid;
        private GameObject _roadStraight = new GameObject();
        private GameObject _roadCorner = new GameObject();
        private RoadStructureSO _roadSO = ScriptableObject.CreateInstance<RoadStructureSO>();
        private GameObject _road3Way = new GameObject();
        private GameObject _road4Way = new GameObject();

        [OneTimeSetUp]
        public void Init()
        {
            _grid = new GridStructure(3, 10, 10);
            _roadSO.prefab = _roadStraight;
            _roadSO.cornerPrefab = _roadCorner;
            _roadSO.threeWayPrefab = _road3Way;
            _roadSO.fourWayPrefab = _road4Way;

            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(3, 0, 6), _roadSO);
            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(6, 0, 3), _roadSO);
            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(9, 0, 6), _roadSO);
            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(6, 0, 9), _roadSO);

            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(12, 0, 9), _roadSO);
            _grid.PlaceStructureOnTheGrid(_roadStraight, new Vector3(15, 0, 6), _roadSO);
        }


        [Test]
        public void RoadManagerTestsGetStatusWithoutNeighbor()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(27, 0, 27), _grid, null);
            Assert.AreEqual(0, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsOnTheRight()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(0, 0, 6), _grid, null);
            Assert.AreEqual((int)Direction.Right, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsOnTheLeft()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(18, 0, 6), _grid, null);
            Assert.AreEqual((int)Direction.Left, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsUp()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(6, 0, 0), _grid, null);
            Assert.AreEqual((int)Direction.Up, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsDown()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(12, 0, 12), _grid, null);
            Assert.AreEqual((int)Direction.Down, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsDownRight()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(3, 0, 9), _grid, null);
            Assert.AreEqual((int)Direction.Down | (int)Direction.Right, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsDownLeft()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(15, 0, 9), _grid, null);
            Assert.AreEqual((int)Direction.Down | (int)Direction.Left, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsUpRight()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(3, 0, 3), _grid, null);
            Assert.AreEqual((int)Direction.Up | (int)Direction.Right, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighborIsUpLeft()
        {
            var results = RoadManager.GetRoadNeighborStatus(new Vector3(9, 0, 3), _grid, null);
            Assert.AreEqual((int)Direction.Up | (int)Direction.Left, results);
        }

        [Test]
        public void RoadManagerTestsGetStatusNeighbourDownLeftRight()
        {
            var result = RoadManager.GetRoadNeighborStatus(new Vector3(9, 0, 9), _grid, null);
            Assert.AreEqual((int)Direction.Down | (int)Direction.Left | (int)Direction.Right, result);
        }
        [Test]
        public void RoadManagerTestsGetStatusNeighbourDownUpLeftRight()
        {
            var result = RoadManager.GetRoadNeighborStatus(new Vector3(6, 0, 6), _grid, null);
            Assert.AreEqual((int)Direction.Up | (int)Direction.Down | (int)Direction.Left | (int)Direction.Right, result);
        }

        [Test]
        public void RoadManagerTestsGetNeighboursAll4()
        {
            var result = RoadManager.GetRoadNeighborsPosition(_grid, new Vector3Int(6, 0, 6));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(3, 0, 6)));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(9, 0, 6)));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(6, 0, 3)));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(6, 0, 9)));
        }

        [Test]
        public void RoadManagerTestsGetNeighboursRightLeftDown()
        {
            var result = RoadManager.GetRoadNeighborsPosition(_grid, new Vector3Int(9, 0, 9));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(6, 0, 9)));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(9, 0, 6)));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(12, 0, 9)));
        }

        [Test]
        public void RoadManagerTestsGetNeighboursRight()
        {
            var result = RoadManager.GetRoadNeighborsPosition(_grid, new Vector3Int(0, 0, 6));
            Assert.IsTrue(result.ContainsKey(new Vector3Int(3, 0, 6)));
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryTrue()
        {
            var dictionary = new Dictionary<Vector3Int, GameObject>();
            var position = new Vector3Int(3, 0, 6);
            dictionary.Add(position, _roadStraight);
            var result = RoadManager.CheckIfNeighborHasRoadWithinDictionary(position, dictionary);
            Assert.IsTrue(result);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryFalse()
        {
            var dictionary = new Dictionary<Vector3Int, GameObject>();
            var position = new Vector3Int(3, 0, 6);
            dictionary.Add(position, _roadStraight);
            var result = RoadManager.CheckIfNeighborHasRoadWithinDictionary(new Vector3Int(6, 0, 6), dictionary);
            Assert.IsFalse(result);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadOnGridTrue()
        {
            var position = new Vector3Int(3, 0, 6);
            var result = RoadManager.CheckIfNeighborHasRoadOnTheGrid(_grid, position);
            Assert.IsTrue(result);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadOnGridFalse()
        {
            var position = new Vector3Int(0, 0, 0);
            var result = RoadManager.CheckIfNeighborHasRoadOnTheGrid(_grid, position);
            Assert.IsFalse(result);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightFits0()
        {
            var result = RoadManager.CheckIfStraightRoadFits(0, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightFitsDownR90()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Down, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R90, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightFitsUpR90()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Up, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R90, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightFitsUpDownR90()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Up | (int)Direction.Down, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R90, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightRightFitsR0()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Right, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightLeftFitsR0()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Left, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfStraightRightLeftFitsR0()
        {
            var result = RoadManager.CheckIfStraightRoadFits((int)Direction.Left | (int)Direction.Right, null, _roadSO);
            Assert.AreEqual(_roadStraight, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfCornerFitsR0()
        {
            var result = RoadManager.CheckIfCornerRoadFits((int)Direction.Up | (int)Direction.Right, null, _roadSO);
            Assert.AreEqual(_roadCorner, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfCornerFitsR90()
        {
            var result = RoadManager.CheckIfCornerRoadFits((int)Direction.Down | (int)Direction.Right, null, _roadSO);
            Assert.AreEqual(_roadCorner, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R90, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfCornerFitsR180()
        {
            var result = RoadManager.CheckIfCornerRoadFits((int)Direction.Down | (int)Direction.Left, null, _roadSO);
            Assert.AreEqual(_roadCorner, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R180, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfCornerFitsR270()
        {
            var result = RoadManager.CheckIfCornerRoadFits((int)Direction.Up | (int)Direction.Left, null, _roadSO);
            Assert.AreEqual(_roadCorner, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R270, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfThreeWayFitsR0()
        {
            var result = RoadManager.CheckIfThreewayRoadFits((int)Direction.Up | (int)Direction.Right | (int)Direction.Down, null, _roadSO);
            Assert.AreEqual(_road3Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfThreeWayFitsR90()
        {
            var result = RoadManager.CheckIfThreewayRoadFits((int)Direction.Right | (int)Direction.Down | (int)Direction.Left, null, _roadSO);
            Assert.AreEqual(_road3Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R90, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfThreeWayFitsR180()
        {
            var result = RoadManager.CheckIfThreewayRoadFits((int)Direction.Down | (int)Direction.Left | (int)Direction.Up, null, _roadSO);
            Assert.AreEqual(_road3Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R180, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfThreeWayFitsR270()
        {
            var result = RoadManager.CheckIfThreewayRoadFits((int)Direction.Up | (int)Direction.Left | (int)Direction.Right, null, _roadSO);
            Assert.AreEqual(_road3Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R270, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsCheckIfNeighbourIsRoadInDictionaryCheckIfFourWayFitsR0()
        {
            var result = RoadManager.CheckIfFourwayRoadFits((int)Direction.Up | (int)Direction.Left | (int)Direction.Right | (int)Direction.Down, null, _roadSO);
            Assert.AreEqual(_road4Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }

        [Test]
        public void RoadManagerTestsGetCorrectRoadPrefab4WayGrid()
        {
            var result = RoadManager.GetCorrectRoadPrefab(new Vector3Int(6, 0, 6), _roadSO, null, _grid);
            Assert.AreEqual(_road4Way, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }
        [Test]
        public void RoadManagerTestsGetCorrectRoadPrefabCornerDictionary()
        {
            var dictionary = new Dictionary<Vector3Int, GameObject>();
            dictionary.Add(new Vector3Int(3, 0, 0), _roadStraight);
            dictionary.Add(new Vector3Int(0, 0, 3), _roadStraight);
            var result = RoadManager.GetCorrectRoadPrefab(new Vector3Int(0, 0, 0), _roadSO, dictionary, _grid);
            Assert.AreEqual(_roadCorner, result.RoadPrefab);
            Assert.AreEqual(RotationValue.R0, result.RoadPrefabRotation);
        }

    }
}
