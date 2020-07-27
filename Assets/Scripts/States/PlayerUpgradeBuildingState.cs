using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUpgradeBuildingState : PlayerState
{
    public PlayerUpgradeBuildingState(GameManager gameManager, BuildingManager buildingManager) 
        : base(gameManager, buildingManager)
    {

    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForUpgradeAt(position);
    }

    public override void EnterState(string model)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
    }
}
