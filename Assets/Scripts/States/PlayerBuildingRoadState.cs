using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuildingRoadState : PlayerState
{
    private BuildingManager _buildingManager;
    private string _structureName;

    public PlayerBuildingRoadState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager)
    {
        this._buildingManager = buildingManager;
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

    public override void OnInputPointerChange(Vector3 position)
    {
        this._buildingManager.PrepareStructureForModification(position, this._structureName, StructureType.Road);
    }

    public override void OnInputPointerup()
    {
        this._buildingManager.StopContinuousPlacement();
    }

    public override void OnConfirmAction()
    {
        this._buildingManager.ConfirmModification();
        base.OnConfirmAction();
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

    public override void OnCancel()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
    }
}
