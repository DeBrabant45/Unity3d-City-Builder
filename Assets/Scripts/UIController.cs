using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIController : MonoBehaviour
{
    private Action<string> _onBuildZoneHandler;
    private Action<string> _onBuildSingleStructureHandler;
    private Action<string> _onBuildRoadHandler;
    private Action<string> _onBuildManufacturerHandler;

    private Action _onCancelActionHandler;
    private Action _onRemoveActionHandler;
    private Action _onUpgradeActionHandler;
    private Action _onConfirmActionHandler;

    public StructureRepository structureRepository;
    public Button buildResidentialAreaBtn;
    public Button cancelActionBtn;
    public Button confirmActionBtn;
    public GameObject cancelActionPanel;

    public GameObject buildingMenuPanel;
    public Button openBuildingMenuBtn;
    public Button removeBtn;
    public Button upgradeBtn;

    public GameObject helpMenuPanel;
    public Button openHelpMenuBtn;
    public Button closeHelpMenuBtn;

    public GameObject zonesPanel;
    public GameObject facilitiesPanel;
    public GameObject manufacturersPanel;
    public GameObject roadsPanel;
    public Button closeBuildMenuBtn;

    public GameObject buildButtonPrefab;

    public TextMeshProUGUI moneyValue;
    public TextMeshProUGUI shoppingCartValue;
    public TextMeshProUGUI populationValue;

    public GameObject insufficientFundsPanel;
    public Button closeInsufficientFundsBtn;

    public GameObject gameOverPanel;
    public Button replayGameBtn;

    public GameObject fadePanel;

    public UIStructureInfoPanelHelper structureInfoPanelHelper;
    
    // Start is called before the first frame update
    void Start()
    {
        cancelActionPanel.SetActive(false);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);

        buildingMenuPanel.SetActive(false);
        openBuildingMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        removeBtn.onClick.AddListener(OnRemovalHandler);
        upgradeBtn.onClick.AddListener(OnUpgradeHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);

        helpMenuPanel.SetActive(false);
        openHelpMenuBtn.onClick.AddListener(OnOpenHelpMenu);
        closeHelpMenuBtn.onClick.AddListener(OnCloseHelpMenu);

        insufficientFundsPanel.SetActive(false);
        closeInsufficientFundsBtn.onClick.AddListener(OnCloseInsufficientFundsAlert);

        gameOverPanel.SetActive(false);
        replayGameBtn.onClick.AddListener(OnReplayGame);

        fadePanel.SetActive(true);
    }

    private void OnCloseInsufficientFundsAlert()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        insufficientFundsPanel.SetActive(false);
    }

    private void OnReplayGame()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        LevelManager levelManager = GameObject.Find("LevelManager").GetComponent<LevelManager>();
        levelManager.LoadLevel(1);
    }

    private void OnCloseHelpMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        helpMenuPanel.SetActive(false);
    }

    private void OnOpenHelpMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        helpMenuPanel.SetActive(true);
    }

    private void OnUpgradeHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        cancelActionPanel.SetActive(true);
        _onUpgradeActionHandler?.Invoke();
        OnCloseMenuHandler();
    }

    public void HideStructureInfo()
    {
        structureInfoPanelHelper.Hide();
    }

    public bool GetStructureInfoVisability()
    {
        return structureInfoPanelHelper.gameObject.activeSelf;
    }

    private void OnOpenBuildMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(true);
         PrepareBuildMenu();
    }

    private void PrepareBuildMenu()
    {
        CreateButtonsInPanel(zonesPanel.transform, structureRepository.GetZoneNames(), OnBuildZoneCallback);
        CreateButtonsInPanel(facilitiesPanel.transform, structureRepository.GetSingleStructureNames(), OnBuildSingleStructureCallback);
        CreateButtonsInPanel(manufacturersPanel.transform, structureRepository.GetManufacturerNames(), OnBuildManufacturerCallback);
        CreateButtonsInPanel(roadsPanel.transform, new List<string>() { structureRepository.GetRoadStructureName() }, OnBuildRoadCallback);
    }


    private void CreateButtonsInPanel(Transform panelTransform, List<string> dataToShow, Action<string> callback)
    {
        if(dataToShow.Count > panelTransform.childCount)
        {
            int quantityDifference = dataToShow.Count - panelTransform.childCount;
            for(int i = 0; i < quantityDifference; i++)
            {
                Instantiate(buildButtonPrefab, panelTransform);
            }
        }
        for(int i = 0; i < panelTransform.childCount; i++)
        {
            var button = panelTransform.GetChild(i).GetComponent<Button>();
            if(button != null)
            {
                button.GetComponentInChildren<TextMeshProUGUI>().text = dataToShow[i];
                button.onClick.AddListener(() => callback(button.GetComponentInChildren<TextMeshProUGUI>().text));
            }
        }
    }

    public void SetPopulationValue(int population)
    {
        populationValue.text = population + "";
    }

    public void SetMoneyValue(int moneyAmount)
    {
        moneyValue.text = moneyAmount + "";
    }

    public void SetShoppingCartValue(int cartAmount)
    {
        shoppingCartValue.text = cartAmount + "";
    }

    public void DisplayBasicStructureInfo(StructureBaseSO data)
    {
        structureInfoPanelHelper.DisplayBasicStructureInfo(data);
    }

    public void DisplayZoneStructureInfo(ZoneStructureSO data)
    {
        structureInfoPanelHelper.DisplayZoneStructureInfo(data);
    }

    public void DisplayFacilityStructureInfo(SingleFacilitySO data)
    {
        structureInfoPanelHelper.DisplayFacilityStructureInfo(data);
    }

    private void OnBuildManufacturerCallback(string nameOfStructure)
    {
        PrepareUIForBuilding();
        _onBuildManufacturerHandler?.Invoke(nameOfStructure);
        OnCloseMenuHandler();
    }

    private void OnBuildZoneCallback(string nameOfStructure)
    {
        PrepareUIForBuilding();
        _onBuildZoneHandler?.Invoke(nameOfStructure);
        OnCloseMenuHandler();
    } 

    private void OnBuildRoadCallback(string nameOfStructure)
    {
        PrepareUIForBuilding();
        _onBuildRoadHandler?.Invoke(nameOfStructure);
        OnCloseMenuHandler();
    }

    private void OnBuildSingleStructureCallback(string nameOfStructure)
    {
        PrepareUIForBuilding();
        _onBuildSingleStructureHandler?.Invoke(nameOfStructure);
        OnCloseMenuHandler();
    }

    public void PrepareUIForBuilding()
    {
        cancelActionPanel.SetActive(true);
    }

    private void OnCancelActionCallback()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        cancelActionPanel.SetActive(false);
        _onCancelActionHandler?.Invoke();
    }

    private void OnConfirmActionCallback()
    {
        cancelActionPanel.SetActive(false);
        _onConfirmActionHandler?.Invoke();
    }

    private void OnRemovalHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        cancelActionPanel.SetActive(true);
        _onRemoveActionHandler?.Invoke();
        OnCloseMenuHandler();
    }

    private void OnCloseMenuHandler()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
    }

    public void AddListenerOnBuildManufacturerEvent(Action<string> listener)
    {
        _onBuildManufacturerHandler += listener;
    }

    public void RemoveListenerOnBuildManufacturerEvent(Action<string> listener)
    {
        _onBuildManufacturerHandler -= listener;
    }

    public void AddListenerOnBuildZoneEvent(Action<string> listener)
    {
        _onBuildZoneHandler += listener;
    }

    public void RemoveListenerOnBuildZoneEvent(Action<string> listener)
    {
        _onBuildZoneHandler -= listener;
    }

    public void AddListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        _onBuildSingleStructureHandler += listener;
    }

    public void RemoveListenerOnBuildSingleStructureEvent(Action<string> listener)
    {
        _onBuildSingleStructureHandler -= listener;
    }

    public void AddListenerOnRoadEvent(Action<string> listener)
    {
        _onBuildRoadHandler += listener;
    }

    public void RemoveListenerOnRoadEvent(Action<string> listener)
    {
        _onBuildRoadHandler -= listener;
    }

    public void AddListenerOnCancelActionEvent(Action listener)
    {
        _onCancelActionHandler += listener;
    }

    public void RemoveListenerOnCancelActionEvent(Action listener)
    {
        _onCancelActionHandler -= listener;
    }

    public void AddListenerOnConfirmActionEvent(Action listener)
    {
        _onConfirmActionHandler += listener;
    }

    public void RemoveListenerOnConfirmActionEvent(Action listener)
    {
        _onConfirmActionHandler -= listener;
    }

    public void AddListenerOnRemoveActionEvent(Action listener)
    {
        _onRemoveActionHandler += listener;
    }

    public void RemoveListenerOnRemoveActionEvent(Action listener)
    {
        _onRemoveActionHandler -= listener;
    }

    public void AddListenerOnUpgradeActionEvent(Action listener)
    {
        _onUpgradeActionHandler += listener;
    }

    public void RemoveListenerOnUpgradeActionEvent(Action listener)
    {
        _onUpgradeActionHandler -= listener;
    }
}
