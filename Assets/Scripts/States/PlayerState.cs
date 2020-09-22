using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected GameManager _gameManager;
    protected CameraMovement _cameraMovement;
    protected BuildingManager _buildingManager;
    protected IResourceManager _resourceManager;

    public PlayerState(GameManager gameManager, BuildingManager buildingManager, IResourceManager resourceManager)
    {
        this._gameManager = gameManager;
        this._buildingManager = buildingManager;
        this._resourceManager = resourceManager;
        this._cameraMovement = _gameManager.cameraMovement;
    }

    public virtual void OnConfirmAction()
    {
        if(_resourceManager.CanIBuyIt(_resourceManager.ShoppingCartMoneyAmount(), _resourceManager.ShoppingCartSteelAmount(), _resourceManager.ShoppingCartWoodAmount()))
        {
            AudioManager.Instance.PlayPlaceBuildingSound();
            this._buildingManager.ConfirmModification();
            this._gameManager.TransitionToState(this._gameManager.selectionState, null);
        }
        else
        {
            AudioManager.Instance.PlayInsufficientFundsSound();
            this._gameManager.uIController.PrepareUIForBuilding();
        }
    }

    public virtual void OnInputPointerDown(Vector3 position)
    {

    }

    public virtual void OnInputPointerChange(Vector3 position)
    {

    }

    public virtual void OnInputPointerup()
    {

    }

    public virtual void EnterState(string model) 
    {

    }

    internal void OnBuildManufacturer(string structureName)
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.buildingManufacturerState, structureName);
    }

    public virtual void OnBuildZone(string structureName)
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.buildingZoneState, structureName);
    }

    public virtual void OnBuildSingleStructure(string structureName)
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.buildingSingleStructureState, structureName);
    }

    public virtual void OnBuildRoad(string structureName)
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.buildingRoadState, structureName);
    }
    
    public virtual void OnRemovalStructure()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.removalState, null);
    }

    public virtual void OnInputPanChange(Vector3 panPosition)
    {
        _cameraMovement.MoveCamera(panPosition);
    }

    public virtual void OnInputPanUp()
    {
        _cameraMovement.StopCameraMovement();
    }

    public virtual void OnCancel()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
        if(_gameManager.uIController.insufficientFundsPanel.activeSelf == true)
        {
            _gameManager.uIController.insufficientFundsPanel.SetActive(false);
        }
    }

    public void OnUpgradeStructure()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.upgradeState, null);
    }
}
