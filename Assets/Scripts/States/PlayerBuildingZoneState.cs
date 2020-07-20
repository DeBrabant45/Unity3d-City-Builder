using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingZoneState : PlayerState
{
    private string _structureName;

    public PlayerBuildingZoneState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager, buildingManager)
    {

    }

    public override void EnterState(string structureName)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
        this._structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForModification(position, this._structureName, StructureType.Zone);
    }

    public override void OnInputPointerChange(Vector3 position)
    {
        this._buildingManager.PrepareStructureForModification(position, this._structureName, StructureType.Zone);
    }

    public override void OnInputPointerup()
    {
        this._buildingManager.StopContinuousPlacement();
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
}
