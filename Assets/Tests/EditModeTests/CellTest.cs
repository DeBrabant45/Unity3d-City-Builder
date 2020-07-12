using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class CellTest
    {
        [Test]
        public void CellSetGameObjectPass()
        {
            Cell cell = new Cell();
            cell.SetConstruction(new GameObject(), null);
            Assert.IsTrue(cell.IsTaken);
        }

        [Test]
        public void CellSetGameObjectNullFail()
        {
            Cell cell = new Cell();
            cell.SetConstruction(null, null);
            Assert.IsFalse(cell.IsTaken);
        }

        [Test]
        public void CellSetGameOjectRemovePasses()
        {
            Cell cell = new Cell();
            cell.SetConstruction(new GameObject(), null);
            cell.RemoveStructure();
            Assert.IsFalse(cell.IsTaken);
        }

        [Test]
        public void CellGetStructureData()
        {
            Cell cell = new Cell();
            RoadStructureSO roadSO = ScriptableObject.CreateInstance<RoadStructureSO>();
            cell.SetConstruction(new GameObject(), roadSO);
            Assert.AreEqual(roadSO, cell.GetStructureData());
        }
    }
}
