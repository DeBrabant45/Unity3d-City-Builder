using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemoveBuildingState : PlayerState
{
    public PlayerRemoveBuildingState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager, buildingManager)
    {

    }

    public override void OnBuildRoad(string structureName)
    {
        this._buildingManager.CancelModification();
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this._buildingManager.CancelModification();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnBuildZone(string structureName)
    {
        this._buildingManager.CancelModification();
        base.OnBuildZone(structureName);
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForRemovalAt(position);
    }

    public override void EnterState(string model)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
    }
}
