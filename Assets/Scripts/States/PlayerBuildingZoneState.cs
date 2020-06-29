using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingZoneState : PlayerState
{
    private BuildingManager _buildingManager;
    private string _structureName;

    public PlayerBuildingZoneState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager)
    {
        this._buildingManager = buildingManager;
    }

    public override void EnterState(string structureName)
    {
        this._structureName = structureName;
    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForPlacement(position, this._structureName, StructureType.Zone);
    }

    public override void OnConfirmAction()
    {
        this._buildingManager.ConfirmPlacement();
        base.OnConfirmAction();
    }
    
    public override void OnBuildRoad(string structureName)
    {
        this._buildingManager.CancelPlacement();
        base.OnBuildRoad(structureName);
    }

    public override void OnBuildSingleStructure(string structureName)
    {
        this._buildingManager.CancelPlacement();
        base.OnBuildSingleStructure(structureName);
    }

    public override void OnCancel()
    {
        this._buildingManager.CancelPlacement();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
    }

}
