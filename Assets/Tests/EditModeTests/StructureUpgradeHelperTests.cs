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
        private GameObject _structureObject = new GameObject();
        private StructureType _structureType = StructureType.Zone;
        private Vector3 _gridPosition1 = new Vector3(3, 0, 3);
        private Vector3Int _gridPosition1Int;
        private StructureModificationHelper _structureModificationHelper;

        [SetUp]
        public void Init()
        {
            StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingZoneStructure();
            IPlacementManager placementManager = Substitute.For<IPlacementManager>();
            _tempObject = new GameObject();
            _gridPosition1Int = Vector3Int.FloorToInt(_gridPosition1);
            placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_tempObject);
            _grid = new GridStructure(3, 10, 10);
            IResourceManager resourceManager = Substitute.For<IResourceManager>();
            resourceManager.CanIBuyIt(default).Returns(true);
            _structureModificationHelper = new StructureUpgradeHelper(structureRepository, _grid, placementManager, resourceManager);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasAlreadyUpgraded()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(0, 0, 0));
            residentialZone.upgradeActive = true;
            Assert.True(residentialZone.HasUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasNotUpgraded()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.False(residentialZone.HasUpgraded());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneSelectForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }       
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveStructureToBeModifiedPasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }               

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveOldStructureForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            Assert.IsNull(objectInOldStructureDictionary);
        }

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveOldStructureForUpgradeAnSetOldStructureGameObectToActivePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            var structureGameObject = _grid.GetStructureFromTheGrid(_gridPosition1);
            Assert.IsTrue(structureGameObject.activeSelf == true);
        }

        [Test]
        public void ResidentialZoneRevokeStructureUpgradePlacementAtRemoveNewStuctureDataForUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneCancelUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            GameObject objectInOldStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInOldStructuresDictionary(_gridPosition1);
            StructureBaseSO objectInNewStructureDictionary = ((StructureUpgradeHelper)_structureModificationHelper).AccessStructureInNewStructureDataDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
            Assert.IsNull(objectInOldStructureDictionary);
            Assert.IsNull(objectInNewStructureDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneConfirmUpgradePasses()
        {
            ZoneStructureSO residentialZone = CreateResidentialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(residentialZone.HasUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneSelectForUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneCancelUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void CommercialZoneConfirmUpgradePasses()
        {
            ZoneStructureSO commercialZone = CreateCommercialZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(commercialZone.HasUpgraded());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneSelectForUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneCancelUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void AgricultureZoneConfirmUpgradePasses()
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZoneAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(agricultureZone.HasUpgraded());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void PowerPlantSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(powerPlant.HasUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void WaterTowerSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(waterTower.HasUpgraded());
        }


        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilitySelectForUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.AreEqual(_tempObject, objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilityCancelUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.CancelModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsNull(objectInDictionary);
        }

        // A Test behaves as an ordinary method
        [Test]
        public void SiloSingleFacilityConfirmUpgradePasses()
        {
            SingleFacilitySO silo = CreateSiloSingleFacilityAtPosition(new Vector3Int(3, 0, 3));
            _structureModificationHelper.PrepareStructureForModification(_gridPosition1, "", StructureType.None);
            _structureModificationHelper.ConfirmModifications();
            GameObject objectInDictionary = _structureModificationHelper.AccessStructureInDictionary(_gridPosition1);
            Assert.IsTrue(silo.HasUpgraded());
        }

        private static ZoneStructureSO CreateResidentialZone()
        {
            ZoneStructureSO residentialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            residentialZone.requireRoadAccess = true;
            residentialZone.requirePower = true;
            residentialZone.requireWater = true;
            residentialZone.upgradable = true;
            residentialZone.upgradeActive = false;
            residentialZone.upkeepCost = 0;
            residentialZone.prefab = TestPrefab;
            residentialZone.upgradePrefabVariants = new GameObject[1] { TestPrefab2 };
            residentialZone.maxFacilitySearchRange = 2;
            return residentialZone;
        }        
        
        private static ZoneStructureSO CreateCommercialZone()
        {
            ZoneStructureSO commercialZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            commercialZone.buildingName = "Commercial";
            commercialZone.zoneType = ZoneType.Commercial;
            commercialZone.requireRoadAccess = true;
            commercialZone.requirePower = true;
            commercialZone.requireWater = true;
            commercialZone.upgradable = true;
            commercialZone.upgradeActive = false;
            commercialZone.upkeepCost = 0;
            commercialZone.prefab = TestPrefab;
            commercialZone.maxFacilitySearchRange = 2;
            commercialZone.upgradePrefab = TestPrefab2;
            return commercialZone;
        }

        private static ZoneStructureSO CreateAgricultureZone()
        {
            ZoneStructureSO agricultureZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            agricultureZone.buildingName = "Agriculture";
            agricultureZone.zoneType = ZoneType.Agridcultural;
            agricultureZone.requireRoadAccess = true;
            agricultureZone.requirePower = true;
            agricultureZone.requireWater = true;
            agricultureZone.upgradable = true;
            agricultureZone.upgradeActive = false;
            agricultureZone.upkeepCost = 0;
            agricultureZone.prefab = TestPrefab;
            agricultureZone.maxFacilitySearchRange = 2;
            agricultureZone.upgradePrefab = TestPrefab2;
            return agricultureZone;
        }

        private static SingleFacilitySO CreatePowerPlantSingleFacility()
        {
            SingleFacilitySO powerPlant = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            powerPlant.buildingName = "Power plant";
            powerPlant.facilityType = FacilityType.Power;
            powerPlant.requireRoadAccess = true;
            powerPlant.requirePower = false;
            powerPlant.requireWater = false;
            powerPlant.upgradable = true;
            powerPlant.upgradeActive = false;
            powerPlant.upkeepCost = 0;
            powerPlant.prefab = TestPrefab;
            powerPlant.upgradePrefab = TestPrefab2;
            return powerPlant;
        }

        private static SingleFacilitySO CreateWaterTowerSingleFacility()
        {
            SingleFacilitySO waterTower = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            waterTower.buildingName = "Water Tower";
            waterTower.facilityType = FacilityType.Water;
            waterTower.requireRoadAccess = true;
            waterTower.requirePower = false;
            waterTower.requireWater = false;
            waterTower.upgradable = true;
            waterTower.upgradeActive = false;
            waterTower.upkeepCost = 0;
            waterTower.prefab = TestPrefab;
            waterTower.upgradePrefab = TestPrefab2;
            return waterTower;
        }

        private static SingleFacilitySO CreateSiloSingleFacility()
        {
            SingleFacilitySO silo = ScriptableObject.CreateInstance<SingleFacilitySO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            silo.buildingName = "Silo";
            silo.facilityType = FacilityType.Silo;
            silo.requireRoadAccess = true;
            silo.requirePower = false;
            silo.requireWater = false;
            silo.upgradable = true;
            silo.upgradeActive = false;
            silo.upkeepCost = 0;
            silo.prefab = TestPrefab;
            silo.upgradePrefab = TestPrefab2;
            return silo;
        }

        private ZoneStructureSO CreateResidentialZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return residentialZone;
        }        
        
        private ZoneStructureSO CreateCommercialZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO commercialZone = CreateCommercialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, commercialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return commercialZone;
        }

        private ZoneStructureSO CreateAgricultureZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO agricultureZone = CreateAgricultureZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, agricultureZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return agricultureZone;
        }

        private SingleFacilitySO CreatePowerPlantSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO powerPlant = CreatePowerPlantSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, powerPlant);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return powerPlant;
        }

        private SingleFacilitySO CreateWaterTowerSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO waterTower = CreateWaterTowerSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, waterTower);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return waterTower;
        }

        private SingleFacilitySO CreateSiloSingleFacilityAtPosition(Vector3Int position)
        {
            SingleFacilitySO silo = CreateSiloSingleFacility();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, silo);
            StructureEconomyManager.PrepareFacilityStructure(position, _grid);
            return silo;
        }
    }
}
