using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingRoadState : PlayerState
{
    private string _structureName;

    public PlayerBuildingRoadState(GameManager gameManager, BuildingManager buildingManager)
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
        this._buildingManager.PrepareStructureForModification(position, this._structureName, StructureType.Road);
    }
}
