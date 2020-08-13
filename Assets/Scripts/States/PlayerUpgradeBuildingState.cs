using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeBuildingState : PlayerState
{
    private string _structureName;

    public PlayerUpgradeBuildingState(GameManager gameManager, BuildingManager buildingManager, IResourceManager resourceManager) 
        : base(gameManager, buildingManager, resourceManager)
    {

    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForUpgradeAt(position);
    }

    public override void EnterState(string structureName)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
        this._structureName = structureName;
    }
}
