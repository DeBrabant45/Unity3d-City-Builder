using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerRemoveBuildingState : PlayerState
{
    public PlayerRemoveBuildingState(GameManager gameManager, BuildingManager buildingManager, IResourceManager resourceManager)
        :base(gameManager, buildingManager, resourceManager)
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
        if (_resourceManager.CanIBuyIt(_resourceManager.ShoppingCartMoneyAmount(), 0, 0))
        {
            AudioManager.Instance.PlayRemoveSound();
            this._buildingManager.ConfirmModification();
            this._gameManager.TransitionToState(this._gameManager.selectionState, null);
        }
        else
        {
            this._gameManager.uIController.PrepareUIForBuilding();
        }
    }
}
