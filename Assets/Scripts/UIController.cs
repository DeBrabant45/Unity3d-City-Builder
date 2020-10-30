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
    public GameObject mainBuildPanel;

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
    public GameObject commercialStoreSelectMenuPanel;
    public GameObject commercialBusinessSelectMenuPanel;
    public GameObject agricultureSelectMenuPanel;
    public GameObject entertainmentSelectMenuPanel;
    public GameObject parkSelectMenuPanel;
    public GameObject restaurantSelectMenuPanel;

    public GameObject utilitiesSelectMenuPanel;
    public GameObject emergencySelectMenuPanel;
    public GameObject governmentSelectMenuPanel;

    public GameObject manufactureSelectMenuPanel;

    public GameObject roadSelectMenuPanel;

    public Button residentialOpenMenuBtn;
    public Button commercialStoreOpenMenuBtn;
    public Button commercialBusinessOpenMenuBtn;
    public Button agricultureOpenMenuBtn;
    public Button entertainmentOpenMenuBtn;
    public Button parkOpenMenuBtn;
    public Button restaurantOpenMenuBtn;

    public Button utilitiesOpenMenuBtn;
    public Button emergencyOpenMenuBtn;
    public Button governmentOpenMenuBtn;

    public Button manufactureOpenMenuBtn;

    public Button roadOpenMenuBtn;

    public GameObject buildButtonPrefab;
    public GameObject buildPanelPrefab;

    public GameObject moneyPanel;
    public TextMeshProUGUI moneyValue;

    public GameObject shoppingCartPanel;
    public TextMeshProUGUI shoppingCartMoneyValue;
    public TextMeshProUGUI shoppingCartWoodValue;
    public TextMeshProUGUI shoppingCartSteelValue;

    public GameObject populationPanel;
    public TextMeshProUGUI populationValue;
    public GameObject woodMaterialPanel;
    public TextMeshProUGUI woodMaterialValue;
    public GameObject steelMaterialPanel;
    public TextMeshProUGUI steelMaterialValue;

    public GameObject insufficientFundsPanel;

    public GameObject insufficientFundsMoneyPanel;
    public TextMeshProUGUI insufficientFundsMoneyValue;
    public TextMeshProUGUI insufficientFundsMoneyShoppingCartValue;

    public GameObject insufficientFundsWoodPanel;
    public TextMeshProUGUI insufficientFundsWoodValue;
    public TextMeshProUGUI insufficientFundsWoodShoppingCartValue;

    public GameObject insufficientFundsSteelPanel;
    public TextMeshProUGUI insufficientFundsSteelValue;
    public TextMeshProUGUI insufficientFundsSteelShoppingCartValue;

    public Button closeInsufficientFundsBtn;

    public GameObject gameOverPanel;
    public TextMeshProUGUI gameOverReason;
    public TextMeshProUGUI gameOverTip;
    public Button replayGameBtn;

    public GameObject fadePanel;

    public UIStructureInfoPanelHelper structureInfoPanelHelper;

    // Start is called before the first frame update
    void Start()
    {
        mainBuildPanel.SetActive(true);
        moneyPanel.SetActive(true);
        shoppingCartPanel.SetActive(true);
        woodMaterialPanel.SetActive(true);
        steelMaterialPanel.SetActive(true);
        populationPanel.SetActive(true);
        cancelActionPanel.SetActive(false);
        cancelActionBtn.onClick.AddListener(OnCancelActionCallback);
        confirmActionBtn.onClick.AddListener(OnConfirmActionCallback);

        buildingMenuPanel.SetActive(false);
        openBuildingMenuBtn.onClick.AddListener(OnOpenBuildMenu);
        removeBtn.onClick.AddListener(OnRemovalHandler);
        upgradeBtn.onClick.AddListener(OnUpgradeHandler);
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);

        PrepareBuildMenu();
        CloseAllSelectMenus();

        residentialOpenMenuBtn.onClick.AddListener(OnOpenResidentialMenu);
        commercialStoreOpenMenuBtn.onClick.AddListener(OnOpenCommercialStoreMenu);
        commercialBusinessOpenMenuBtn.onClick.AddListener(OnOpenCommercialBusinessMenu);
        agricultureOpenMenuBtn.onClick.AddListener(OnOpenAgricultureMenu);
        entertainmentOpenMenuBtn.onClick.AddListener(OnOpenEntertainmentMenu);
        parkOpenMenuBtn.onClick.AddListener(OnOpenParkMenu);
        restaurantOpenMenuBtn.onClick.AddListener(OnOpenRestaurantMenu);

        utilitiesOpenMenuBtn.onClick.AddListener(OnOpenUtilitiesMenu);
        emergencyOpenMenuBtn.onClick.AddListener(OnOpenEmergencyMenu);
        governmentOpenMenuBtn.onClick.AddListener(OnOpenGovernmentMenu);

        manufactureOpenMenuBtn.onClick.AddListener(OnOpenMaufactureMenu);

        roadOpenMenuBtn.onClick.AddListener(OnOpenRoadeMenu);

        helpMenuPanel.SetActive(false);
        openHelpMenuBtn.onClick.AddListener(OnOpenHelpMenu);
        closeHelpMenuBtn.onClick.AddListener(OnCloseHelpMenu);

        insufficientFundsPanel.SetActive(false);
        closeInsufficientFundsBtn.onClick.AddListener(OnCloseInsufficientFundsAlert);

        gameOverPanel.SetActive(false);
        replayGameBtn.onClick.AddListener(OnReplayGame);

        fadePanel.SetActive(true);
    }

    public void ActivateInsufficientFundsWoodPanel()
    {
        insufficientFundsWoodPanel.SetActive(true);
        insufficientFundsWoodValue.text = woodMaterialValue.text;
        insufficientFundsWoodShoppingCartValue.text = shoppingCartWoodValue.text;
    }

    public void DeactivateInsufficientFundsWoodPanel()
    {
        insufficientFundsWoodPanel.SetActive(false);
    }

    public void ActivateInsufficientFundsSteelPanel()
    {
        insufficientFundsSteelPanel.SetActive(true);
        insufficientFundsSteelValue.text = steelMaterialValue.text;
        insufficientFundsSteelShoppingCartValue.text = shoppingCartSteelValue.text;
    }

    public void DeactivateInsufficientFundsSteelPanel()
    {
        insufficientFundsSteelPanel.SetActive(false);
    }

    public void ActivateInsufficientFundsMoneyPanel()
    {
        insufficientFundsMoneyPanel.SetActive(true);
        insufficientFundsMoneyValue.text = moneyValue.text;
        insufficientFundsMoneyShoppingCartValue.text = shoppingCartMoneyValue.text;
    }

    public void DeactivateInsufficientFundsMoneyPanel()
    {
        insufficientFundsMoneyPanel.SetActive(false);
    }

    public void ReloadGame()
    {
        CloseAllSelectMenus();
        DeactivateOnScreenUI();
        HideStructureInfo();
        gameOverPanel.SetActive(true);
    }

    private void DeactivateOnScreenUI()
    {
        buildingMenuPanel.SetActive(false);
        cancelActionPanel.SetActive(false);
        mainBuildPanel.SetActive(false);
        moneyPanel.SetActive(false);
        shoppingCartPanel.SetActive(false);
        woodMaterialPanel.SetActive(false);
        steelMaterialPanel.SetActive(false);
        populationPanel.SetActive(false);
    }

    private void OnOpenRestaurantMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        restaurantSelectMenuPanel.SetActive(true);
    }

    private void OnOpenParkMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        parkSelectMenuPanel.SetActive(true);
    }

    private void OnOpenEntertainmentMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        entertainmentSelectMenuPanel.SetActive(true);
    }

    private void OnOpenGovernmentMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        governmentSelectMenuPanel.SetActive(true);
    }

    private void OnOpenCommercialBusinessMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        commercialBusinessSelectMenuPanel.SetActive(true);
    }

    public void OnOpenInsufficientFundsAlertBox()
    {
        AudioManager.Instance.PlayInsufficientFundsSound();
        insufficientFundsPanel.SetActive(true);
    }

    private void OnOpenMaufactureMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        manufactureSelectMenuPanel.SetActive(true);
    }

    private void OnOpenEmergencyMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        emergencySelectMenuPanel.SetActive(true);
    }

    private void OnOpenUtilitiesMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        utilitiesSelectMenuPanel.SetActive(true);
    }

    private void OnOpenRoadeMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        roadSelectMenuPanel.SetActive(true);
    }

    private void OnOpenCommercialStoreMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        buildingMenuPanel.SetActive(false);
        commercialStoreSelectMenuPanel.SetActive(true);
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
        CreateCommercialStoreBuildMenu();
        CreateCommercialBusinessBuildMenu();
        CreateAgricultureBuildMenu();
        CreateRoadBuildMenu();
        CreateUtilitiesBuildMenu();
        CreateEmergencyBuildMenu();
        CreateManufactureBuildMenu();
        CreateGovernmentBuildMenu();
        CreateEntertainmentBuildMenu();
        CreateParkBuildMenu();
        CreateRestaurantBuildMenu();
    }

    private void CreateResidentialBuildMenu()
    {
        CreateBuildMenu(residentialSelectMenuPanel.transform, structureRepository.GetResidentialInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateCommercialStoreBuildMenu()
    {
        CreateBuildMenu(commercialStoreSelectMenuPanel.transform, structureRepository.GetCommercialStoreInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }    
    
    private void CreateCommercialBusinessBuildMenu()
    {
        CreateBuildMenu(commercialBusinessSelectMenuPanel.transform, structureRepository.GetCommercialBusinessInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateAgricultureBuildMenu()
    {
        CreateBuildMenu(agricultureSelectMenuPanel.transform, structureRepository.GetAgricultureInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateUtilitiesBuildMenu()
    {
        CreateBuildMenu(utilitiesSelectMenuPanel.transform, structureRepository.GetUtilitiesInfo(), OnBuildSingleStructureCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateEmergencyBuildMenu()
    {
        CreateBuildMenu(emergencySelectMenuPanel.transform, structureRepository.GetEmergencyInfo(), OnBuildSingleStructureCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateGovernmentBuildMenu()
    {
        CreateBuildMenu(governmentSelectMenuPanel.transform, structureRepository.GetGovernmentStructureInfo(), OnBuildSingleStructureCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateManufactureBuildMenu()
    {
        CreateBuildMenu(manufactureSelectMenuPanel.transform, structureRepository.GetManufactureInfo(), OnBuildManufacturerCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateEntertainmentBuildMenu()
    {
        CreateBuildMenu(entertainmentSelectMenuPanel.transform, structureRepository.GetEntertainmentStructureInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateParkBuildMenu()
    {
        CreateBuildMenu(parkSelectMenuPanel.transform, structureRepository.GetParkStructureInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateRestaurantBuildMenu()
    {
        CreateBuildMenu(restaurantSelectMenuPanel.transform, structureRepository.GetRestaurantStructureInfo(), OnBuildZoneCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
    }

    private void CreateRoadBuildMenu()
    {
        CreateBuildMenu(roadSelectMenuPanel.transform, structureRepository.GetRoadInfo(), OnBuildRoadCallback, OnBackToBuildMenu, OnCancelSelectionMenu);
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
        int count = 0;
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                foreach (Transform childOfPanelChild in panelChild)
                {
                    var panelChildbutton = childOfPanelChild.GetComponent<Button>();
                    if (panelChildbutton != null)
                    {
                        panelChildbutton.GetComponentInChildren<TextMeshProUGUI>().text = dataToShow[count].buildingName;
                        panelChildbutton.onClick.AddListener(() => callback(panelChildbutton.GetComponentInChildren<TextMeshProUGUI>().text));
                        count++;
                    }
                }
            }
        }
    }

    void AddStructureDataToPanelChildText(Transform panelTransform, List<StructureBaseSO> structureData)
    {
        int count = 0;
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                var panelChildText = panelChild.GetComponentInChildren<TextMeshProUGUI>();
                if (panelChildText != null)
                {
                    panelChildText.GetComponentInChildren<TextMeshProUGUI>().text = "Placement cost: " + structureData[count].placementCost;
                    count++;
                }
            }
        }
    }

    void AddStructureDataToPanelChildImage(Transform panelTransform, List<StructureBaseSO> structureData)
    {
        int count = 0;
        foreach (Transform panelChild in panelTransform)
        {
            if (panelChild.gameObject.name != "ToolsPanel")
            {
                foreach (Transform childOfPanelChild in panelChild)
                {
                    if (childOfPanelChild.gameObject.name == "StructureImage")
                    {
                        var childOfPanelChildImage = panelChild.GetComponent<Image>();
                        if (childOfPanelChildImage != null)
                        {
                            childOfPanelChild.GetComponent<Image>().sprite = structureData[count].buildingImage;
                            count++;
                        }
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

    internal void SetSteelValue(int steelAmount)
    {
        steelMaterialValue.text = steelAmount + "";
    }

    public void SetShoppingCartMoneyValue(int cartAmount)
    {
        shoppingCartMoneyValue.text = cartAmount + "";
    }

    public void SetShoppingCartWoodValue(int cartAmount)
    {
        shoppingCartWoodValue.text = cartAmount + "";
    }

    public void SetShoppingCartSteelValue(int cartAmount)
    {
        shoppingCartSteelValue.text = cartAmount + "";
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
        AudioManager.Instance.PlayButtonClickedSound();
        CloseAllSelectMenus();
        buildingMenuPanel.SetActive(true);
    }

    private void OnCancelSelectionMenu()
    {
        AudioManager.Instance.PlayButtonClickedSound();
        CloseAllSelectMenus();
    }

    private void CloseAllSelectMenus()
    {
        residentialSelectMenuPanel.SetActive(false);
        commercialStoreSelectMenuPanel.SetActive(false);
        commercialBusinessSelectMenuPanel.SetActive(false);
        agricultureSelectMenuPanel.SetActive(false);
        utilitiesSelectMenuPanel.SetActive(false);
        emergencySelectMenuPanel.SetActive(false);
        manufactureSelectMenuPanel.SetActive(false);
        roadSelectMenuPanel.SetActive(false);
        governmentSelectMenuPanel.SetActive(false);
        entertainmentSelectMenuPanel.SetActive(false);
        parkSelectMenuPanel.SetActive(false);
        restaurantSelectMenuPanel.SetActive(false);
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
        CloseAllSelectMenus();
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
