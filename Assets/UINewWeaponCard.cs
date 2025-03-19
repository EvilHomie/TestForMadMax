using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UINewWeaponCard : MonoBehaviour
{
    [SerializeField] Image _newWeaponImage;
    [SerializeField] TextMeshProUGUI _cardText;
    [SerializeField] Button _btn;

    private void OnEnable()
    {
        _btn.onClick.AddListener(OnPointerClick);
    }

    private void OnDisable()
    {
        _btn.onClick.RemoveAllListeners();
    }
    public void ConfigCard(string weaponName)
    {
        _newWeaponImage.sprite = GameAssets.Instance.GameItems.ItemsSpritesAtlas.GetSprite(weaponName);
        _cardText.text = $"Получить \"{weaponName}\"";
    }

    public void OnPointerClick()
    {
        SurviveModeUpgradePanel.Instance.OnWeaponSelectConfirm();
    }
}
