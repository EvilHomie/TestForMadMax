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
        _cardText.text = $"{upgradeCardData.UpgradeText} {upgradeCardData.ChangeValue}";
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
