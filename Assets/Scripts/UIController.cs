using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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

    public GameObject residentialSelectMenuPanel;
    public GameObject commercialSelectMenuPanel;
    public GameObject agricultureSelectMenuPanel;

    public Button residentialOpenMenuBtn;
    public Button commercialOpenMenuBtn;
    public Button agricultureOpenMenuBtn;

    public GameObject buildButtonPrefab;
    public GameObject buildPanelPrefab;

    public TextMeshProUGUI moneyValue;
    public TextMeshProUGUI shoppingCartValue;
    public TextMeshProUGUI populationValue;
    public TextMeshProUGUI woodMaterialValue;

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

        PrepareBuildMenu();

        residentialSelectMenuPanel.SetActive(false);
        commercialSelectMenuPanel.SetActive(false);
        agricultureSelectMenuPanel.SetActive(false);

        residentialOpenMenuBtn.onClick.AddListener(OnOpenResidentialMenu);
        commercialOpenMenuBtn.onClick.AddListener(OnOpenCommercialMenu);
        agricultureOpenMenuBtn.onClick.AddListener(OnOpenAgricultureMenu);

        helpMenuPanel.SetActive(false);
        openHelpMenuBtn.onClick.AddListener(OnOpenHelpMenu);
        closeHelpMenuBtn.onClick.AddListener(OnCloseHelpMenu);

        insufficientFundsPanel.SetActive(false);
        closeInsufficientFundsBtn.onClick.AddListener(OnCloseInsufficientFundsAlert);

        gameOverPanel.SetActive(false);
        replayGameBtn.onClick.AddListener(OnReplayGame);

        fadePanel.SetActive(true);
    }

    private void OnOpenCommercialMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        commercialSelectMenuPanel.SetActive(true);
    }

    private void OnCloseComericalMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        commercialSelectMenuPanel.SetActive(false);
    }

    private void OnOpenResidentialMenu()
    {
        buildingMenuPanel.SetActive(false);
        AudioManager.Instance.PlayButtonClickedSound();
        residentialSelectMenuPanel.SetActive(true);
    }

    private void OnOpenAgricultureMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        agricultureSelectMenuPanel.SetActive(true);
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
    }

    private void PrepareBuildMenu()
    {
        CreateResidentialBuildMenu();
        CreateCommercialBuildMenu();
        CreateAgricultureBuildMenu();
    }

    private void CreateResidentialBuildMenu()
    {
        CreateBuildMenu(residentialSelectMenuPanel.transform, structureRepository.GetResidentialInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateCommercialBuildMenu()
    {
        CreateBuildMenu(commercialSelectMenuPanel.transform, structureRepository.GetCommercialInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateAgricultureBuildMenu()
    {
        CreateBuildMenu(agricultureSelectMenuPanel.transform, structureRepository.GetAgricultureInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateBuildMenu(Transform panelTranform, List<StructureBaseSO> structureData, Action<string> callback, Action backToMenuAction, Action cancelAction)
    {
        CreateChildPanel(panelTranform, structureData);
        AddStructureDataToPanelChildButton(panelTranform, structureData, callback);
        AddStructureDataToPanelChildText(panelTranform, structureData);
        AddStructureDataToPanelChildImage(panelTranform, structureData);
        SetBackButtonActionInPanel(panelTranform, backToMenuAction);
        SetCancelButtonActionInPanel(panelTranform, cancelAction);
    }

    private void CreateChildPanel(Transform panelTransform, List<StructureBaseSO> dataToShow)
    {
        if(dataToShow.Count >= panelTransform.childCount)
        {
            int quantityDifference = dataToShow.Count - (panelTransform.childCount-1);
            for(int i = 0; i < quantityDifference; i++)
            {
                Instantiate(buildPanelPrefab, panelTransform);
            }
        }
    }

    void AddStructureDataToPanelChildButton(Transform panelTransform, List<StructureBaseSO> dataToShow, Action<string> callback)
    {
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                foreach (Transform childOfPanelChild in panelChild)
                {
                    var panelChildbutton = childOfPanelChild.GetComponent<Button>();
                    if (panelChildbutton != null)
                    {
                        foreach (var data in dataToShow)
                        {
                            panelChildbutton.GetComponentInChildren<TextMeshProUGUI>().text = data.buildingName;
                            panelChildbutton.onClick.AddListener(() => callback(panelChildbutton.GetComponentInChildren<TextMeshProUGUI>().text));
                        }
                    }
                }
            }
        }
    }

    void AddStructureDataToPanelChildText(Transform panelTransform, List<StructureBaseSO> structureData)
    {
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                var panelChildText = panelChild.GetComponentInChildren<TextMeshProUGUI>();
                if (panelChildText != null)
                {
                    foreach (var structure in structureData)
                    {
                        panelChildText.GetComponentInChildren<TextMeshProUGUI>().text = "Placement cost: " + structure.placementCost;
                    }
                }
            }
        }
    }

    void AddStructureDataToPanelChildImage(Transform panelTransform, List<StructureBaseSO> structureData)
    {
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                var panelChildImage = panelChild.GetComponent<Image>();
                if (panelChildImage != null)
                {
                    foreach (var structure in structureData)
                    {
                        panelChildImage.GetComponent<Image>().sprite = structure.buildingImage;
                    }
                }
            }
        }
    }

    void SetBackButtonActionInPanel(Transform panelTransform, Action callback)
    {
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name == "ToolsPanel")
            {
                foreach (Transform childOfPanelChild in panelChild)
                {
                    if (childOfPanelChild.gameObject.name == "BackBtn")
                    {
                        var panelChildbutton = childOfPanelChild.GetComponent<Button>();
                        panelChildbutton.onClick.AddListener(() => callback());
                    }
                }
            }
        }
    }

    void SetCancelButtonActionInPanel(Transform panelTransform, Action callback)
    {
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name == "ToolsPanel")
            {
                foreach (Transform childOfPanelChild in panelChild)
                {
                    if (childOfPanelChild.gameObject.name == "CancelBtn")
                    {
                        var panelChildbutton = childOfPanelChild.GetComponent<Button>();
                        panelChildbutton.onClick.AddListener(() => callback());
                    }
                }
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

    public void SetWoodValue(int woodAmount)
    {
        woodMaterialValue.text = woodAmount + "";
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

    public void DisplayManufactureStructureInfo(ManufacturerBaseSO data)
    {
        structureInfoPanelHelper.DisplayManufactureStructureInfo(data);
    }

    private void OnBackToBuildMenu()
    {
        CloseAllSelectMenus();
        buildingMenuPanel.SetActive(true);
    }

    private void OnCancelSelectionMenu()
    {
        CloseAllSelectMenus();
    }

    private void CloseAllSelectMenus()
    {
        residentialSelectMenuPanel.SetActive(false);
        commercialSelectMenuPanel.SetActive(false);
        agricultureSelectMenuPanel.SetActive(false);
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
        residentialSelectMenuPanel.SetActive(false);
        commercialSelectMenuPanel.SetActive(false);
        agricultureSelectMenuPanel.SetActive(false);
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
