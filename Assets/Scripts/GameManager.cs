using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BuildingManager _buildingManager;
    private int _cellSize = 3;
    private PlayerState state;

    public PlacementManager placementManager;
    public IInputManager inputManager;
    public UIController uIController;
    public CameraMovement cameraMovement;
    public StructureRepository structureRepository;
    public int width, length;
    //States
    public PlayerState State { get => state; }
    public PlayerSelectionState selectionState;
    public PlayerBuildingSingleStructureState buildingSingleStructureState;
    public PlayerRemoveBuildingState removalState;
    public PlayerBuildingRoadState buildingRoadState;
    public PlayerBuildingZoneState buildingZoneState;

    public LayerMask inputMask;

    private void Awake() 
    {
        _buildingManager = new BuildingManager(_cellSize, width, length, placementManager, structureRepository);
        PrepareStates();

#if (UNITY_EDITOR && TEST) || !(UNITY_IOS || UNITY_ANDROID)
        inputManager = gameObject.AddComponent<InputManager>();
#endif       
    }

    private void PrepareStates()
    {
        selectionState = new PlayerSelectionState(this);
        removalState = new PlayerRemoveBuildingState(this, _buildingManager);
        buildingSingleStructureState = new PlayerBuildingSingleStructureState(this, _buildingManager);
        buildingRoadState = new PlayerBuildingRoadState(this, _buildingManager);
        buildingZoneState = new PlayerBuildingZoneState(this, _buildingManager);
        state = selectionState;
    }

    // Start is called before the first frame update
    void Start()
    {
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
    }

    private void AssignUIControllerListeners()
    {
        uIController.AddListenerOnBuildZoneEvent((structureName) => state.OnBuildZone(structureName));
        uIController.AddListenerOnBuildSingleStructureEvent((structureName) => state.OnBuildSingleStructure(structureName));
        uIController.AddListenerOnRoadEvent((structureName) => state.OnBuildRoad(structureName));
        uIController.AddListenerOnRemoveActionEvent(() => state.OnRemovalStructure());
        uIController.AddListenerOnCancelActionEvent(() => state.OnCancel());
        uIController.AddListenerOnConfirmActionEvent(() => state.OnConfirmAction());
    }

    public void TransitionToState(PlayerState newState, string model)
    {
        this.state = newState;
        this.state.EnterState(model);
    }
}
