using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PlayerState
{
    protected GameManager _gameManager;
    protected CameraMovement _cameraMovement;
    protected BuildingManager _buildingManager;

    public PlayerState(GameManager gameManager, BuildingManager buildingManager)
    {
        this._gameManager = gameManager;
        this._buildingManager = buildingManager;
        this._cameraMovement = _gameManager.cameraMovement;
    }

    public virtual void OnConfirmAction()
    {
        AudioManager.Instance.PlayPlaceBuildingSound();
        this._buildingManager.ConfirmModification();
        this._gameManager.TransitionToState(this._gameManager.selectionState, null);
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
    }

    public void OnUpgradeStructure()
    {
        this._buildingManager.CancelModification();
        this._gameManager.TransitionToState(this._gameManager.upgradeState, null);
    }
}
