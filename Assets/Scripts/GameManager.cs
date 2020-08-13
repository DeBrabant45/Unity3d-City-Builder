using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private int _cellSize = 3;
    private PlayerState state;
    private IPlacementManager _placementManager;
    private IResourceManager _resourceManager;

    public GameObject placementManagerGameObject;
    public GameObject resourceManagerGameObject;
    public IInputManager inputManager;
    public UIController uIController;
    public CameraMovement cameraMovement;
    public StructureRepository structureRepository;
    public int width, length;
    public WorldManager worldManager;
    //States
    public PlayerState State { get => state; }

    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerRemoveBuildingState removalState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingZoneState buildingZoneState;
    public PlayerUpgradeBuildingState upgradeState;

    public LayerMask inputMask;

    private void Awake() 
    {
#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
#endif       
    }

    private void PrepareStates()
    {
        selectionState = new PlayerSelectionState(this, _buildingManager, _resourceManager);
        removalState = new PlayerRemoveBuildingState(this, _buildingManager, _resourceManager);
        upgradeState = new PlayerUpgradeBuildingState(this, _buildingManager, _resourceManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, _buildingManager, _resourceManager);
        buildingRoadState = new PlayerBuildingRoadState(this, _buildingManager, _resourceManager);
        buildingZoneState = new PlayerBuildingZoneState(this, _buildingManager, _resourceManager);
        state = selectionState;
    }

    // Start is called before the first frame update
    void Start()
    {
        //Any interface assignment needs to called before the buildingmanager is assigned or
        //you will get an object refernce not set to an instance of an object
        _placementManager = placementManagerGameObject.GetComponent<IPlacementManager>();
        _resourceManager = resourceManagerGameObject.GetComponent<IResourceManager>();
        //
        worldManager.PrepareWorld(_cellSize, width, length);
        _placementManager.PreparePlacementManager(worldManager);
        _buildingManager = new BuildingManager(worldManager.Grid, _placementManager, structureRepository, _resourceManager);
        _resourceManager.PrepareResourceManager(_buildingManager);
        PrepareStates();
        PrepareGameComponents();
        AssignInputListeners();
        AssignUIControllerListeners();
    }

    private void PrepareGameComponents()
    {
        inputManager.MouseInputMask = inputMask;
        cameraMovement.SetCameraLimits(0, width, 0, length);
    }

    private void AssignInputListeners()
    {
        inputManager.AddListenerOnPointerDownEvent((position) => state.OnInputPointerDown(position));
        inputManager.AddListenerOnPointerSecondChangeEvent((position) => state.OnInputPanChange(position));
        inputManager.AddListenerOnPointerSecondUpEvent(() => state.OnInputPanUp());
        inputManager.AddListenerOnPointerChangeEvent((position) => state.OnInputPointerChange(position));
        inputManager.AddListenerOnPointerUpEvent(() => state.OnInputPointerup());
    }

    private void AssignUIControllerListeners()
    {
        uIController.AddListenerOnBuildZoneEvent((structureName) => state.OnBuildZone(structureName));
        uIController.AddListenerOnBuildSingleStructureEvent((structureName) => state.OnBuildSingleStructure(structureName));
        uIController.AddListenerOnRoadEvent((structureName) => state.OnBuildRoad(structureName));
        uIController.AddListenerOnRemoveActionEvent(() => state.OnRemovalStructure());
        uIController.AddListenerOnCancelActionEvent(() => state.OnCancel());
        uIController.AddListenerOnConfirmActionEvent(() => state.OnConfirmAction());
        uIController.AddListenerOnUpgradeActionEvent(() => state.OnUpgradeStructure());
    }

    public void TransitionToState(PlayerState newState, string model)
    {
        this.state = newState;
        this.state.EnterState(model);
    }
}
