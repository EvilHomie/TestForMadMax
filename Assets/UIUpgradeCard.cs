using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUpgradeCard : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _cardText;
    [SerializeField] Image _cardIcon;
    [SerializeField] Button _btn;

    UpgradeCardData _upgradeCardData;

    private void OnEnable()
    {
        _btn.onClick.AddListener(OnPointerClick);
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveAllListeners();
    }
    public void ConfigCard(UpgradeCardData upgradeCardData)
    {
        _upgradeCardData = upgradeCardData;
        _cardIcon.sprite = upgradeCardData.Icon;

        string plusSimbol = upgradeCardData.ChangeValue > 0 ? "+" : "";

        switch (upgradeCardData.CharacteristicsName)
        {
            case CharacteristicsName.WeaponKineticDmg:
                _cardText.text = $"{TextConstants.UPGRADEDAMAGE} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.WeaponFireRate:
                _cardText.text = $"{TextConstants.UPGRADEFIRERATE} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.WeaponReloadTime:
                _cardText.text = $"{TextConstants.UPGRADERELOADTIME} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.WeaponMagCapacity:
                _cardText.text = $"{TextConstants.UPGRADECAPACITY} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.VehicleHullHP:
                _cardText.text = $"{TextConstants.UPGRADEHULLHP} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.VehicleShieldHP:
                _cardText.text = $"{TextConstants.UPGRADESHIELDHP} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            case CharacteristicsName.VehicleShieldRegRate:
                _cardText.text = $"{TextConstants.UPGRADESHIELDREGRATE} {plusSimbol} {upgradeCardData.ChangeValue}";
                break;
            default:
                break;
        }
    }

    void OnPointerClick()
    {
        if (_upgradeCardData.UpgradeItemType == UpgradeItemType.Weapon)
        {
            SurviveModeUpgradeService.Instance.OnWeaponUpgrade(_upgradeCardData);
        }
        else
        {
            SurviveModeUpgradeService.Instance.OnVehicleUpgrade(_upgradeCardData);
        }
        SurviveModeUpgradePanel.Instance.OnCardSelected();
    }
}
