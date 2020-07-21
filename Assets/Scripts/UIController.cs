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

    private Action _onCancelActionHandler;
    private Action _onRemoveActionHandler;
    private Action _onConfirmActionHandler;

    public StructureRepository structureRepository;
    public Button buildResidentialAreaBtn;
    public Button cancelActionBtn;
    public Button confirmActionBtn;
    public GameObject cancelActionPanel;

    public GameObject buildingMenuPanel;
    public Button openBuildingMenuBtn;
    public Button removeBtn;

    public GameObject zonesPanel;
    public GameObject facilitiesPanel;
    public GameObject roadsPanel;
    public Button closeBuildMenuBtn;

    public GameObject buildButtonPrefab;

    public TextMeshProUGUI moneyValue;
    public TextMeshProUGUI populationValue;

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
        closeBuildMenuBtn.onClick.AddListener(OnCloseMenuHandler);
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

    private void PrepareUIForBuilding()
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
}
