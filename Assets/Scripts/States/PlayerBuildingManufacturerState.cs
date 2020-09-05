using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingManufacturerState : PlayerState
{
    private string _structureName;

    public PlayerBuildingManufacturerState(GameManager gameManager, BuildingManager buildingManager, IResourceManager resourceManager) 
        : base(gameManager, buildingManager, resourceManager)
    {

    }
    public override void EnterState(string structureName)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
        this._structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForModification(position, this._structureName, StructureType.Manufacturer);
    }
}
