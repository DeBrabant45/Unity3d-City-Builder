using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests
{
    public class StructureEconomyManagerTest
    {
        private GridStructure _grid;
        private GameObject _structureObject = new GameObject();

        [SetUp]
        public void Init()
        {
            _grid = new GridStructure(3, 10, 10);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void StructureManagerEconomyTestsAddReidentialStructureNoRoad()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void StructureManagerEconomyTestsAddReidentialStructureNearRoadConnections()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 0));
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.True(residentialZone.HasRoadAccess());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void StructureManagerEconomyTestsAddReidentialStructureRoadDiagonalNoConnections()
        {
            CreateRoadAtPosition(new Vector3Int(3, 0, 0));
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }
                
        // A Test behaves as an ordinary method
        [Test]
        public void StructureManagerEconomyTestsAddRoadNearReidentialStructureRoadDiagonalNoConnections()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            Assert.False(residentialZone.HasRoadAccess());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void StructureManagerEconomyTestsRemoveRoadNearReidentialStructureRoadDiagonalConnections()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            RoadStructureSO roadStructure = ScriptableObject.CreateInstance<RoadStructureSO>();
            CreateRoadAtPosition(new Vector3Int(3, 0, 3));
            StructureEconomyManager.PrepareRoadRemoval(new Vector3Int(3, 0, 0), _grid);
            _grid.RemoveStructureFromTheGrid(new Vector3(3, 0, 0));
            Assert.False(residentialZone.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddRoadNear3ReidentialStructureConnectionWith1()
        {
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZOneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZOneAtPosition(new Vector3Int(0, 0, 6));
            CreateRoadATPosition(new Vector3Int(3, 0, 3));

            Assert.False(residentialZone.HasRoadAccess());
            Assert.True(residentialZone1.HasRoadAccess());
            Assert.False(residentialZone2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAdd3ReidentialStructureNearRoadConnectionWith1()
        {
            CreateRoadATPosition(new Vector3Int(3, 0, 3));
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZOneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZOneAtPosition(new Vector3Int(0, 0, 6));


            Assert.False(residentialZone.HasRoadAccess());
            Assert.True(residentialZone1.HasRoadAccess());
            Assert.False(residentialZone2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAdd3FacilityStructureNearRoadConnectionWith1()
        {
            CreateRoadATPosition(new Vector3Int(3, 0, 3));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(0, 0, 0));
            SingleFacilitySO facility1 = CreateFacilityAtPosition(new Vector3Int(0, 0, 3));
            SingleFacilitySO facility2 = CreateFacilityAtPosition(new Vector3Int(0, 0, 6));


            Assert.False(facility.HasRoadAccess());
            Assert.True(facility1.HasRoadAccess());
            Assert.False(facility2.HasRoadAccess());
        }

        [Test]
        public void StructureEconomyManagerAddPowerFacilityNear3Residential3Connected()
        {
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZOneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZOneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);

            Assert.True(residentialZone.HasPower());
            Assert.True(residentialZone1.HasPower());
            Assert.True(residentialZone2.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 3);
        }

        [Test]
        public void StructureEconomyManagerRemovePowerFacilityNear3Residential3Connected()
        {
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZOneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZOneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            StructureEconomyManager.PrepareFacilityRemoval(new Vector3Int(6, 0, 6), _grid);
            _grid.RemoveStructureFromTheGrid(new Vector3Int(6, 0, 6));
            Assert.False(residentialZone.HasPower());
            Assert.False(residentialZone1.HasPower());
            Assert.False(residentialZone2.HasPower());
            Assert.True(_grid.GetStructureDataFromTheGrid(new Vector3Int(6, 0, 6)) == null);
        }

        [Test]
        public void StructureEconomyManager3ResidentialsConnectedToFacilityRemove2()
        {
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            ZoneStructureSO residentialZone1 = CreateZOneAtPosition(new Vector3Int(0, 0, 3));
            ZoneStructureSO residentialZone2 = CreateZOneAtPosition(new Vector3Int(0, 0, 6));
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            StructureEconomyManager.PrepareStructureForRemoval(new Vector3Int(0, 0, 0), _grid);
            StructureEconomyManager.PrepareStructureForRemoval(new Vector3Int(0, 0, 3), _grid);
            _grid.RemoveStructureFromTheGrid(new Vector3(0, 0, 0));
            _grid.RemoveStructureFromTheGrid(new Vector3(0, 0, 3));
            Assert.True(residentialZone2.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 1);
        }

        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityConnect()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 0), FacilityType.Power);
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            Assert.True(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 1);
        }


        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityTooFar()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 6), FacilityType.Power);
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 0);
        }

        [Test]
        public void StructureEconomyManagerPlaceResidentialAfterFacilityTooSmallFacilityRange()
        {
            SingleFacilitySO facility = CreateFacilityAtPosition(new Vector3Int(6, 0, 0), FacilityType.Power);
            facility.singleStructureRange = 1;
            ZoneStructureSO residentialZone = CreateZOneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasPower());
            Assert.True(facility.GetNumberOfCustomers() == 0);
        }
        private SingleFacilitySO CreateFacilityAtPosition(Vector3Int positon, FacilityType facilityType = FacilityType.None)
        {
            SingleFacilitySO facility = new SingleFacilitySO();
            facility.requireRoadAccess = true;
            facility.singleStructureRange = 3;
            facility.facilityType = facilityType;
            facility.maxCustomers = 3;
            _grid.PlaceStructureOnTheGrid(_structureObject, positon, facility);
            StructureEconomyManager.PrepareFacilityStructure(positon, _grid);
            return facility;
        }

        private void CreateRoadATPosition(Vector3Int positon)
        {
            RoadStructureSO roadStructure = ScriptableObject.CreateInstance<RoadStructureSO>();
            _grid.PlaceStructureOnTheGrid(_structureObject, positon, roadStructure);
            StructureEconomyManager.PrepareRoadStructure(positon, _grid);
        }

        private ZoneStructureSO CreateZOneAtPosition(Vector3Int positon)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, positon, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(positon, _grid);
            return residentialZone;
        }

        private static ZoneStructureSO CreateResidentialZone()
        {
            ZoneStructureSO residentialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            residentialZone.requireRoadAccess = true;
            residentialZone.requirePower = true;
            residentialZone.requireWater = true;
            residentialZone.upkeepCost = 30;
            residentialZone.maxFacilitySearchRange = 2;
            return residentialZone;
        }

        private void CreateRoadAtPosition(Vector3Int position)
        {
            RoadStructureSO roadStructure = ScriptableObject.CreateInstance<RoadStructureSO>();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, roadStructure);
            StructureEconomyManager.PrepareRoadStructure(position, _grid);
        }

        private ZoneStructureSO CreateZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return residentialZone;
        }
    }
}
