using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CreateBuildMenu
{
    public void CreateMenu(GameObject buildPanelPrefab, Transform panelTranform, List<StructureBaseSO> structureData, Action<string> callback, Action backToMenuAction, Action cancelAction)
    {
        CreateChildPanel(buildPanelPrefab, panelTranform, structureData);
        AddStructureDataToPanelChildButton(panelTranform, structureData, callback);
        AddStructureDataToPanelChildText(panelTranform, structureData);
        AddStructureDataToPanelChildImage(panelTranform, structureData);
        SetBackButtonActionInPanel(panelTranform, backToMenuAction);
        SetCancelButtonActionInPanel(panelTranform, cancelAction);
    }

    public void CreateChildPanel(GameObject buildPanelPrefab, Transform panelTransform, List<StructureBaseSO> dataToShow)
    {
        if (dataToShow.Count >= panelTransform.childCount)
        {
            int quantityDifference = dataToShow.Count - (panelTransform.childCount - 1);
            for (int i = 0; i < quantityDifference; i++)
            {
                GameObject.Instantiate(buildPanelPrefab, panelTransform);
            }
        }
    }

    public void AddStructureDataToPanelChildButton(Transform panelTransform, List<StructureBaseSO> dataToShow, Action<string> callback)
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

    public void AddStructureDataToPanelChildText(Transform panelTransform, List<StructureBaseSO> structureData)
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

    public void AddStructureDataToPanelChildImage(Transform panelTransform, List<StructureBaseSO> structureData)
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

    public void SetBackButtonActionInPanel(Transform panelTransform, Action callback)
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

    public void SetCancelButtonActionInPanel(Transform panelTransform, Action callback)
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
}
