using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructureInfoPanelHelper : MonoBehaviour
{
    public TextMeshProUGUI nameText, residentsText, incomeText, upkeepText, upgradeAmountText, upgradedText, clientText;
    public Toggle powerToggle, waterToggle, roadToggle, siloToggle, upgradeToggle;

    // Start is called before the first frame update
    void Start()
    {
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void DisplayBasicStructureInfo(StructureBaseSO data)
    {
        Show();
        HideElement(clientText.gameObject);
        HideElement(powerToggle.gameObject);
        HideElement(waterToggle.gameObject);
        HideElement(roadToggle.gameObject);
        HideElement(siloToggle.gameObject);
        HideElement(upgradeAmountText.gameObject);
        HideElement(upgradedText.gameObject);
        HideElement(upgradeToggle.gameObject);
        HideElement(residentsText.gameObject);
        HideElement(incomeText.gameObject);
        SetText(nameText, data.buildingName);
        SetText(upkeepText, data.upkeepCost + "");
    }

    public void DisplayZoneStructureInfo(ZoneStructureSO data)
    {
        Show();
        HideElement(clientText.gameObject);
        HideElement(upkeepText.gameObject);
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(upgradeAmountText, data.GetUpgradePlacementCost() + "");
        CheckStructureToDisplayResidentsText(data);
        CheckStructureToDisplayPowerToggle(data);
        CheckStructureToDisplayRoadToggle(data);
        CheckStructureToDisplayWaterToggle(data);
        CheckStructureToDisplayUpgradeToggle(data);
        CheckStructureToDisplaySiloToggle(data);
    }

    public void DisplayFacilityStructureInfo(SingleFacilitySO data)
    {
        Show();
        HideElement(upkeepText.gameObject);
        HideElement(residentsText.gameObject);
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(upgradeAmountText, data.GetUpgradePlacementCost() + "");
        SetText(clientText, data.GetNumberOfCustomers() + "/" + data.maxCustomers);
        CheckStructureToDisplayPowerToggle(data);
        CheckStructureToDisplayRoadToggle(data);
        CheckStructureToDisplayWaterToggle(data);
        CheckStructureToDisplayUpgradeToggle(data);
        CheckStructureToDisplaySiloToggle(data);
    }

    private void HideElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(false);
    }

    private void ShowElement(GameObject element)
    {
        element.transform.parent.gameObject.SetActive(true);
    }

    private void SetText(TextMeshProUGUI element, string value)
    {
        ShowElement(element.gameObject);
        element.text = value;
    }

    private void SetToggle(Toggle element, bool value)
    {
        ShowElement(element.gameObject);
        element.isOn = value;
    }

    private void CheckStructureToDisplayPowerToggle(StructureBaseSO structure)
    {
        if (structure.requirePower)
        {
            SetToggle(powerToggle, structure.HasPower());
        }
        else
        {
            HideElement(powerToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayRoadToggle(StructureBaseSO structure)
    {
        if (structure.requireRoadAccess)
        {
            SetToggle(roadToggle, structure.HasRoadAccess());
        }
        else
        {
            HideElement(roadToggle.gameObject);
        }
    }

    private void CheckStructureToDisplaySiloToggle(StructureBaseSO structure)
    {
        if (structure.requireSilo)
        {
            SetToggle(siloToggle, structure.HasSilo());
        }
        else
        {
            HideElement(siloToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayUpgradeToggle(StructureBaseSO structure)
    {
        SetToggle(upgradeToggle, structure.HasUpgraded());
        if (structure.HasUpgraded() == true)
        {
            HideElement(upgradeAmountText.gameObject);
        }
        else
        {
            HideElement(upgradeToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayWaterToggle(StructureBaseSO structure)
    {
        if (structure.requireWater)
        {
            SetToggle(waterToggle, structure.HasWater());
        }
        else
        {
            HideElement(waterToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayResidentsText(ZoneStructureSO structure)
    {
        if (structure.zoneType == ZoneType.Residential)
        {
            SetText(residentsText, structure.GetResidentsAmount() + "");
        }
        else
        {
            HideElement(residentsText.gameObject);
        }
    }
}