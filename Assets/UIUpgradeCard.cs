using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UIUpgradeCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] TextMeshProUGUI _cardText;

    UpgradeCardData _upgradeCardData;
    public void ConfigCard(UpgradeCardData upgradeCardData)
    {
        _upgradeCardData = upgradeCardData;
        _cardText.text = $"{upgradeCardData.UpgradeText} {upgradeCardData.ChangeValue}";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (_upgradeCardData.UpgradeItemType == UpgradeItemType.Weapon)
        {
            SurviveModeManager.Instance.OnSelectWeaponUpgradeCard(_upgradeCardData);
        }
        else
        {
            SurviveModeManager.Instance.OnSelectVehicleUpgradeCard(_upgradeCardData);
        }
        SurviveModeUpgradePanel.Instance.OnCardSelected();
    }
}
