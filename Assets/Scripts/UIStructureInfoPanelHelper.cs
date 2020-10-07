using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIStructureInfoPanelHelper : MonoBehaviour
{
    public TextMeshProUGUI nameText, residentsText, incomeText, upkeepText, upgradeAmountText, upgradedText, clientText, materialBuildTimeText;
    public Toggle powerToggle, waterToggle, roadToggle, siloToggle, upgradeToggle, heatlhcareToggle, lawEnforcementToggle, fireProtectionToggle, postalServiceToggle, bankingServiceToggle, garbageServiceToggle;

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
        HideElement(postalServiceToggle.gameObject);
        HideElement(lawEnforcementToggle.gameObject);
        HideElement(fireProtectionToggle.gameObject);
        HideElement(bankingServiceToggle.gameObject);
        HideElement(garbageServiceToggle.gameObject);
        HideElement(heatlhcareToggle.gameObject);
        HideElement(roadToggle.gameObject);
        HideElement(siloToggle.gameObject);
        HideElement(upgradeAmountText.gameObject);
        HideElement(upgradedText.gameObject);
        HideElement(upgradeToggle.gameObject);
        HideElement(residentsText.gameObject);
        HideElement(incomeText.gameObject);
        HideElement(materialBuildTimeText.gameObject);
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
        CheckStructureToDisplayMaterialBuildTimeText(data);
        CheckStructureToDisplayUpgradeAmountText(data);
        CheckStructureToDisplayResidentsText(data);
        CheckStructureToDisplayPowerToggle(data);
        CheckStructureToDisplayRoadToggle(data);
        CheckStructureToDisplayWaterToggle(data);
        CheckStructureToDisplayUpgradeToggle(data);
        CheckStructureToDisplaySiloToggle(data);
        CheckStructureToDisplayLawEnforcementToggle(data);
        CheckStructureToDisplayFireProtectionToggle(data);
        CheckStructureToDisplayHealthcareToggle(data);
        CheckStructureToDisplayPostalServiceToggle(data);
        CheckStructureToDisplayBankingServiceToggle(data);
        CheckStructureToDisplayGarbageServiceToggle(data);
    }

    public void DisplayFacilityStructureInfo(SingleFacilitySO data)
    {
        Show();
        HideElement(upkeepText.gameObject);
        HideElement(residentsText.gameObject);
        SetText(nameText, data.buildingName);
        SetText(incomeText, data.GetIncome() + "");
        SetText(clientText, data.GetNumberOfCustomers() + "/" + data.maxCustomers);
        CheckStructureToDisplayMaterialBuildTimeText(data);
        CheckStructureToDisplayUpgradeAmountText(data);
        CheckStructureToDisplayPowerToggle(data);
        CheckStructureToDisplayRoadToggle(data);
        CheckStructureToDisplayWaterToggle(data);
        CheckStructureToDisplayUpgradeToggle(data);
        CheckStructureToDisplaySiloToggle(data);
        CheckStructureToDisplayLawEnforcementToggle(data);
        CheckStructureToDisplayFireProtectionToggle(data);
        CheckStructureToDisplayHealthcareToggle(data);
        CheckStructureToDisplayPostalServiceToggle(data);
        CheckStructureToDisplayBankingServiceToggle(data);
        CheckStructureToDisplayGarbageServiceToggle(data);
    }

    public void DisplayManufactureStructureInfo(ManufacturerBaseSO data)
    {
        Show();
        HideElement(incomeText.gameObject);
        HideElement(upkeepText.gameObject);
        HideElement(clientText.gameObject);
        HideElement(residentsText.gameObject);
        SetText(nameText, data.buildingName);
        CheckStructureToDisplayMaterialBuildTimeText(data);
        CheckStructureToDisplayUpgradeAmountText(data);
        CheckStructureToDisplayPowerToggle(data);
        CheckStructureToDisplayRoadToggle(data);
        CheckStructureToDisplayWaterToggle(data);
        CheckStructureToDisplayUpgradeToggle(data);
        CheckStructureToDisplaySiloToggle(data);
        CheckStructureToDisplayLawEnforcementToggle(data);
        CheckStructureToDisplayFireProtectionToggle(data);
        CheckStructureToDisplayHealthcareToggle(data);
        CheckStructureToDisplayPostalServiceToggle(data);
        CheckStructureToDisplayBankingServiceToggle(data);
        CheckStructureToDisplayGarbageServiceToggle(data);
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
        SetToggle(upgradeToggle, structure.HasFullyUpgraded());
        if (structure.HasFullyUpgraded() == true)
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

    private void CheckStructureToDisplayLawEnforcementToggle(StructureBaseSO structure)
    {
        if (structure.requireLawEnforcement)
        {
            SetToggle(lawEnforcementToggle, structure.HasLawEnforcement());
        }
        else
        {
            HideElement(lawEnforcementToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayFireProtectionToggle(StructureBaseSO structure)
    {
        if (structure.requireFireProtection)
        {
            SetToggle(fireProtectionToggle, structure.HasFireProtection());
        }
        else
        {
            HideElement(fireProtectionToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayHealthcareToggle(StructureBaseSO structure)
    {
        if (structure.requireHealthcare)
        {
            SetToggle(heatlhcareToggle, structure.HasHealthcare());
        }
        else
        {
            HideElement(heatlhcareToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayPostalServiceToggle(StructureBaseSO structure)
    {
        if (structure.requirePostalService)
        {
            SetToggle(postalServiceToggle, structure.HasPostalService());
        }
        else
        {
            HideElement(postalServiceToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayBankingServiceToggle(StructureBaseSO structure)
    {
        if(structure.requireBankService)
        {
            SetToggle(bankingServiceToggle, structure.HasBankingService());
        }
        else
        {
            HideElement(bankingServiceToggle.gameObject);
        }
    }

    private void CheckStructureToDisplayGarbageServiceToggle(StructureBaseSO structure)
    {
        if(structure.requireGarbageService)
        {
            SetToggle(garbageServiceToggle, structure.HasGarbageService());
        }
        else
        {
            HideElement(garbageServiceToggle.gameObject);
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

    private void CheckStructureToDisplayMaterialBuildTimeText(StructureBaseSO structure)
    {
        if (structure.GetType() == typeof(ManufacturerBaseSO))
        {
            SetText(materialBuildTimeText, ((ManufacturerBaseSO)structure).GetMaterialBuildTimer() + "'s");
        }
        else
        {
            HideElement(materialBuildTimeText.gameObject);
        }
    }

    private void CheckStructureToDisplayUpgradeAmountText(StructureBaseSO structure)
    {
        if (structure.upgradable == true)
        {
            SetText(upgradeAmountText, structure.GetUpgradePlacementCost() + "");
        }
        else
        {
            HideElement(upgradeAmountText.gameObject);
        }
    }
}