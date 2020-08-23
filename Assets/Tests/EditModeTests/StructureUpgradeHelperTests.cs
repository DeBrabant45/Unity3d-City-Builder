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
        private string _structureName = "Commercial";
        private Vector3 _gridPosition1 = Vector3.zero;
        private Vector3 _gridPosition2 = new Vector3(3, 0, 3);
        private StructureModificationHelper _structureModificationHelper;
        private StructureModificationHelper _structureModificationHelper1;

        [SetUp]
        public void Init()
        {
            StructureRepository structureRepository = TestHelpers.CreateStructureRepositoryContainingZoneStructure();
            IPlacementManager placementManager = Substitute.For<IPlacementManager>();
            _tempObject = new GameObject();
            placementManager.CreateGhostStructure(default, default).ReturnsForAnyArgs(_tempObject);
            _grid = new GridStructure(3, 10, 10);

            _grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition1, null);
            _grid.PlaceStructureOnTheGrid(_tempObject, _gridPosition2, null);

            IResourceManager resourceManager = Substitute.For<IResourceManager>();
            resourceManager.CanIBuyIt(default).Returns(true);
            _structureModificationHelper1 = new ZonePlacementHelper(structureRepository, _grid, placementManager, Vector3.zero, resourceManager);
            _structureModificationHelper = new StructureUpgradeHelper(structureRepository, _grid, placementManager, resourceManager);

        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasAlreadyUpgraded()
        {
            ZoneStructureSO residentialZone = CreateZoneAtPosition(new Vector3Int(0, 0, 0));
            Assert.True(residentialZone.HasUpgraded());
        }

        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasNotUpgraded()
        {
            ZoneStructureSO residentialZone = CreateZonePosition(new Vector3Int(0, 0, 0));
            residentialZone.upgradeActive = false;
            Assert.False(residentialZone.HasUpgraded());
        }        
        
        // A Test behaves as an ordinary method
        [Test]
        public void ResidentialZoneHasCanUpgraded()
        {
            ZoneStructureSO CreateComZone = CreateZonePosition(new Vector3Int(3, 0, 3));
            //residentialZone.upgradeActive = false;
            //_structureModificationHelper1.PrepareStructureForModification(_gridPosition2, _structureName, _structureType);
            //_structureModificationHelper1.ConfirmModifications();
            _structureModificationHelper.PrepareStructureForModification(_gridPosition2, "", StructureType.None);
            Assert.False(CreateComZone.HasUpgraded());
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
            residentialZone.upgradeActive = true;
            residentialZone.upkeepCost = 0;
            residentialZone.prefab = TestPrefab;
            //residentialZone.upgradePrefabVariants[0] = TestPrefab2;
            residentialZone.maxFacilitySearchRange = 2;
            return residentialZone;
        }        
        
        private static ZoneStructureSO CreateComZone()
        {
            ZoneStructureSO comZone = ScriptableObject.CreateInstance<ZoneStructureSO>();
            GameObject TestPrefab = new GameObject();
            GameObject TestPrefab2 = new GameObject();
            comZone.buildingName = "Commercial";
            comZone.zoneType = ZoneType.Commercial;
            comZone.requireRoadAccess = true;
            comZone.requirePower = true;
            comZone.requireWater = true;
            comZone.upgradable = true;
            //comZone.upgradeActive = true;
            comZone.upkeepCost = 0;
            comZone.prefab = TestPrefab;
            comZone.maxFacilitySearchRange = 2;
            comZone.upgradePrefab = TestPrefab2;
            return comZone;
        }

        private ZoneStructureSO CreateZoneAtPosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateResidentialZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return residentialZone;
        }        
        
        private ZoneStructureSO CreateZonePosition(Vector3Int position)
        {
            ZoneStructureSO residentialZone = CreateComZone();
            _grid.PlaceStructureOnTheGrid(_structureObject, position, residentialZone);
            StructureEconomyManager.PrepareZoneStructure(position, _grid);
            return residentialZone;
        }
    }
}
