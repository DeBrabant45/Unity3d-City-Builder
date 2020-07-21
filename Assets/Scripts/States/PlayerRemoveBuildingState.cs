using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemoveBuildingState : PlayerState
{
    public PlayerRemoveBuildingState(GameManager gameManager, BuildingManager buildingManager)
        :base(gameManager, buildingManager)
    {

    }

    public override void OnInputPointerDown(Vector3 position)
    {
        this._buildingManager.PrepareStructureForRemovalAt(position);
    }

    public override void EnterState(string model)
    {
        this._buildingManager.PrepareBuildingManager(this.GetType());
    }

    public override void OnConfirmAction()
    {
        AudioManager.Instance.PlayRemoveSound();
        this._buildingManager.ConfirmModification();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
    }
}
