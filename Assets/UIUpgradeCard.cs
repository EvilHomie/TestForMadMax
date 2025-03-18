using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIUpgradeCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI _cardText;
    [SerializeField] Image _cardIcon;

    UpgradeCardData _upgradeCardData;
    public void ConfigCard(UpgradeCardData upgradeCardData)
    {
        _upgradeCardData = upgradeCardData;
        _cardIcon.sprite = upgradeCardData.Icon;
        _cardText.text = $"{upgradeCardData.UpgradeText} {upgradeCardData.ChangeValue}";
    }

    public void OnPointerClick(PointerEventData eventData)
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
