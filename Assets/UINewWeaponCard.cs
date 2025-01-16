using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UINewWeaponCard : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] Image _newWeaponImage;
    [SerializeField] TextMeshProUGUI _cardText;
    public void ConfigCard(string weaponName)
    {
        _newWeaponImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName);
        _cardText.text = $"Получить \"{weaponName}\"";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SurviveModeUpgradePanel.Instance.OnWeaponSelectConfirm();
    }
}
