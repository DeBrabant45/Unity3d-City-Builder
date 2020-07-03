using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemoveBuildingState : PlayerState
{
    private BuildingManager _buildingManager;

    public PlayerRemoveBuildingState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager)
    {
        this._buildingManager = buildingManager;    
    }

    public override void OnCancel()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
    }

    public override void OnConfirmAction()
    {
        this._buildingManager.ConfirmModification();
        base.OnConfirmAction();
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
